using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlaw
{
    public class LobbyManager : MonoBehaviour
    {
        GameObject _prefabStageWindow;
        GameObject _prefabAvatarWindow;
        GameObject _startEffect;

        StageWindow _wndStage;
        AvatarWindow _wndAvatar;
        ShowTextUI _goldText;
        ShowTextUI _diamondText;

        UserInfo _userData;

        bool _firstCheck = false;

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

            SoundManager.Instance.PlayerBGMSound(ETypeBGMSound.Lobby);
            GameObject go = GameObject.Find("GoldWnd");
            _goldText = go.GetComponent<ShowTextUI>();
            go = GameObject.Find("DiamondWnd");
            _diamondText = go.GetComponent<ShowTextUI>();
            _userData = DataManager.Instance._userData;
            _goldText.SetGoodsValue(_userData._ownGold);
            _diamondText.SetGoodsValue(_userData._ownDiamond);
            _startEffect = GameObject.Find("PlayerStartEffect");
            _startEffect.SetActive(false);

        }

        // Update is called once per frame
        void Update()
        {
            if (!_firstCheck) 
            {
                if (SceneControlManager.Instance._nowLoaddingState == ELoaddingState.None)
                {
                    _firstCheck = true;
                    _startEffect.SetActive(true);
                }
            }
        }

        public void OpenWindow(ETypeWindow type,ETypePlanet planet = ETypePlanet.스탄피드)
        {
            GameObject go;
            switch (type)
            {
                case ETypeWindow.StageWnd:
                        go = Instantiate(_prefabStageWindow);
                        _wndStage = go.GetComponent<StageWindow>();
                        _wndStage.OpenWindow(planet);
                    break;
                case ETypeWindow.CharacterInfoWnd:

                        go = Instantiate(_prefabAvatarWindow);
                        _wndAvatar = go.GetComponent<AvatarWindow>();
                        _wndAvatar.OpenWindow();
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
                case ETypeWindow.EnhanceWnd:
                    break;
            }
        }
    }
}