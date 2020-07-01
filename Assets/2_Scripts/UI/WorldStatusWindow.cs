using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldStatusWindow : MonoBehaviour
{
    [SerializeField] Slider _hpBar = null;
    [SerializeField] float _limitViewTime = 3.0f;
    float _timeCheck=int.MaxValue;

    private void Start()
    {
        _timeCheck = _limitViewTime;
    }

    private void LateUpdate()
    {
        if (gameObject.activeSelf)
        {
            _timeCheck += Time.deltaTime;
            if(_timeCheck >= _limitViewTime)
            {
                gameObject.SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {
        transform.LookAt(Camera.main.transform);
    }

    public void SetHpRate(float rate)
    {
        gameObject.SetActive(true);
        _hpBar.value = rate;
        _timeCheck = 0;
    }
}
