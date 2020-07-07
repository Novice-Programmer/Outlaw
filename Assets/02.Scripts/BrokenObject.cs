using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;

namespace Outlaw
{
    public class BrokenObject : MonoBehaviour
    {
        [SerializeField] int _duration = 999;
        [SerializeField] string _hitEffectName = "Hit";
        [SerializeField] string _boomEffectName = "Explosion";
        [SerializeField] string _frameEffectName = "Frame";
        [SerializeField] float _effSize = 1;
        [SerializeField] float _markerSize = 1;

        int _brokenScore = 0;
        bool _isGoal = false;

        GameObject _effectHit;
        GameObject _effectExplosion;
        GameObject _effectFrame;

        const string effPass = "Prefabs/ParticleEffects/";

        public float _sizeMarker
        {
            get { return _markerSize; }
        }

        public int _durability
        {
            get { return _duration; }
        }

        // Start is called before the first frame update
        void Awake()
        {
            string hitPass = effPass + _hitEffectName;
            string boomPass = effPass + _boomEffectName;
            string framePass = effPass + _frameEffectName;

            _effectHit = Resources.Load(hitPass) as GameObject;
            _effectExplosion = Resources.Load(boomPass) as GameObject;
            _effectFrame = Resources.Load(framePass) as GameObject;

            _brokenScore = _duration / 2;
        }

        public void InitGoal()
        {
            _isGoal = true;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("BulletObj"))
            {
                GameObject go = Instantiate(_effectHit, other.transform.position, Quaternion.identity);
                Destroy(go, 2.0f);
                Destroy(other.gameObject);
                if (_duration != 999)
                {
                    _duration--;
                    if (_duration <= 0)
                    {
                        IngameManager.Instance._scoreBroken = _brokenScore;
                        go = Instantiate(_effectExplosion, transform.position, Quaternion.identity);
                        Vector3 scale = go.transform.localScale;
                        go.transform.localScale = scale * _effSize;
                        Destroy(go, 2.0f);
                        go = Instantiate(_effectFrame, transform.position, Quaternion.identity);
                        scale = go.transform.localScale;
                        go.transform.localScale = scale * _effSize;
                        Destroy(go, 5.0f);
                        if (_isGoal)
                            IngameManager.Instance.GameEnd(true);
                        Destroy(gameObject);
                    }
                }
            }
        }
    }
}