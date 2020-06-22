using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Player : UnitBase
{
    [SerializeField] Transform _posFire;

    Animator _ctrlAni;
    CharacterController _controller;
    GameObject _modelObj;
    GameObject _prefabBullet;

    // UI Reference
    StickObject _stickLauncher;

    float _runSpeed = 5;
    float _walkSpeed = 1.0f;
    float _sideSpeed = 0.5f;
    bool _isAttack = false;

    void Awake()
    {
        _ctrlAni = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
        _modelObj = transform.GetChild(0).gameObject;
        _prefabBullet = Resources.Load("Prefabs/Objects/BulletObject") as GameObject;

        _ctrlAni.SetBool("IsBattle", true);
    }

    private void Start()
    {
        GameObject go = GameObject.FindGameObjectWithTag("LauncherStick");
        _stickLauncher = go.GetComponent<StickObject>();
        _stickLauncher.SetOwnerPlayer(this);
    }

    void Update()
    {
        if (_isDead)
            return;

        float speed = _runSpeed;
        float mz = Input.GetAxis("Horizontal");
        float mx = Input.GetAxis("Vertical");

        Vector3 mv = new Vector3(mx, 0, -mz);
        mv = (mv.magnitude > 1) ? mv.normalized : mv;

        if (_stickLauncher._isAimMotion)
        {
            speed = ChangeAnimationToDirection(mv);
        }
        else
        {
            if (mv.magnitude == 0)
                _ctrlAni.SetInteger("AniType", (int)eAniType.IDLE);
            else if (mv.magnitude > 0)
            {
                _ctrlAni.SetInteger("AniType", (int)eAniType.RUN);
                _modelObj.transform.rotation = Quaternion.LookRotation(mv);
            }
        }

        _controller.Move(mv * speed * Time.deltaTime);
    }

    float ChangeAnimationToDirection(Vector3 dir)
    {
        float speed = _walkSpeed;
        if (dir == Vector3.zero)
        {
            if (_ctrlAni.GetInteger("AniType") != (int)eAniType.ATTACK)
            {
                _ctrlAni.SetInteger("AniType", (int)eAniType.ATTACK);
                _ctrlAni.SetBool("StartAttack", true);
            }
            _modelObj.transform.rotation = Quaternion.LookRotation(_stickLauncher._direction);
        }
        else
        {
            InitializeDirection();
            if (dir.z == 0)
            {
                if (dir.x > 0)
                {
                    speed = _sideSpeed;
                    _ctrlAni.SetInteger("AniType", (int)eAniType.WALK_LEFT);
                }
                else if (dir.x < 0)
                {
                    speed = _sideSpeed;
                    _ctrlAni.SetInteger("AniType", (int)eAniType.WALK_RIGHT);
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
                _ctrlAni.SetInteger("AniType", (int)eAniType.RUN);
            }
            else
            {
                _ctrlAni.SetInteger("AniType", (int)eAniType.WALK_BACK);
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
        return speed;
    }

    public void InitializeDirection()
    {
        _modelObj.transform.rotation = Quaternion.identity;
    }

    public void Fire()
    {
        Instantiate(_prefabBullet, _posFire.position, _posFire.rotation);

        _ctrlAni.SetBool("StartAttack", false);
    }
}