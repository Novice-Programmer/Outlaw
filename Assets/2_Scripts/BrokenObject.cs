using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenObject : MonoBehaviour
{
    [SerializeField] float _duration = 999;

    float _time = 0;
    float _effTime = 0;

    bool _broken = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _time += Time.deltaTime;
        _effTime += Time.deltaTime;
        if (!_broken)
        {
            if (_time >= _duration && _duration != 999)
            {
                GameObject boomEffect = Resources.Load("Prefabs/ParticleEffects/Explosion") as GameObject;
                GameObject go = Instantiate(boomEffect, transform);
                Destroy(go, 2.0f);
                _broken = true;
            }
            else
            {
                if (_effTime >= 0.5f)
                {
                    _effTime = 0;
                    GameObject spakeEffect = Resources.Load("Prefabs/ParticleEffects/Spake") as GameObject;
                    GameObject go = Instantiate(spakeEffect, transform);
                    Destroy(go, 1.0f);
                }
            }
        }
    }
}
