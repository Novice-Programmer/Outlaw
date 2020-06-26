using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameManager : MonoBehaviour
{
    public enum EGameState
    {
        Ready,
        PlayerSpawn,
        GameStart,
        Play,
        GameEnd,
        Result
    }

    [SerializeField] GameObject _playerSticks = null;
    [SerializeField] GameObject _playerMiniStatus = null;
    [SerializeField] GameObject _prefabResultWindow = null;

    List<SpawnControl> _spawnPointList = new List<SpawnControl>();
    MinimapController _minimapController;

    EGameState _nowGameState = EGameState.Ready;

    public EGameState _gameState => _nowGameState;

    bool _isWin = false;

    float _timeCheck = 0;
    [SerializeField] int _maxSpawnPoint = 3;
    int _killCount = 0;
    int _limitCount = 0;

    static IngameManager _uniqueInstance;

    public static IngameManager _instance
    {
        get { return _uniqueInstance; }
    }

    private void Awake()
    {
        _uniqueInstance = this;
    }
    void Start()
    {
        ListUpSpawnControl();
        Ready();
    }

    void Update()
    {
        switch (_nowGameState)
        {
            case EGameState.Ready:
                _timeCheck += Time.deltaTime;
                if(_timeCheck > 0.5f)
                {
                    _timeCheck = 0;
                    PlayerSpawn();
                }
                break;
            case EGameState.PlayerSpawn:
                _timeCheck += Time.deltaTime;
                if (_timeCheck > 0.5f)
                {
                    _timeCheck = 0;
                    GameStart();
                }
                break;
            case EGameState.Play:
                _timeCheck += Time.deltaTime;
                break;
        }
    }

    void Ready()
    {
        _nowGameState = EGameState.Ready;
        int removeCount = _spawnPointList.Count - _maxSpawnPoint;
        for(int i = 0; i < removeCount; i++)
        {
            int rid = Random.Range(0, _spawnPointList.Count);
            Destroy(_spawnPointList[rid].gameObject);
            _spawnPointList.RemoveAt(rid);
        }
        _minimapController = GameObject.FindGameObjectWithTag("MinimapController").GetComponent<MinimapController>();
        _minimapController.InitMarker();
    }

    void PlayerSpawn()
    {
        _nowGameState = EGameState.PlayerSpawn;
        Transform tr = GameObject.Find("PlayerSpawnPos").GetComponent<Transform>();
        GameObject player = Resources.Load("Prefabs/Character/PlayerObject") as GameObject;
        Instantiate(player, tr.position, player.transform.rotation);
        _playerSticks.SetActive(true);
        _playerMiniStatus.SetActive(true);
        CameraController camera = Camera.main.GetComponent<CameraController>();
        _minimapController.InitPlayer();
        camera.InitPlayer();
    }

    void GameStart()
    {
        _nowGameState = EGameState.GameStart;
        Play();
    }

    void Play()
    {
        _nowGameState = EGameState.Play;
    }

    void GameEnd()
    {
        _nowGameState = EGameState.GameEnd;
        GameObject go = Instantiate(_prefabResultWindow);
        ResultWindow wnd = go.GetComponent<ResultWindow>();
        wnd.OpenWindow(_isWin, _timeCheck, _killCount, _limitCount);
        Result();
    }

    void Result()
    {
        _nowGameState = EGameState.Result;
    }
    void ListUpSpawnControl()
    {
        SpawnControl[] scArray = FindObjectsOfType<SpawnControl>();

        for(int i = 0; i < scArray.Length; i++)
        {
            _spawnPointList.Add(scArray[i]);
        }
    }

    public void SpawnPointRemove(SpawnControl removeSpawn)
    {
        _spawnPointList.Remove(removeSpawn);
        Destroy(removeSpawn.gameObject);
        if(_spawnPointList.Count == 0)
        {
            _isWin = true;
            GameEnd();
        }
    }

    public void ReceivePlayerDie()
    {
        for(int i = 0; i < _spawnPointList.Count; i++)
        {
            _spawnPointList[i].AllNotificationPlayerDeath();
        }
        _isWin = false;
        GameEnd();
    }
}
