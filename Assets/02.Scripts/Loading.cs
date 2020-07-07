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

        public void OpenLoaddingWnd(ELoadType type)
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
                    StageInfo nowStage = DataManager.Instance._userData._nowStage;
                    string goalMain = "";
                    goalMain += "행성 " + nowStage._planet + "\n";
                    goalMain += "지역 " + nowStage._stageName + "\n";
                    string goalString = "";
                    switch (nowStage._goal)
                    {
                        case ETypeGoal.모든적을제거:
                            goalString = "모든 적을 제거하시오.";
                            break;
                        case ETypeGoal.특정지역방문:
                            goalString = "특정 지역을 방문하시오.";
                            break;
                        case ETypeGoal.특정건물파괴:
                            goalString = "특정 건물을 파괴하시오.";
                            break;
                        case ETypeGoal.일정건물파괴:
                            goalString = "건물을 일정량만큼 파괴하시오.";
                            break;
                        case ETypeGoal.보스처치:
                            goalString = "몬스터를 잡고 보스를 처치하시오.";
                            break;
                    }
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