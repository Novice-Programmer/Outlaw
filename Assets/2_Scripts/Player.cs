using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Player : UnitBase
{
    [SerializeField] Transform _posFire = null;

    Animator _ctrlAni;
    CharacterController _controller;
    GameObject _modelObj;
    GameObject _prefabBullet;

    // UI Reference
    StickObject _stickLauncher;

    // Player 기본 정보
    float _runSpeed = 5;
    float _walkSpeed = 1.0f;
    float _sideSpeed = 0.5f;
    int _limitBulletCount = 24;

    // Player 활용 정보
    float _movSpeed;
    int _curBulletCount = 0;

    public int _finalDamage
    {
        get { return _baseAtt; }
    }

    public bool _playerDead
    {
        get { return _isDead; }
    }

    eAniType _nowAction;

    void Awake()
    {
        _ctrlAni = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
        _modelObj = transform.GetChild(0).gameObject;
        _prefabBullet = Resources.Load("Prefabs/Objects/BulletObject") as GameObject;

        _ctrlAni.SetBool("IsBattle", true);

        //임시
        InitalizeData("플레이어", 30, 3, 2);
        //
    }

    private void Start()
    {
        GameObject go = GameObject.FindGameObjectWithTag("LauncherStick");
        _stickLauncher = go.GetComponent<StickObject>();
        _stickLauncher.SetOwnerPlayer(this);
    }

    void Update()
    {
        if (_isDead || _nowAction == eAniType.RELOAD)
            return;

        float mz = Input.GetAxis("Horizontal");
        float mx = Input.GetAxis("Vertical");

        Vector3 mv = new Vector3(mx, 0, -mz);
        mv = (mv.magnitude > 1) ? mv.normalized : mv;

        if (_stickLauncher._isAimMotion)
        {
            ChangeAnimationToDirection(mv);
        }
        else
        {
            if (mv.magnitude == 0)
                ChangeAction(eAniType.IDLE);
            else if (mv.magnitude > 0)
            {
                ChangeAction(eAniType.RUN);
                _modelObj.transform.rotation = Quaternion.LookRotation(mv);
            }
        }

        _controller.Move(mv * _movSpeed * Time.deltaTime);
    }

    void ChangeAnimationToDirection(Vector3 dir)
    {
        if (dir == Vector3.zero)
        {
            if (_ctrlAni.GetInteger("AniType") != (int)eAniType.ATTACK)
            {
                ChangeAction(eAniType.ATTACK);
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
                    ChangeAction(eAniType.WALK_LEFT);
                    _movSpeed = _sideSpeed;
                }
                else if (dir.x < 0)
                {
                    ChangeAction(eAniType.WALK_RIGHT);
                    _movSpeed = _sideSpeed;
                }
            }
            else if (dir.z > 0)
            {
                ChangeAction(eAniType.RUN);
                if (dir.x != 0)
                    _movSpeed -= _sideSpeed;
            }
            else
            {
                ChangeAction(eAniType.WALK_BACK);
                if (dir.x != 0)
                    _movSpeed -= _sideSpeed;
            }
        }
    }

    public void ChangeAction(eAniType type)
    {
        switch (type)
        {
            case eAniType.IDLE:
            case eAniType.ATTACK:
                _ctrlAni.SetInteger("AniType", (int)type);
                break;
            case eAniType.RUN:
                _movSpeed = _runSpeed;
                _ctrlAni.SetInteger("AniType", (int)type);
                break;
            case eAniType.WALK:
            case eAniType.WALK_LEFT:
            case eAniType.WALK_RIGHT:
            case eAniType.WALK_BACK:
                _movSpeed = _walkSpeed;
                _ctrlAni.SetInteger("AniType", (int)type);
                break;
            case eAniType.RELOAD:
                _curBulletCount = 0;
                _ctrlAni.SetTrigger("Reload");
                break;
            case eAniType.DEAD:
                _isDead = true;
                _ctrlAni.SetTrigger("Dead");
                _controller.enabled = false;
                _stickLauncher.enabled = false;
                break;
        }
        _nowAction = type;
    }

    public void InitializeDirection()
    {
        _modelObj.transform.rotation = Quaternion.identity;
    }

    public void Fire()
    {

        GameObject go = Instantiate(_prefabBullet, _posFire.position, _posFire.rotation);
        Bullet bullet = go.GetComponent<Bullet>();
        bullet.InitSetting(this);
        _curBulletCount++;

        _ctrlAni.SetBool("StartAttack", false);
        if (_curBulletCount >= _limitBulletCount)
            ChangeAction(eAniType.RELOAD);
    }

    public void EndReload()
    {
    
        ChangeAction(eAniType.IDLE);
    }

    public bool OnHitting(int hitDamage)
    {
        if (HittingMe(hitDamage))
        {
            ChangeAction(eAniType.DEAD);
        }
        else
        {

        }
        return _isDead;
    }
}