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

        public ELoaddingState _nowLoaddingState
        {
            get { return _currentStateLoad; }
        }

        int _nowStageNumber, _oldStageNumber;

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

        public void StartSceneLobby()
        {
            _oldSceneType = _nowSceneType;
            _nowSceneType = ESceneType.LOBBY;
            StartCoroutine(LoaddingScene("LobbyScene", 0));
        }

        public void StartSceneIngame(int stageNum)
        {
            _oldSceneType = _nowSceneType;
            _nowSceneType = ESceneType.INGAME;
            _oldStageNumber = _nowStageNumber;
            _nowStageNumber = stageNum;
            StartCoroutine(LoaddingScene("IngameScene", stageNum));
        }

        IEnumerator LoaddingScene(string sceneName, int stageNum = 1)
        {
            // 씬을 로드한다.
            // IngameScene일 경우 스테이지를 로드한다. 그리고 SetActiveScene을 StageScene으로 한다.

            Loading wnd = Instantiate(_prefabLoading, transform).GetComponent<Loading>();

            if (stageNum == 0)
                wnd.OpenLoaddingWnd(ELoadType.Lobby);
            else
                wnd.OpenLoaddingWnd(ELoadType.Planet);

            AsyncOperation aOper;
            Scene aScene;

            _currentStateLoad = ELoaddingState.LoadSceneStart;
            aOper = SceneManager.LoadSceneAsync(sceneName);
            while (!aOper.isDone)
            {
                _currentStateLoad = ELoaddingState.LoaddingScene;
                wnd.SettingLoadRate(aOper.progress);
                yield return null;
            }
            wnd.SettingLoadRate(1);

            aScene = SceneManager.GetSceneByName(sceneName);
            _currentStateLoad = ELoaddingState.LoadSceneEnd;

            if (_nowSceneType == ESceneType.INGAME)
            {
                string stageName = "Stage";
                _currentStateLoad = ELoaddingState.LoadSceneStart;
                aOper = SceneManager.LoadSceneAsync(stageName + stageNum.ToString(), LoadSceneMode.Additive);
                while (!aOper.isDone)
                {
                    _currentStateLoad = ELoaddingState.LoaddingStage;
                    wnd.SettingLoadRate(aOper.progress);
                    yield return null;
                }
                aScene = SceneManager.GetSceneByName(stageName + stageNum.ToString());
                _currentStateLoad = ELoaddingState.LoadStageEnd;
            }
            wnd.SettingLoadRate(1);
            yield return new WaitForSeconds(2.0f);
            Destroy(wnd.gameObject);

            SceneManager.SetActiveScene(aScene);
            _currentStateLoad = ELoaddingState.LoadEnd;
        }
    }
}