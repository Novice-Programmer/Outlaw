using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlaw
{
    public class Environment : MonoBehaviour
    {
        [SerializeField] bool _isSlow = false;
        [Range(0.1f, 0.9f)] [SerializeField] float _slowPower = 0.3f;
        [SerializeField] bool _isDamage = false;
        [Range(1, 5)] [SerializeField] int _damagePower = 1;
        [Range(0.5f, 10.0f)] [SerializeField] float _damageTime = 1.5f;
        Player player;

        float _time = 0.0f;

        private void LateUpdate()
        {
            if (_isDamage)
            {
                if (player != null)
                {
                    _time += Time.deltaTime;
                    if (_time >= _damageTime)
                    {
                        _time = 0.0f;
                        player.OnHitting(_damagePower);
                    }
                }
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                player = other.GetComponent<Player>();
                if (_isSlow)
                    player.SpeedCheck(_slowPower);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                player = other.GetComponent<Player>();
                if (_isSlow)
                    player.SpeedCheck();
                _time = 0;
                player = null;
            }

        }
    }
}