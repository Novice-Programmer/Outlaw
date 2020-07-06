using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Outlaw
{
    public class TimeWindow : MonoBehaviour
    {
        [SerializeField] Text _secText = null;
        [SerializeField] Text _msecText = null;

        public void TimeUpdate(float time)
        {
            int sec = (int)time;
            int msec = (int)((time - sec) * 100);
            _msecText.text = msec < 10 ? "0" : string.Empty;
            _secText.text = sec.ToString();
            _msecText.text += msec.ToString();
        }
    }
}