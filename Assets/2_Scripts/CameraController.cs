using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Vector3 _offSet = Vector3.zero;
    [SerializeField] float _followSpeed = 2.5f;
    GameObject _playerObj;

    void Update()
    {
        if (_playerObj != null)
        {
            Vector3 target = _playerObj.transform.position + _offSet;
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * _followSpeed);
            // 즉각반응
            //transform.position = _playerObj.transform.position + _offSet;
        }
    }

    public void SetPlayer(GameObject p)
    {
        _playerObj = p;
    }
}
