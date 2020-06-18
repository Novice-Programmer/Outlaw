using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float _force = 800.0f;

    Rigidbody _rgbd;

    private void Awake()
    {
        _rgbd = GetComponent<Rigidbody>();
        _rgbd.AddForce(transform.forward * _force);
        Destroy(gameObject, 3.0f);
    }
    void Start()
    {
    }

    void Update()
    {
        
    }
}
