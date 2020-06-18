using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenObject : MonoBehaviour
{
    [SerializeField] int _duration = 999;
    GameObject _effectHit;
    GameObject _effectExplosion;
    GameObject _effectFrame;

    // Start is called before the first frame update
    void Awake()
    {
        _effectHit = Resources.Load("Prefabs/ParticleEffects/Hit") as GameObject;
        _effectExplosion = Resources.Load("Prefabs/ParticleEffects/Explosion") as GameObject;
        _effectFrame = Resources.Load("Prefabs/ParticleEffects/Frame") as GameObject;
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
