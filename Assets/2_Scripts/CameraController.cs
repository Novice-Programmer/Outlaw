using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Vector3 _offSet = Vector3.zero;
    [SerializeField] Vector3 _firstViewOffSet = Vector3.zero;
    [SerializeField] float _followSpeed = 10.0f;
    [SerializeField] float _rotSpeed = 5;
    GameObject _playerObj;
    Player _cPlayer;

    Vector3 _goalPosition;

    void Update()
    {
        if (IngameManager.Instance._firstView)
        {
            float currentYAngle = Mathf.LerpAngle(transform.eulerAngles.y, _playerObj.transform.eulerAngles.y, _rotSpeed * Time.deltaTime);
            Quaternion rot = Quaternion.Euler(0, currentYAngle, 0);
            _goalPosition = _playerObj.transform.position - (rot * Vector3.forward * _firstViewOffSet.z) + (Vector3.up * _firstViewOffSet.y);
            transform.position = Vector3.MoveTowards(transform.position, _goalPosition,_followSpeed * Time.deltaTime);
            transform.LookAt(_cPlayer._tfLookPos);
        }
        else
        {
            if (_playerObj != null)
            {
                Vector3 target = _playerObj.transform.position + _offSet;
                transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * _followSpeed);
                // 즉각반응
                //transform.position = _playerObj.transform.position + _offSet;
            }
        }
    }

    public void SetPlayer(GameObject p)
    {
        _playerObj = p;
        _cPlayer = p.GetComponent<Player>();
    }
}
