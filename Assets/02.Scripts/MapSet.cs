using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlaw
{
    public class MapSet : MonoBehaviour
    {
        [SerializeField] string _mapName = "Desert";
        [SerializeField] string _mapExplan = "사막";

        public string _nameMap
        {
            get { return _mapName; }
        }

        public string _explanMap
        {
            get { return _mapExplan; }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                IngameManager.Instance.MapChange(this);
            }
        }
    }
}