using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Loading : MonoBehaviour
{
    [Range(0.1f, 0.5f)] [SerializeField] float _dotTime = 0.3f;
    [SerializeField] Image _imgLoading = null;
    [SerializeField] Text _txtLoading = null;
    [SerializeField] Text _txtLoadingValue = null;
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

    public void OpenLoaddingWnd()
    {
        _rate = 0;
        _txtLoadingValue.text = "0%";
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
