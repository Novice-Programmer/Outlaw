using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Outlaw
{
    public class Loading : MonoBehaviour
    {
        [Range(0.1f, 0.5f)] [SerializeField] float _dotTime = 0.3f;
        [SerializeField] Image _imgLoading = null;
        [SerializeField] Text _txtLoading = null;
        [SerializeField] Text _txtLoadingValue = null;
        [SerializeField] Text _txtGoal = null;
        [SerializeField] Text _txtGoalMain = null;
        [SerializeField] GameObject _targetObject = null;
        float _rate = 0;
        float _timeCheck = 0;

        int _dotCount = 0;
        int _maxDotCount = 6;
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (_rate < 1)
            {
                if (_timeCheck > _dotTime)
                {
                    _timeCheck = 0;
                    _dotCount++;
                    if (_dotCount > _maxDotCount)
                        _dotCount = 0;
                    _txtLoading.text = "Loading.";
                    for (int i = 0; i < _dotCount; i++)
                    {
                        _txtLoading.text += ".";
                    }
                }
            }
        }

        public void OpenLoaddingWnd(ELoadType type, string planet = "", string stageName = "", string goalString = "")
        {
            _rate = 0;
            _txtLoadingValue.text = "0%";
            switch (type)
            {
                case ELoadType.Lobby:
                    _txtGoal.text = "귀환중";
                    _txtGoalMain.text = "우주 정거장 17-A\n다양한 지역으로 포탈 가능\n상태 점검, 부위 강화등\n가능하다.";
                    break;
                case ELoadType.Planet:
                    _txtGoal.text = "목표";
                    string goalMain = "";
                    goalMain += "행성 " + planet + "\n";
                    goalMain += "지역 " + stageName + "\n";
                    goalMain += goalString;
                    _txtGoalMain.text = goalMain;
                    _targetObject.SetActive(true);
                    break;
            }
        }

        public void SettingLoadRate(float rate)
        {
            _rate = rate;
            _imgLoading.fillAmount = rate;
            _txtLoadingValue.text = rate * 100 + "%";

            if (_rate == 1)
            {
                _txtLoading.text = "Just a moment, please";
            }
        }
    }
}