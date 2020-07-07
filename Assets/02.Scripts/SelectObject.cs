using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlaw
{
    public class SelectObject : LobbyObject
    {
        [SerializeField] ETypeWindow _windowType = ETypeWindow.StageWnd;
        [SerializeField] ETypePlanet _stagePlanet = ETypePlanet.None;
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

        void WindowButton(LobbyPlayer lp)
        {
            switch (_windowType)
            {
                case ETypeWindow.StageWnd:
                    lp.SelectChange("행성 이동", this);
                    break;
                case ETypeWindow.CharacterInfoWnd:
                    lp.SelectChange("상태 확인", this);
                    break;
                case ETypeWindow.EnhanceWnd:
                    lp.SelectChange("업그레이드", this);
                    break;
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                _modelRenderer.material = _mats[(int)ESelectType.Select];
                LobbyPlayer lp = other.GetComponent<LobbyPlayer>();
                WindowButton(lp);
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
            LobbyManager.Instance.OpenWindow(_windowType, _stagePlanet);
        }
    }
}