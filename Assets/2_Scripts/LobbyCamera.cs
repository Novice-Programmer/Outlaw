using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyCamera : MonoBehaviour
{
    [SerializeField] Vector3 _offSet = Vector3.zero;

    LobbyPlayer _playerObj;

    Transform _lookTr;

    Quaternion _startQ;

    private void Awake()
    {
        _startQ = transform.rotation;
    }

    // Start is called before the first frame update
    void Start()
    {
        GameObject go = GameObject.FindGameObjectWithTag("Player");
        _playerObj = go.GetComponent<LobbyPlayer>();
        _lookTr = _playerObj._lookTr;
    }

    // Update is called once per frame
    void Update()
    {
        if(_playerObj != null)
        {
            Vector3 rotateEular = _playerObj.transform.rotation.eulerAngles + _startQ.eulerAngles;

            transform.position = _playerObj.transform.position + _offSet;
            transform.LookAt(_lookTr);
           // transform.rotation = Quaternion.Euler(rotateEular);
        }
    }
}
