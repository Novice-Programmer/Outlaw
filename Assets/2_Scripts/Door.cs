using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Transform _openPos = null;
    [SerializeField] float _openSpeed = 1.5f;

    Vector3 _doorPos = Vector3.zero;

    bool _isOpenDoor = false;

    private void Awake()
    {
        _doorPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isOpenDoor)
        {
            transform.position = Vector3.MoveTowards(transform.position, _openPos.position, Time.deltaTime * _openSpeed);
            if (Vector3.Distance(transform.position, _openPos.position) < 0.05f)
            {
                _isOpenDoor = false;
            }
        }
    }

    public void OpenDoor()
    {
        _isOpenDoor = true;
    }

    public void CloseDoor()
    {

    }
}
