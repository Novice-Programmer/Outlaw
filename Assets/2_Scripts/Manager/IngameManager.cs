using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameManager : MonoBehaviour
{
    public enum EGameState
    {
        None = 0,
        Ready,
        SpawnPlayer,
        Start,
        Play,
        End,
        Result
    }

    enum EActionScore
    {
        MonsterKill,
        AnimalKill,
        BrokenObject
    }

    [SerializeField] GameObject _prefabResultWindow = null;

    List<SpawnControl> _spawnPointList = new List<SpawnControl>();
    List<Animal> _animals = new List<Animal>();
    Transform _playerSpawnPos;
    GameObject _prefabPlayer;
    GameObject _stickWindow;
    GameObject _miniStatusWindow;
    GameObject _minimapWindow;
    MessageBox _msgBox;

    MinimapController _minimapController;
    TimeWindow _timeWnd;
    ScoreWindow _scoreWnd;

    Player _player;
    EGameState _currentGameState = EGameState.None;

    public EGameState _nowGameState => _currentGameState;

    float _timeCheck = 0;
    float _gameTime = 0;
    [SerializeField] int _maxSpawnPoint = 3;
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
    }
    void Update()
    {
        switch (_currentGameState)
        {
            case EGameState.None:
                if (SceneControlManager.Instance._nowLoaddingState == SceneControlManager.eLoaddingState.LoadEnd)
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
                if (CheckClearConditions())
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
                if(_timeCheck >= 1.0f)
                {
                    GameResult();
                }
                break;
        }
    }

    public void GameReady()
    {
        _currentGameState = EGameState.Ready;
        _msgBox.OpenMessageBox("준비~!!", true);
        ListUpSpawnControl();
        _playerSpawnPos = GameObject.Find("GameStartPos").transform;
        int removeCount = _spawnPointList.Count - _maxSpawnPoint;
        for (int i = 0; i < removeCount; i++)
        {
            int rid = Random.Range(0, _spawnPointList.Count);
            Destroy(_spawnPointList[rid].gameObject);
            _spawnPointList.RemoveAt(rid);
        }

        _minimapController.InitMarker();
    }

    public void SpawnPlayer()
    {
        _currentGameState = EGameState.SpawnPlayer;
        _msgBox.OpenMessageBox("플레이어 등장~!!", true);
        _miniStatusWindow.SetActive(true);
        GameObject go = Instantiate(_prefabPlayer, _playerSpawnPos.position, _playerSpawnPos.rotation);
        _player = go.GetComponent<Player>();
        CameraController cc = Camera.main.GetComponent<CameraController>();
        cc.SetPlayer(go);
    }

    public void GameStart()
    {
        _currentGameState = EGameState.Start;

        _msgBox.OpenMessageBox("플레이 시작!!!", true);
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
            _msgBox.OpenMessageBox("목표 달성~!!", true);
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
        // 모든 몬스터, 동물 삭제
        for(int i = 0; i < _spawnPointList.Count; i++)
        {
            _spawnPointList[i].MonsterDestroy();
        }
        
    }
    void ListUpSpawnControl()
    {
        SpawnControl[] scArray = FindObjectsOfType<SpawnControl>();

        for (int i = 0; i < scArray.Length; i++)
        {
            _spawnPointList.Add(scArray[i]);
        }
    }

    void GameSetting()
    {
        GameObject go = GameObject.Find("Time");
        _timeWnd = go.GetComponent<TimeWindow>();
        go = GameObject.Find("Score");
        _scoreWnd = go.GetComponent<ScoreWindow>();
        go = GameObject.FindGameObjectWithTag("MessageBox");
        _msgBox = go.GetComponent<MessageBox>();
        go = GameObject.FindGameObjectWithTag("MinimapController");
        _minimapController = go.GetComponent<MinimapController>();
        _stickWindow = GameObject.FindGameObjectWithTag("StickControllerWindow");
        _miniStatusWindow = GameObject.FindGameObjectWithTag("MiniAvatarWindow");
        _minimapWindow = GameObject.FindGameObjectWithTag("MinimapWindow");

        _stickWindow.SetActive(false);
        _miniStatusWindow.SetActive(false);
        _minimapWindow.SetActive(false);

        GameObject[] animalObjects = GameObject.FindGameObjectsWithTag("Animal");
        for (int i = 0; i < animalObjects.Length; i++)
        {
            Animal animal = animalObjects[i].GetComponent<Animal>();
            _animals.Add(animal);
        }

        GameReady();
    }

    bool CheckClearConditions()
    {
        int cnt = 0;
        foreach(SpawnControl item in _spawnPointList)
        {
            if (item._checkRemainingCount)
                cnt++;
        }

        if (cnt >= _maxSpawnPoint)
            return true;
        else
            return false;
    }

    void ScoreCal(EActionScore action,int addScore = 0)
    {
        switch (action)
        {
            case EActionScore.MonsterKill:
                addScore = 5;
                break;
            case EActionScore.AnimalKill:
                addScore = -7;
                break;
            case EActionScore.BrokenObject:
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

    void AnimalDestroy()
    {
        for(int i = 0; i < _animals.Count; i++)
        {
            if (_animals[i] != null)
            {
                Destroy(_animals[i]);
            }
        }
    }

    public void ReceivePlayerDie()
    {
        for (int i = 0; i < _spawnPointList.Count; i++)
        {
            _spawnPointList[i].AllNotificationPlayerDeath();
        }
    }

    public void MapChange(MapSet mapSet)
    {
        _minimapController.InitMapData(mapSet);
    }
}