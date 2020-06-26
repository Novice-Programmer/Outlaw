using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ResultWindow : MonoBehaviour
{
    [SerializeField] Text _txtResult;
    [SerializeField] Text _txtSec;
    [SerializeField] Text _txtMSec;
    [SerializeField] Text _txtKillCount;
    [SerializeField] Text _txtLimitKillCount;
    // Start is called before the first frame update
    public void OpenWindow(bool isWin, float time, int kill, int limitKill)
    {
        _txtResult.text = isWin ? "Win" : "Lose";
        int sec = (int)time;
        int msec = (int)((time - sec) * 100);
        _txtMSec.text = msec < 10 ? "0" : string.Empty;
        _txtSec.text = sec.ToString();
        _txtMSec.text += msec.ToString();

        _txtKillCount.text = isWin ? limitKill.ToString() : kill.ToString();
        _txtLimitKillCount.text = limitKill.ToString();
    }
    
    public void ClickRestartButton()
    {
        SceneManager.LoadScene("IngameScene");
    }
}
