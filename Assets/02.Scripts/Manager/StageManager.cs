using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlaw
{
    public class StageManager : MonoBehaviour
    {
        GameObject _startEffect;
        bool _firstCheck = false;

        private void Start()
        {
            _startEffect = GameObject.Find("PlayerStartEffect");
            _startEffect.SetActive(_firstCheck);
        }
        // Update is called once per frame
        void Update()
        {
            if (!_firstCheck)
            {
                if (IngameManager.Instance._nowGameState == EGameState.SpawnPlayer)
                {
                    _firstCheck = true;
                    _startEffect.SetActive(_firstCheck);
                }
            }
        }
    }
}