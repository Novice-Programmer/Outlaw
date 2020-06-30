using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class Player : UnitBase
{
    [SerializeField] Transform _posFire = null;
    [SerializeField] Marker _marker = null;

    Animator _ctrlAni;
    CharacterController _controller;
    GameObject _modelObj;
    GameObject _prefabBullet;

    // UI Reference
    StickObject _stickLauncher;
    StickObject _stickMovement;
    MiniStatusWindow _miniWnd;

    // Player 기본 정보
    float _runSpeed = 5;
    float _walkSpeed = 1.0f;
    float _sideSpeed = 0.5f;
    int _limitBulletCount = 24;

    // Player 활용 정보
    float _movSpeed;
    float _nowSpeed = 1.0f;
    float _speedPower = 1.0f;
    int _curBulletCount = 0;

    public int _finalDamage
    {
        get { return _baseAtt; }
    }

    public int _maxBulletCount
    {
        get { return _limitBulletCount; }
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
        GameObject go = GameObject.FindGameObjectWithTag("MiniAvatarWindow");
        _miniWnd = go.GetComponent<MiniStatusWindow>();
        _miniWnd.InitializeSetData(_myName, _limitBulletCount);
        MinimapController.Instance.AddMarker(_marker);
    }

    void Update()
    {
        if(IngameManager.Instance._nowGameState != IngameManager.EGameState.Play)
        {
            ChangeAction(eAniType.IDLE);
            return;
        }

        if (_isDead || _nowAction == eAniType.RELOAD)
            return;
        Vector3 mv;
#if UNITY_EDITOR
        float mz = -Input.GetAxis("Horizontal");
        float mx = Input.GetAxis("Vertical");
        if (mz == 0 && mx == 0)
            mv = _stickMovement._dirMov;
        else
            mv = new Vector3(mx, 0, mz);
#else
        mv = _stickMovement._dirMov;
#endif

        if (_stickLauncher._isAimMotion)
        {
            ChangeAnimationToDirection(mv);
        }
        else
        {
            mv = (mv.magnitude > 1) ? mv.normalized : mv;
            if (mv.magnitude == 0)
                ChangeAction(eAniType.IDLE);
            else if (mv.magnitude > 0)
            {
                ChangeAction(eAniType.RUN);
                _modelObj.transform.rotation = Quaternion.LookRotation(mv);
            }
        }

        _controller.Move(mv * _movSpeed * _nowSpeed * Time.deltaTime);
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
                PlayerDead();
                _ctrlAni.SetTrigger("Dead");
                break;
        }
        _nowAction = type;
    }

    void PlayerDead()
    {
        _isDead = true;
        _controller.enabled = false;
        _stickLauncher.gameObject.SetActive(false);
        _stickMovement.gameObject.SetActive(false);
    }

    public void InitializeDirection()
    {
        _modelObj.transform.rotation = Quaternion.identity;
    }

    public void Fire()
    {
        if (_nowAction == eAniType.RELOAD)
            return;
        GameObject go = Instantiate(_prefabBullet, _posFire.position, _posFire.rotation);
        Bullet bullet = go.GetComponent<Bullet>();
        bullet.InitData(this);

        _curBulletCount++;
        _miniWnd.SetBulletRate(_limitBulletCount - _curBulletCount);
        _ctrlAni.SetBool("StartAttack", false);
        if (_curBulletCount >= _limitBulletCount)
            ChangeAction(eAniType.RELOAD);
    }

    public void EndReload()
    {
        _curBulletCount = 0;
        ChangeAction(eAniType.IDLE);
        _miniWnd.SetBulletRate(_limitBulletCount - _curBulletCount);
    }

    public override bool OnHitting(int hitDamage)
    {
        if (HittingMe(hitDamage))
        {
            ChangeAction(eAniType.DEAD);
        }
        else
        {

        }
        _miniWnd.SetHPRate(_hpRate);
        return _isDead;
    }

    public void SpeedCheck(float speed = 1.0f)
    {
        if (speed == 1.0f)
            _nowSpeed = _speedPower;
        _nowSpeed = speed;
    }

    public void SettingSticks()
    {
        GameObject go = GameObject.FindGameObjectWithTag("LauncherStick");
        _stickLauncher = go.GetComponent<StickObject>();
        go = GameObject.FindGameObjectWithTag("MoveStick");
        _stickMovement = go.GetComponent<StickObject>();
        _stickLauncher.SetOwnerPlayer(this);
        _stickMovement.SetOwnerPlayer(this);
    }
}