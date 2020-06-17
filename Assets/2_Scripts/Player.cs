using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : UnitBase
{

    CharacterController _controller;
    [SerializeField] float _moveSpeed = 15;

    void Start()
    {
        _controller = GetComponent<CharacterController>();
    }


    void Update()
    {
        float z = Input.GetAxis("Horizontal");
        float x = Input.GetAxis("Vertical");

        transform.Translate((Vector3.forward * x + Vector3.right * z) * Time.deltaTime * _moveSpeed);
    }
}
