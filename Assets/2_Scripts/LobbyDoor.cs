using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlaw
{
    public class LobbyDoor : LobbyObject
    {
        [SerializeField] bool _isAnimDoor = false;

        Animator _doorCtrl;

        Door[] _doors = null;

        bool _isOpened = false;

        private void Awake()
        {
            if (!_isAnimDoor)
                _doors = transform.GetComponentsInChildren<Door>();
            else
                _doorCtrl = GetComponent<Animator>();
        }

        public override void Select()
        {
            _isOpened = !_isOpened;
            if (!_isAnimDoor)
            {
                for (int i = 0; i < _doors.Length; i++)
                {
                    _doors[i].DoorChange();
                }
            }
            else
            {
                _doorCtrl.SetBool("IsOpen", _isOpened);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                LobbyPlayer lp = other.GetComponent<LobbyPlayer>();
                if (_isOpened)
                    lp.SelectChange("문 닫기", this);
                else
                    lp.SelectChange("문 열기", this);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                LobbyPlayer lp = other.GetComponent<LobbyPlayer>();
                lp.SelectChange();
            }
        }
    }
}