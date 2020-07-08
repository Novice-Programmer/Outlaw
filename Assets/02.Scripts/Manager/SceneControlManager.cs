using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Outlaw
{
    public class SceneControlManager : MonoBehaviour
    {
        [SerializeField] GameObject _prefabLoading = null;

        ESceneType _nowSceneType;
        ESceneType _oldSceneType;

        ELoaddingState _currentStateLoad;

        Loading _wnd;

        public ELoaddingState _nowLoaddingState
        {
            get { return _currentStateLoad; }
        }

        int _nowStageNumber, _oldStageNumber;
        float _timeCheck = 0;

        static SceneControlManager _uniqueInstance;

        public static SceneControlManager Instance
        {
            get { return _uniqueInstance; }
        }

        public int _stageNow
        {
            get { return _nowStageNumber; }
        }

        private void Awake()
        {
            _uniqueInstance = this;
            DontDestroyOnLoad(this);
        }

        void Start()
        {
            StartSceneLobby();
        }

        private void Update()
        {
            if(_currentStateLoad == ELoaddingState.LoadEnd)
            {
                _timeCheck += Time.deltaTime;
                if (_timeCheck > 2.0f)
                {
                    StopCoroutine("LoaddingScene");
                    _currentStateLoad = ELoaddingState.None;
                    Destroy(_wnd.gameObject);
                    _timeCheck = 0;
                }
            }
        }

        public void StartSceneLobby()
        {
            _oldSceneType = _nowSceneType;
            _nowSceneType = ESceneType.LOBBY;
            StartCoroutine(LoaddingScene("LobbyScene", 0));
        }

        public void StartSceneIngame(int stageNum, ETypePlanet planet = ETypePlanet.None)
        {
            _oldSceneType = _nowSceneType;
            _nowSceneType = ESceneType.INGAME;
            _oldStageNumber = _nowStageNumber;
            _nowStageNumber = stageNum;
            if (planet == ETypePlanet.None)
                DataManager.Instance.StageChange(DataManager.Instance._userData._nowStage._planet, stageNum);
            else
                DataManager.Instance.StageChange(planet, stageNum);
            StartCoroutine(LoaddingScene("IngameScene", stageNum));
        }

        IEnumerator LoaddingScene(string sceneName, int stageNum = 1)
        {
            // 씬을 로드한다.
            // IngameScene일 경우 스테이지를 로드한다. 그리고 SetActiveScene을 StageScene으로 한다.

            _wnd = Instantiate(_prefabLoading, transform).GetComponent<Loading>();

            if (stageNum == 0)
                _wnd.OpenLoaddingWnd(ELoadType.Lobby);
            else
                _wnd.OpenLoaddingWnd(ELoadType.Planet);

            AsyncOperation aOper;
            Scene aScene;

            _currentStateLoad = ELoaddingState.LoadSceneStart;
            aOper = SceneManager.LoadSceneAsync(sceneName);
            while (!aOper.isDone)
            {
                _currentStateLoad = ELoaddingState.LoaddingScene;
                _wnd.SettingLoadRate(aOper.progress);
                yield return null;
            }
            _wnd.SettingLoadRate(1);

            aScene = SceneManager.GetSceneByName(sceneName);
            _currentStateLoad = ELoaddingState.LoadSceneEnd;

            if (_nowSceneType == ESceneType.INGAME)
            {
                string stageName = "Stage";
                _currentStateLoad = ELoaddingState.LoadSceneStart;
                sceneName = stageName + stageNum.ToString();
                aOper = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
                while (!aOper.isDone)
                {
                    _currentStateLoad = ELoaddingState.LoaddingStage;
                    _wnd.SettingLoadRate(aOper.progress);
                    yield return null;
                }
                aScene = SceneManager.GetSceneByName(sceneName);
                _currentStateLoad = ELoaddingState.LoadStageEnd;
            }
            _wnd.SettingLoadRate(1);
            SceneManager.SetActiveScene(aScene);
            _currentStateLoad = ELoaddingState.LoadEnd;
        }
    }
}