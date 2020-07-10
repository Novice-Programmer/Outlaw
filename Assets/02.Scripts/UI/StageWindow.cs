using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Outlaw
{
    public class StageWindow : BaseWindow
    {
        [SerializeField] Text _planetName = null;
        [SerializeField] Button _warpButton = null;
        [SerializeField] Transform _frameRoot = null;

        Canvas _canvas;

        int _pageNum;
        int _selectNum = 1;

        ETypePlanet _nowPlanet;

        GameObject _prefabSlot;

        List<StageInfo> _stages = new List<StageInfo>();
        List<SlotUI> _stageSlotList = new List<SlotUI>();
        private void Awake()
        {
            _canvas = GetComponent<Canvas>();
            _canvas.worldCamera = Camera.main;
            _prefabSlot = Resources.Load("Prefabs/UI/Slot") as GameObject;
        }
        // Start is called before the first frame update

        // Update is called once per frame
        void Update()
        {

        }

        public void OpenWindow(ETypePlanet planet)
        {
            _nowPlanet = planet;
            _planetName.text = planet.ToString();
            ListUpSlot();
        }

        public override void SelectAllCheck(int no)
        {
            _selectNum = no;
            _warpButton.interactable = true;
            for (int i = 0; i < _stageSlotList.Count; i++)
            {
                if(_stageSlotList[i]._myNumber == no)
                    continue;
                _stageSlotList[i].DisableSelect();
            }
        }

        void ListUpSlot()
        {
            _stages = DataManager.Instance.GetStages(_nowPlanet);

            for (int i = 0; i < _stages.Count; i++)
            {
                GameObject slotObject = Instantiate(_prefabSlot, _frameRoot.transform);
                SlotUI su = slotObject.GetComponent<SlotUI>();
                bool isClear = DataManager.Instance._userData._bestStageClear + 1 < _stages[i]._no;
                su.InitIcon(_stages[i]._stageIcon, this, _stages[i]._no, isClear);
                _stageSlotList.Add(su);
            }

        }

        public void CloseWnd()
        {
            Destroy(gameObject);
        }

        public void ClickStartBtn()
        {
            SoundManager.Instance.PlayEffectSound(ETypeEffectSound.WarpButton, Camera.main.transform);
            SceneControlManager.Instance.StartSceneIngame(_selectNum, _nowPlanet);
        }
    }
}