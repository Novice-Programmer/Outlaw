using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Outlaw
{
    public class SlotUI : MonoBehaviour
    {
        [SerializeField] Image _slotIcon = null;
        [SerializeField] Image _imgSelect = null;
        [SerializeField] Image _imgCover = null;

        BaseWindow _ownerWnd;
        int _no = 0;

        public int _myNumber
        {
            get { return _no; }
        }

        public void DisableSelect()
        {
            _imgSelect.gameObject.SetActive(false);
        }

        public void EnabledCover(bool isOn)
        {
            _imgCover.gameObject.SetActive(isOn);
        }

        public void InitIcon(Sprite icon, BaseWindow wnd, int number, bool isClear)
        {
            _ownerWnd = wnd;
            _slotIcon.sprite = icon;
            _no = number;
            DisableSelect();
            EnabledCover(isClear);
        }

        public void OnClick()
        {
            if (!_imgSelect.gameObject.activeSelf && !_imgCover.gameObject.activeSelf)
            {
                _imgSelect.gameObject.SetActive(true);
                // 내 상위 스크립트에 내가 선택 되었다고 알려줘야 함.
                _ownerWnd.SelectAllCheck(_no);
            }
        }
    }
}