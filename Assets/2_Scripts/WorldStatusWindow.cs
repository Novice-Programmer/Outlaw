using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WorldStatusWindow : MonoBehaviour
{
    [SerializeField] Slider _sdMonSlider;
    [SerializeField] float _viewTime = 3.0f;
    float time=0;
    bool _isAttack=false;

    private void Start()
    {
        time = _viewTime;
        _sdMonSlider.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_isAttack)
        {
            time -= Time.deltaTime;
            if (time <= 0)
            {
                _isAttack = false;
                _sdMonSlider.gameObject.SetActive(false);
            }
        }
    }

    private void FixedUpdate()
    {
        transform.LookAt(Camera.main.transform);
    }

    public void HPRateUpdate(float rate)
    {
        _sdMonSlider.value = rate;
        _isAttack = true;
        time = _viewTime;
        _sdMonSlider.gameObject.SetActive(true);
    }
}
