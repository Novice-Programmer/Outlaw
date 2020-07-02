using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    [SerializeField] Vector3 _offSet = Vector3.zero;
    [SerializeField] Vector3 _otherViewRotate = Vector3.zero;
    [SerializeField] Vector3 _firstViewOffSet = Vector3.zero;
    [SerializeField] Vector3 _firstViewRotate = Vector3.zero;
    [SerializeField] float _followSpeed = 2.5f;
    GameObject _playerObj;
    // Start is called before the first frame update
    void Start()
    {
        // 임시
        _playerObj = GameObject.FindGameObjectWithTag("Player");
        //
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            int lMask = 1 << LayerMask.NameToLayer("FIELD");
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, lMask))
            {
            }
        }
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
