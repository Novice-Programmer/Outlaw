using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlaw
{
    public class IngameManager : MonoBehaviour
    {
        Transform _playerSpawnPos;
        GameObject _prefabPlayer;
        GameObject _prefabResultWindow = null;
        GameObject _stickWindow;
        GameObject _miniStatusWindow;
        GameObject _minimapWindow;
        GameObject _timeWidnow;
        MessageBox _msgBox;

        MinimapController _minimapController;
        TimeWindow _timeWnd;
        ScoreWindow _scoreWnd;
        GoalWindow _goalWnd;

        Player _player;

        EGameState _currentGameState = EGameState.None;

        public EGameState _nowGameState => _currentGameState;

        float _timeCheck = 0;
        float _gameTime = 0;
        int _monsterkillCount = 0;
        int _brokenScore = 0;
        int _animalKillCount = 0;
        int _totalScore = 0;
        bool _isWin = false;

        public int _countMonsterKill
        {
            set
            {
                _monsterkillCount = value;
                ScoreCal(EActionScore.MonsterKill);
            }
            get { return _monsterkillCount; }
        }

        public int _scoreBroken
        {
            set
            {
                _brokenScore += value;
                ScoreCal(EActionScore.BrokenObject, value);
            }
            get { return _brokenScore; }
        }

        public int _countAnimalKill
        {
            set
            {
                _animalKillCount = value;
                ScoreCal(EActionScore.AnimalKill);
            }
            get { return _animalKillCount; }
        }

        public int _scoreTotal
        {
            set { _totalScore = value; }
            get { return _totalScore; }
        }

        static IngameManager _uniqueInstance;

        public static IngameManager Instance
        {
            get { return _uniqueInstance; }
        }

        void Awake()
        {
            _uniqueInstance = this;
            _prefabPlayer = Resources.Load("Prefabs/Characters/PlayerObject") as GameObject;
            _prefabResultWindow = Resources.Load("Prefabs/UI/ResultWindow") as GameObject;
        }

        private void Start()
        {
        }

        void Update()
        {
            switch (_currentGameState)
            {
                case EGameState.None:
                    if (SceneControlManager.Instance._nowLoaddingState == ELoaddingState.None)
                    {
                        _currentGameState = EGameState.Ready;
                        GameSetting();
                    }
                    break;
                case EGameState.Ready:
                    _timeCheck += Time.deltaTime;
                    if (_timeCheck >= 1.0f)
                    {
                        _timeCheck = 0.0f;
                        SpawnPlayer();
                    }
                    break;
                case EGameState.SpawnPlayer:
                    _timeCheck += Time.deltaTime;
                    if (_timeCheck >= 2.0f)
                    {
                        _timeCheck = 0.0f;
                        GameStart();
                    }
                    break;
                case EGameState.Start:
                    _timeCheck += Time.deltaTime;
                    if (_timeCheck >= 0.5f)
                    {
                        _timeCheck = 0.0f;
                        GamePlay();
                    }
                    break;
                case EGameState.Play:
                    _timeCheck += Time.deltaTime;
                    _timeWnd.TimeUpdate(_timeCheck);
                    if (StageManager.Instance._clearCheck)
                    {
                        GameEnd(true);
                    }
                    if (_player._isDead)
                    {
                        GameEnd(false);
                    }
                    break;
                case EGameState.End:
                    _timeCheck += Time.deltaTime;
                    if (_timeCheck >= 1.0f)
                    {
                        GameResult();
                    }
                    break;
            }
        }

        public void GameReady()
        {
            _currentGameState = EGameState.Ready;
            _msgBox.OpenMessageBox("포탈로 이동중", true);
            _playerSpawnPos = GameObject.Find("GameStartPos").transform;

            _minimapController.InitMarker();
        }

        public void SpawnPlayer()
        {
            _currentGameState = EGameState.SpawnPlayer;
            _msgBox.OpenMessageBox("행성에 도착", true);
            _miniStatusWindow.SetActive(true);
            GameObject go = Instantiate(_prefabPlayer, _playerSpawnPos.position, _playerSpawnPos.rotation);
            _player = go.GetComponent<Player>();
            CameraController cc = Camera.main.GetComponent<CameraController>();
            cc.SetPlayer(go);
            SoundManager.Instance.PlayEffectSound(ETypeEffectSound.Warp, _player.transform);
        }

        public void GameStart()
        {
            _currentGameState = EGameState.Start;
            _timeWidnow.SetActive(true);
            _msgBox.OpenMessageBox("미션 시작", true);
        }

        public void GamePlay()
        {
            _currentGameState = EGameState.Play;
            _stickWindow.SetActive(true);
            _minimapWindow.SetActive(true);
            _minimapController.InitPlayer();
            _player.SettingSticks();
            _msgBox.OpenMessageBox();
        }

        public void GameEnd(bool isWin)
        {
            _currentGameState = EGameState.End;
            _gameTime = _timeCheck;
            _timeCheck = 0;
            _isWin = isWin;
            if (isWin)
                _msgBox.OpenMessageBox("미션 클리어", true);
            else
                _msgBox.OpenMessageBox("플레이어 사망", true);
        }

        public void GameResult()
        {
            _currentGameState = EGameState.Result;
            _msgBox.OpenMessageBox();
            GameObject go = Instantiate(_prefabResultWindow);
            ResultWindow wnd = go.GetComponent<ResultWindow>();
            wnd.OpenWindow(_isWin, _gameTime, _monsterkillCount, _animalKillCount, TotalScore());
            StageManager.Instance.ClearStage();
            DataManager.Instance.MoneyAdd(_isWin, TotalScore());
        }

        void GameSetting()
        {
            _timeWidnow = GameObject.FindGameObjectWithTag("TimeWindow");
            _timeWnd = _timeWidnow.GetComponent<TimeWindow>();
            GameObject go = GameObject.Find("Score");
            _scoreWnd = go.GetComponent<ScoreWindow>();
            go = GameObject.FindGameObjectWithTag("MessageBox");
            _msgBox = go.GetComponent<MessageBox>();
            go = GameObject.FindGameObjectWithTag("GoalWindow");
            _goalWnd = go.GetComponent<GoalWindow>();
            go = GameObject.FindGameObjectWithTag("MinimapController");
            _minimapController = go.GetComponent<MinimapController>();
            _stickWindow = GameObject.FindGameObjectWithTag("StickControllerWindow");
            _miniStatusWindow = GameObject.FindGameObjectWithTag("MiniAvatarWindow");
            _minimapWindow = GameObject.FindGameObjectWithTag("MinimapWindow");

            _stickWindow.SetActive(false);
            _miniStatusWindow.SetActive(false);
            _minimapWindow.SetActive(false);
            _timeWidnow.SetActive(false);

            GameReady();
        }

        void ScoreCal(EActionScore action, int addScore = 0)
        {
            switch (action)
            {
                case EActionScore.MonsterKill:
                    if (!(StageManager.Instance._nowStageGoal == ETypeGoal.모든적을제거 || StageManager.Instance._nowStageGoal == ETypeGoal.보스처치))
                        _goalWnd.GoalUpdate(StageManager.Instance._goalString);
                    addScore = 5;
                    break;
                case EActionScore.AnimalKill:
                    _goalWnd.GoalUpdate("동물을 죽이지 마시오.", true);
                    addScore = -7;
                    break;
                case EActionScore.BrokenObject:
                    if (!(StageManager.Instance._nowStageGoal == ETypeGoal.일정건물파괴 || StageManager.Instance._nowStageGoal == ETypeGoal.특정건물파괴))
                        _goalWnd.GoalUpdate(StageManager.Instance._goalString);
                    break;
            }
            _scoreWnd.ScoreUpdate(_totalScore, addScore);
        }

        int TotalScore()
        {
            int totalScore = 0;
            totalScore += _monsterkillCount * 5;
            totalScore += _countAnimalKill * -7;
            totalScore += _brokenScore;
            return totalScore;
        }

        public void MapChange(MapSet mapSet)
        {
            _minimapController.InitMapData(mapSet);
        }

        public void ViewChange()
        {
            _player.transform.GetChild(0).localRotation = Quaternion.identity;
        }
    }
}