using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Outlaw
{
    public class ResultWindow : MonoBehaviour
    {
        [SerializeField] Text _txtResult = null;
        [SerializeField] Text _txtSec = null;
        [SerializeField] Text _txtMSec = null;
        [SerializeField] Text _txtMonsterCount = null;
        [SerializeField] Text _txtAnimalKillCount = null;
        [SerializeField] Text _txtTotalScore = null;
        [SerializeField] GameObject _objNextStage = null;

        bool _isOpen = false;
        float _time = 0;
        float _openTotalTime = 3.0f;

        private void Update()
        {
            if (_isOpen)
            {
                _time -= Time.deltaTime;
                if (_time >= _openTotalTime)
                {
                    _isOpen = false;
                }
            }
        }

        // Start is called before the first frame update
        public void OpenWindow(bool isWin, float time, int monsterKill, int animalKill, int totalScore)
        {
            _txtResult.text = isWin ? "Win" : "Lose";
            int sec = (int)time;
            int msec = (int)((time - sec) * 100);
            _txtMSec.text = msec < 10 ? "0" : string.Empty;
            _txtSec.text = sec.ToString();
            _txtMSec.text += msec.ToString();

            _txtMonsterCount.text = monsterKill.ToString();
            _txtAnimalKillCount.text = animalKill.ToString();
            _txtTotalScore.text = totalScore.ToString();
            if (DataManager.Instance.StageCheck())
                _objNextStage.gameObject.SetActive(false);
        }

        public void ClickRestartButton()
        {
            SceneControlManager.Instance.StartSceneIngame(DataManager.Instance._userData._nowStage._no);
        }

        public void ClickHomeButton()
        {
            SceneControlManager.Instance.StartSceneLobby();
        }

        public void ClickNextButton()
        {
            SceneControlManager.Instance.StartSceneIngame(DataManager.Instance._userData._nowStage._no + 1);
        }
    }
}