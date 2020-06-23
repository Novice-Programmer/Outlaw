using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitZone : MonoBehaviour
{
    bool _isMonster;
    UnitBase _ownerCharacter;

    public void InitSettings(UnitBase owner, bool isMon = true)
    {
        _ownerCharacter = owner;
        _isMonster = isMon;
    }

    public void EnableTrigger(bool isOn)
    {
        GetComponent<BoxCollider>().enabled = isOn;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // 오너 클래스에게 히트한걸 알려줌.
        }
        
    }
}
