using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlaw
{
    public class SelectObject : LobbyObject
    {
        enum ESelectType
        {
            Normal = 0,
            Select,
        }

        [SerializeField] LobbyManager.ETypeWindow _windowType = LobbyManager.ETypeWindow.StageWnd;
        [SerializeField] Material[] _mats = null;
        MeshRenderer _modelRenderer;

        private void Start()
        {
            _modelRenderer = transform.GetComponentInChildren<MeshRenderer>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _modelRenderer.material = _mats[(int)ESelectType.Select];
                LobbyPlayer lp = other.GetComponent<LobbyPlayer>();
                switch (_windowType)
                {
                    case LobbyManager.ETypeWindow.StageWnd:
                        lp.SelectChange("행성 이동", this);
                        break;
                    case LobbyManager.ETypeWindow.CharacterInfoWnd:
                        lp.SelectChange("상태 확인", this);
                        break;
                    case LobbyManager.ETypeWindow.StoreWnd:
                        lp.SelectChange("업그레이드", this);
                        break;
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _modelRenderer.material = _mats[(int)ESelectType.Normal];
                LobbyPlayer lp = other.GetComponent<LobbyPlayer>();
                lp.SelectChange();
                LobbyManager.Instance.CloseWindow(_windowType);
            }
        }

        public override void Select()
        {
            LobbyManager.Instance.OpenWindow(_windowType);
        }
    }
}