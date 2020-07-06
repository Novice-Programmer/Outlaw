using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlaw
{
    public class DamageField : MonoBehaviour
    {
        [SerializeField] bool _targetAll = false;
        [Range(1, 10)] [SerializeField] int _damagePower = 3;

        BoxCollider _range;

        private void Awake()
        {
            _range = GetComponent<BoxCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            UnitBase unit = null;
            if (other.CompareTag("Player"))
            {
                unit = other.GetComponent<Player>();
            }
            else if (other.CompareTag("Monster") && _targetAll)
            {
                unit = other.GetComponent<Monster>();
            }

            if (unit != null)
            {
                unit.OnHitting(_damagePower);
            }
        }
    }
}