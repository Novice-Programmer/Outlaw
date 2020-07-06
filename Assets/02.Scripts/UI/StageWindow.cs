using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlaw
{
    public class StageWindow : BaseWindow
    {
        // 임시
        [SerializeField] Sprite _stageImg = null;
        //

        [SerializeField] Transform _frameRoot = null;

        Canvas _canvas;

        int _pageNum;
        int _selectNum = 1;

        List<SlotUI> _stageSlotList = new List<SlotUI>();
        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _canvas.worldCamera = Camera.main;
        }
        // Start is called before the first frame update

        // Update is called once per frame
        void Update()
        {

        }

        public void OpenWindow()
        {
            gameObject.SetActive(true);
            ListUpSlot();

            bool first = false;

            for (int i = 0; i < _stageSlotList.Count; i++)
            {
                _stageSlotList[i].InitIcon(_stageImg, this, i + 1, first);
            }
        }

        public override void SelectAllCheck(int no)
        {
            _selectNum = no;
            for (int i = 0; i < _stageSlotList.Count; i++)
            {
                if (i + 1 == no)
                    continue;
                _stageSlotList[i].DisableSelect();
            }
        }

        void ListUpSlot()
        {
            _stageSlotList.Clear();
            for (int i = 0; i < _frameRoot.childCount; i++)
            {
                SlotUI su = _frameRoot.GetChild(i).GetComponent<SlotUI>();
                _stageSlotList.Add(su);
            }
        }

        public void CloseWnd()
        {
            gameObject.SetActive(false);
        }

        public void ClickStartBtn()
        {
            SceneControlManager.Instance.StartSceneIngame(_selectNum);
        }

        public void ClickCloseBtn()
        {
            Destroy(gameObject);
        }
    }
}