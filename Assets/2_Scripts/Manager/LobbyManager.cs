using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public enum ETypeWindow
    {
        StageWnd = 0,
        CharacterInfoWnd
    }

    GameObject _prefabStageWindow;

    StageWindow _wndStage;

    GameObject _leftDoor;
    GameObject _rightDoor;

    static LobbyManager _uniqueInstance;

    public static LobbyManager Instance
    {
        get { return _uniqueInstance; }
    }

    private void Awake()
    {
        _uniqueInstance = this;
        _leftDoor = GameObject.Find("LeftDoor");
        _rightDoor = GameObject.Find("RightDoor");
        _prefabStageWindow = Resources.Load("Prefabs/UI/StageWindow") as GameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
        _leftDoor.GetComponent<Door>().OpenDoor();
        _rightDoor.GetComponent<Door>().OpenDoor();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void OpenWindow(ETypeWindow type)
    {
        GameObject go;
        switch (type)
        {
            case ETypeWindow.StageWnd:
                if(_wndStage == null)
                {
                    go = Instantiate(_prefabStageWindow);
                    _wndStage = go.GetComponent<StageWindow>();
                    _wndStage.OpenWindow();
                }
                else
                {
                    if (_wndStage.gameObject.activeSelf)
                        _wndStage.CloseWnd();
                    else
                        _wndStage.OpenWindow();
                }
                break;
            case ETypeWindow.CharacterInfoWnd:
                break;
        }
    }
}
