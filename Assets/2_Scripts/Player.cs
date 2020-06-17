using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : UnitBase
{
    Animator _animator;
    CharacterController _controller;
    GameObject _modelObj;
    [SerializeField] float _runSpeed = 5;
    [SerializeField] float _walkSpeed = 1.0f;
    [SerializeField] float _sideSpeed = 0.2f;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
        _modelObj = transform.GetChild(0).gameObject;
        _animator.SetBool("IsBattle", true);
    }


    void Update()
    {
        float speed = _runSpeed;
        float mz = Input.GetAxis("Horizontal");
        float mx = Input.GetAxis("Vertical");

        Vector3 mv = new Vector3(mx, 0, -mz);
        mv = (mv.magnitude > 1) ? mv.normalized : mv;

        if (mv.magnitude == 0)
            _animator.SetInteger("AniType", (int)eAniType.IDLE);
        else if (mv.magnitude > 0)
        {
            _animator.SetInteger("AniType", (int)eAniType.RUN);
            _modelObj.transform.rotation = Quaternion.LookRotation(mv);
        }

        // 방향을 받아서 방향에 따른 애니메이션 변화
        //float speed = ChangeAnimationToDirection(mv);

        _controller.Move(mv * speed * Time.deltaTime);
    }

    float ChangeAnimationToDirection(Vector3 dir)
    {
        float speed = _walkSpeed;
        eAniType aniType = eAniType.IDLE;
        if (dir == Vector3.zero)
            aniType = eAniType.IDLE;
        else
        {
            if (dir.z == 0)
            {
                if (dir.x > 0)
                {
                    speed = _sideSpeed;
                    aniType = eAniType.WALK_LEFT;
                }
                else if (dir.x < 0)
                {
                    speed = _sideSpeed;
                    aniType = eAniType.WALK_RIGHT;
                }
            }
            else if (dir.z > 0)
            {
                speed = _runSpeed;
                if (dir.x > 0)
                {
                    speed -= _sideSpeed;
                }
                else if (dir.x < 0)
                {
                    speed -= _sideSpeed;
                }
                aniType = eAniType.RUN;
            }
            else
            {
                aniType = eAniType.WALK_BACK;
                if (dir.x > 0)
                {
                    speed -= _sideSpeed;
                }
                else if (dir.x < 0)
                {
                    speed -= _sideSpeed;
                }
            }
        }
        _animator.SetInteger("AniType", (int)aniType);
        return speed;
    }
}