using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingMonster : MonoBehaviour
{
    [SerializeField] float _rotateSpeed = 130.0f;
    Quaternion _que = Quaternion.Euler(0, 360, 0);
    void Update()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _que, Time.deltaTime * _rotateSpeed);
    }
}
