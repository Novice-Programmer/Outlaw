using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float _force = 800.0f;
    int _bulletDamage = 3;
    bool _isPlayer = true;
    UnitBase _ownerCharacter;

    Rigidbody _rgbd;

    public int _finalDamage
    {
        get 
        {
            int baseDamage = _bulletDamage;
            if (_isPlayer)
            {
                baseDamage += ((Player)_ownerCharacter)._finalDamage;
            }
            else
            {
                baseDamage += ((Monster)_ownerCharacter)._finalDamage;
            }
            return baseDamage;
        }
    }

    public UnitBase _owner
    {
        get { return (UnitBase)_ownerCharacter; }
    }

    public void InitSetting(UnitBase owner,bool isPlayer = true)
    {
        _ownerCharacter = owner;
        _isPlayer = isPlayer;
    }

    private void Awake()
    {
        _rgbd = GetComponent<Rigidbody>();
        _rgbd.AddForce(transform.forward * _force);
        Destroy(gameObject, 3.0f);
    }
}
