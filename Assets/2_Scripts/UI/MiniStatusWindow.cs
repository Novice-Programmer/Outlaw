using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniStatusWindow : MonoBehaviour
{
    [SerializeField] Text _txtName = null;
    [SerializeField] Text _txtBulletCount = null;
    [SerializeField] Image _hpValue = null;
    [SerializeField] Image _shieldValue = null;
    [SerializeField] Slider _sdBullet = null;

    Player _player;
    int _limitCount;

    public void InitializeSetData(string name, int bulletCount)
    {
        _txtName.text = name;
        _txtBulletCount.text = bulletCount.ToString();

        _hpValue.fillAmount = 1.0f;
        _shieldValue.fillAmount = 1.0f;
        _sdBullet.maxValue = bulletCount;
    }

    public void SetHPRate(float rate) 
    {
        _hpValue.fillAmount = rate;
    }

    public void SetEnergeRate(float rate)
    {
        _shieldValue.fillAmount = rate;
    }

    public void SetBulletRate(int count)
    {
        _txtBulletCount.text = count.ToString();
        _sdBullet.value = count;
    }
}
