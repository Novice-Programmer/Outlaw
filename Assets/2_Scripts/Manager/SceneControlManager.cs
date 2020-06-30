using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneControlManager : MonoBehaviour
{
    public enum eSceneType
    {
        START = 0,
        LOBBY,
        INGAME
    }

    public enum eLoaddingState
    {
        None = 0,
        LoadSceneStart,
        LoaddingScene,
        LoadSceneEnd,
        UnloadStageStart,
        UnloaddingStage,
        UnloadStageEnd,
        LoadStageStart,
        LoaddingStage,
        LoadStageEnd,
        LoadEnd
    }

    [SerializeField] GameObject _prefabLoading = null;

    eSceneType _nowSceneType;
    eSceneType _oldSceneType;

    eLoaddingState _currentStateLoad;

    public eLoaddingState _nowLoaddingState
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
        StartSceneIngame();
    }

    public void StartSceneLobby()
    {

    }

    public void StartSceneIngame(int stageNum = 1)
    {
        _oldSceneType = _nowSceneType;
        _nowSceneType = eSceneType.INGAME;
        _oldStageNumber = _nowStageNumber;
        _nowStageNumber = stageNum;
        StartCoroutine(LoaddingScene("IngameScene", stageNum));
    }

    IEnumerator LoaddingScene(string sceneName,int stageNum)
    {
        // 씬을 로드한다.
        // IngameScene일 경우 스테이지를 로드한다. 그리고 SetActiveScene을 StageScene으로 한다.

        Loading wnd = Instantiate(_prefabLoading,transform).GetComponent<Loading>();

        AsyncOperation aOper;
        Scene aScene;

        _currentStateLoad = eLoaddingState.LoadSceneStart;
        aOper = SceneManager.LoadSceneAsync(sceneName);
        while (!aOper.isDone)
        {
            _currentStateLoad = eLoaddingState.LoaddingScene;
            wnd.SettingLoadRate(aOper.progress);
            yield return null;
        }
        wnd.SettingLoadRate(1);

        aScene = SceneManager.GetSceneByName(sceneName);
        _currentStateLoad = eLoaddingState.LoadSceneEnd;

        if(_nowSceneType == eSceneType.INGAME)
        {
            string stageName = "Stage";
            _currentStateLoad = eLoaddingState.LoadSceneStart;
            aOper = SceneManager.LoadSceneAsync(stageName + stageNum.ToString(), LoadSceneMode.Additive);
            while (!aOper.isDone)
            {
                _currentStateLoad = eLoaddingState.LoaddingStage;
                wnd.SettingLoadRate(aOper.progress);
                yield return null;
            }
            aScene = SceneManager.GetSceneByName(stageName + stageNum.ToString());
            _currentStateLoad = eLoaddingState.LoadStageEnd;
        }
        wnd.SettingLoadRate(1);
        yield return new WaitForSeconds(5.0f);
        Destroy(wnd.gameObject);

        SceneManager.SetActiveScene(aScene);
        _currentStateLoad = eLoaddingState.LoadEnd;
    }
}
