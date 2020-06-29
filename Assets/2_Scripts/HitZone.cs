using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitZone : MonoBehaviour
{
    bool _isMonster;
    [SerializeField] string _hitEffName = "PowHit";
    UnitBase _ownerCharacter;
    Player _oldPlayer;

    GameObject _hitEffect;

    private void Awake()
    {
        _hitEffect = Resources.Load("Prefabs/ParticleEffects/" + _hitEffName) as GameObject;
    }

    public void InitSettings(UnitBase owner, bool isMon = true)
    {
        _ownerCharacter = owner;
        _isMonster = isMon;
    }

    public void EnableTrigger(bool isOn)
    {
        GetComponent<BoxCollider>().enabled = isOn;
        if (!isOn)
            _oldPlayer = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player p = other.GetComponent<Player>();
            if (_oldPlayer == p)
                return;
            _oldPlayer = p;
            GameObject hit = Instantiate(_hitEffect, transform.position, Quaternion.identity);
            Destroy(hit, 1.0f);
            // isMon에 따른 함수 호출
            if (_isMonster)
            {
                if (p.OnHitting(((Monster)_ownerCharacter)._finalDamage))
                    IngameManager.Instance.ReceivePlayerDie();
            }
            else
            {
                if (p.OnHitting(((Player)_ownerCharacter)._finalDamage))
                {

                }
            }
        }
    }
}
