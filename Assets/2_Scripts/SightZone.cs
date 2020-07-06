using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlaw
{
    public class SightZone : MonoBehaviour
    {
        Monster _owner;

        public void InitSettings(Monster owner)
        {
            _owner = owner;
            SphereCollider sc = GetComponent<SphereCollider>();
            sc.radius = _owner._lengthSight;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            { // 플레이어가 시야 범위에 들어왔는가
                Player p = other.GetComponent<Player>();
                _owner.OnBattle(p);
            }
        }
    }
}