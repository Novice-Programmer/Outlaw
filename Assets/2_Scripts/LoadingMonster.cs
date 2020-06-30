using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingMonster : MonoBehaviour
{
    [SerializeField] float _rotateSpeed = 130.0f;
    Quaternion _rotateBack = Quaternion.Euler(Vector3.zero);
    Quaternion _rotateFront = Quaternion.Euler(0, 180, 0);
    Quaternion _rotateStart;
    bool _isBack = false;

    private void Awake()
    {
        _rotateStart = transform.rotation;
    }

    void Update()
    {
        if (!_isBack)
        {
            if (transform.rotation.eulerAngles.y > 0.0f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, _rotateBack, Time.deltaTime * _rotateSpeed);
            }
            else
            {
                _isBack = true;
            }
        }
        else
        {
            if(transform.rotation.eulerAngles.y > -180.0f)
            {
                transform.rotation = Quaternion.RotateTowards(transform.rotation, _rotateFront, Time.deltaTime * _rotateSpeed);
            }
            if(transform.rotation.eulerAngles.y == 180)
            {
                _isBack = false;
                transform.rotation = _rotateStart;
            }
        }
    }
}
