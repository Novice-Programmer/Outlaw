using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;

public class BrokenObject : MonoBehaviour
{
    [SerializeField] int _duration = 999;
    [SerializeField] string _hitEffectName = "Hit";
    [SerializeField] string _boomEffectName = "Explosion";
    [SerializeField] string _frameEffectName = "Frame";
    [SerializeField] int _effSize = 1;
    GameObject _effectHit;
    GameObject _effectExplosion;
    GameObject _effectFrame;

    const string effPass = "Prefabs/ParticleEffects/";

    // Start is called before the first frame update
    void Awake()
    {
        string hitPass = effPass + _hitEffectName;
        string boomPass = effPass + _boomEffectName;
        string framePass = effPass + _frameEffectName;

        _effectHit = Resources.Load(hitPass) as GameObject;
        _effectExplosion = Resources.Load(boomPass) as GameObject;
        _effectFrame = Resources.Load(framePass) as GameObject;
    }

    // Update is called once per frame
    void Update()
    {
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
                    go = Instantiate(_effectExplosion, transform.position, Quaternion.identity);
                    Destroy(go, 2.0f);
                    go = Instantiate(_effectFrame, transform.position, _effectFrame.transform.rotation);
                    go.transform.localScale = Vector3.one * 3;
                    Destroy(go, 5.0f);
                    Destroy(gameObject);
                }
            }
        }
    }
}
