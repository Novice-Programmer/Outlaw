using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyManager : MonoBehaviour
{
    public enum ETypeWindow
    {
        StageWnd = 0,
        CharacterInfoWnd,
        StoreWnd
    }

    GameObject _prefabStageWindow;
    GameObject _prefabAvatarWindow;

    StageWindow _wndStage;
    AvatarWindow _wndAvatar;

    static LobbyManager _uniqueInstance;

    public static LobbyManager Instance
    {
        get { return _uniqueInstance; }
    }

    private void Awake()
    {
        _uniqueInstance = this;
        _prefabStageWindow = Resources.Load("Prefabs/UI/StageWindow") as GameObject;
        _prefabAvatarWindow = Resources.Load("Prefabs/UI/AvatarWindow") as GameObject;
    }

    // Start is called before the first frame update
    void Start()
    {
#if UNITY_EDITOR
        if (SoundManager.Instance != null)
            SoundManager.Instance.PlayerBGMSound(SoundManager.ETypeBGMSound.Lobby);
#else
       SoundManager.Instance.PlayerBGMSound(SoundManager.ETypeBGMSound.Lobby); 
#endif
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
                if (_wndAvatar == null)
                {
                    go = Instantiate(_prefabAvatarWindow);
                    _wndAvatar = go.GetComponent<AvatarWindow>();
                    _wndAvatar.OpenWindow();
                }
                else
                {
                    if (_wndAvatar.gameObject.activeSelf)
                        _wndAvatar.CloseWnd();
                    else
                        _wndAvatar.OpenWindow();
                }
                break;
        }
    }

    public void CloseWindow(ETypeWindow type)
    {
        switch (type)
        {
            case ETypeWindow.StageWnd:
                if (_wndStage != null)
                    _wndStage.CloseWnd();
                break;
            case ETypeWindow.CharacterInfoWnd:
                if (_wndAvatar != null)
                    _wndAvatar.CloseWnd();
                break;
            case ETypeWindow.StoreWnd:
                break;
        }
    }
}
