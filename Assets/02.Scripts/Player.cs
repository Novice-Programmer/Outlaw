using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Outlaw
{
    public class Player : UnitBase
    {
        [SerializeField] Transform _posFire = null;
        [SerializeField] Transform _posLook = null;
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

        public Transform _tfLookPos
        {
            get { return _posLook; }
        }

        EAniType _nowAction;

        void Awake()
        {
            _ctrlAni = GetComponent<Animator>();
            _controller = GetComponent<CharacterController>();
            _modelObj = transform.GetChild(0).gameObject;
            _prefabBullet = Resources.Load("Prefabs/Objects/BulletObject") as GameObject;

            _ctrlAni.SetBool("IsBattle", true);

            UserInfo playerInfo = DataManager.Instance._userData;

            InitalizeData(playerInfo._name, playerInfo._playerAvatar._hp, playerInfo._playerAvatar._att, playerInfo._playerAvatar._def);
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
            if (IngameManager.Instance._nowGameState != EGameState.Play)
            {
                ChangeAction(EAniType.IDLE);
                return;
            }

            if (_isDead || _nowAction == EAniType.RELOAD)
                return;

            Vector3 mv;
            if (ViewPoint.Instance._viewPoint== EViewPoint.FirstPerson)
            {
                float mx, mz;
#if UNITY_EDITOR
                mx = Input.GetAxis("Horizontal");
                mz = Input.GetAxis("Vertical");
                if (mx == 0 && mz == 0)
                {
                    mx = -_stickMovement._dirMoveFirst.x;
                    mz = -_stickMovement._dirMoveFirst.y;
                    mv = transform.forward * mz;
                }
                else
                    mv = transform.forward * mz;
#else
                mx = -_stickMovement._dirMoveFirst.x;
                mz = -_stickMovement._dirMoveFirst.y;
                mv = transform.forward * mz;
#endif
                if (_stickLauncher._isAimMotion)
                {
                    Vector3 md = new Vector3(mx, 0, mz);
                    md = (md.magnitude > 1) ? md.normalized : md;
                    ChangeAnimationToDirectionFirstView(md);

                    md = transform.TransformDirection(md);
                    mv = md * _movSpeed * Time.deltaTime;
                }
                else
                {
                    if (mz > 0)
                        ChangeAction(EAniType.RUN);
                    else if (mz < 0)
                        ChangeAction(EAniType.WALK_BACK);
                    else
                    {
                        if (mx != 0)
                            ChangeAction(EAniType.WALK_BACK);
                        else
                            ChangeAction(EAniType.IDLE);
                    }

                    transform.Rotate(Vector3.up * mx * Time.deltaTime * 100);
                    mv = mv * _movSpeed * _nowSpeed * Time.deltaTime;
                }
            }
            else
            {
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
                        ChangeAction(EAniType.IDLE);
                    else if (mv.magnitude > 0)
                    {
                        ChangeAction(EAniType.RUN);
                        _modelObj.transform.rotation = Quaternion.LookRotation(mv);
                    }
                }
                mv = mv * _movSpeed * _nowSpeed * Time.deltaTime;
            }
            mv = new Vector3(mv.x, mv.y + Physics.gravity.y * 0.1f, mv.z);
            _controller.Move(mv);
        }

        void ChangeAnimationToDirectionFirstView(Vector3 dir)
        {
            if (dir.magnitude == 0)
            {
                if (_nowAction != EAniType.ATTACK)
                {
                    ChangeAction(EAniType.ATTACK);
                    _ctrlAni.SetBool("StartAttack", true);
                }
                if (_stickLauncher._directionFirst.y >= 0.4f || _stickLauncher._directionFirst.y <= -0.4f)
                    transform.Rotate(_stickLauncher._directionFirst * Time.deltaTime * 30);
            }
            else
            {
                if (dir.z == 0)
                {
                    if (dir.x > 0)
                        ChangeAction(EAniType.WALK_LEFT);
                    else if (dir.x < 0)
                        ChangeAction(EAniType.WALK_RIGHT);
                }
                else if (dir.z > 0)

                    ChangeAction(EAniType.RUN);

                else
                    ChangeAction(EAniType.WALK_BACK);
            }
        }

        void ChangeAnimationToDirection(Vector3 dir)
        {
            if (dir == Vector3.zero)
            {
                if (_ctrlAni.GetInteger("AniType") != (int)EAniType.ATTACK)
                {
                    ChangeAction(EAniType.ATTACK);
                    _ctrlAni.SetBool("StartAttack", true);
                }
                if (ViewPoint.Instance._viewPoint == EViewPoint.FirstPerson)
                    transform.rotation = Quaternion.LookRotation(_stickLauncher._directionFirst);
                else
                    _modelObj.transform.rotation = Quaternion.LookRotation(_stickLauncher._direction);
            }
            else
            {
                InitializeDirection();
                if (dir.z == 0)
                {
                    if (dir.x > 0)
                    {
                        ChangeAction(EAniType.WALK_LEFT);
                        _movSpeed = _sideSpeed;
                    }
                    else if (dir.x < 0)
                    {
                        ChangeAction(EAniType.WALK_RIGHT);
                        _movSpeed = _sideSpeed;
                    }
                }
                else if (dir.z > 0)
                {
                    ChangeAction(EAniType.RUN);
                    if (dir.x != 0)
                        _movSpeed -= _sideSpeed;
                }
                else
                {
                    ChangeAction(EAniType.WALK_BACK);
                    if (dir.x != 0)
                        _movSpeed -= _sideSpeed;
                }
            }
        }

        public void ChangeAction(EAniType type)
        {
            switch (type)
            {
                case EAniType.IDLE:
                case EAniType.ATTACK:
                    _ctrlAni.SetInteger("AniType", (int)type);
                    break;
                case EAniType.RUN:
                    _movSpeed = _runSpeed;
                    _ctrlAni.SetInteger("AniType", (int)type);
                    break;
                case EAniType.WALK:
                case EAniType.WALK_LEFT:
                case EAniType.WALK_RIGHT:
                case EAniType.WALK_BACK:
                    _movSpeed = _walkSpeed;
                    _ctrlAni.SetInteger("AniType", (int)type);
                    break;
                case EAniType.RELOAD:
                    _curBulletCount = 0;
                    _ctrlAni.SetTrigger("Reload");
                    break;
                case EAniType.DEAD:
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
            if (ViewPoint.Instance._viewPoint == EViewPoint.ThirdPerson)
                _modelObj.transform.rotation = Quaternion.identity;
        }

        public void Fire()
        {
            if (_nowAction == EAniType.RELOAD)
                return;
            GameObject go = Instantiate(_prefabBullet, _posFire.position, _posFire.rotation);
            Bullet bullet = go.GetComponent<Bullet>();
            bullet.InitData(this);

            _curBulletCount++;
            _miniWnd.SetBulletRate(_limitBulletCount - _curBulletCount);
            _ctrlAni.SetBool("StartAttack", false);
            if (_curBulletCount >= _limitBulletCount)
                ChangeAction(EAniType.RELOAD);
        }

        public void EndReload()
        {
            _curBulletCount = 0;
            ChangeAction(EAniType.IDLE);
            _miniWnd.SetBulletRate(_limitBulletCount - _curBulletCount);
        }

        public override bool OnHitting(int hitDamage)
        {
            if (HittingMe(hitDamage))
            {
                ChangeAction(EAniType.DEAD);
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
}