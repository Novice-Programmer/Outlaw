using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.AI;

namespace Outlaw
{
    public class Monster : UnitBase
    {
        [SerializeField] HitZone _hitZone = null;
        [SerializeField] SightZone _sightZone = null;
        [SerializeField] WorldStatusWindow _worldMiniUI = null;
        [SerializeField] Marker _marker = null;
        [SerializeField] string _hitEffName = "MonsterHit";
        [SerializeField] string _deadEffName = "MonsterDead";
        Player _targetPlayer;
        SpawnControl _ownerParent;

        EKindRoam _roamingKind = EKindRoam.Random;
        ETypeRoam _roamingType = ETypeRoam.Random;
        EAniType _nowAction;
        Animation _ctrlAni;
        NavMeshAgent _navAgent;
        Dictionary<EAniKeyType, string> _aniList = new Dictionary<EAniKeyType, string>();
        List<Vector3> _roamPointList = new List<Vector3>();

        float _timeCheck = 0;

        // Monster의 기본 정보
        float _minWaitTime = 2.5f;
        float _maxWaitTime = 9.9f;
        float _runSpeed = 4;
        float _walkSpeed = 0.7f;
        float _sightRange = 10;
        float _attackRange = 3;
        float _followDistance = 20;
        float _rateMoveAct = 50.0f;

        // Monster의 활용 정보
        int _nowIndex = -1;
        int _moveCount = 0;
        int _randomPos = 0;
        int _monsterNum = 0;
        bool _isBack = false;
        bool _isSelectAct = true;  // false일때 선택을 한다. (true면 선택한 상황)
        bool _isRandom = false;
        bool _isInvincibility = false;
        Vector3 _posBattleStart;

        GameObject _hitEffect;
        GameObject _deadEffect;

        public int _number
        {
            set { _monsterNum = value; }
        }

        public float _lengthSight
        {
            get { return _sightRange; }
        }

        public int _finalDamage
        {
            get { return _baseAtt; }
        }

        public Player _target
        {
            get { return _targetPlayer; }
        }

        private void Awake()
        {
            _ctrlAni = GetComponent<Animation>();
            _navAgent = GetComponent<NavMeshAgent>();
            MyAnimationList();
            InitMonsterData();
        }
        void Start()
        {
            _ctrlAni.Play(_aniList[EAniKeyType.IDLE]);
            _sightZone.InitSettings(this);
            _hitZone.InitSettings(this);
            _hitZone.EnableTrigger(false);
            if (_roamingKind == EKindRoam.Random)
                _randomPos = Random.Range(0, _roamPointList.Count);
            SettingGoalPosition(GetNextPosition());
            _hitEffect = Resources.Load("Prefabs/ParticleEffects/" + _hitEffName) as GameObject;
            _deadEffect = Resources.Load("Prefabs/ParticleEffects/" + _deadEffName) as GameObject;
            MinimapController.Instance.AddMarker(_marker);
        }

        // Update is called once per frame
        void Update()
        {
            if (_isDead)
                return;

            if (!_ctrlAni.isPlaying)
                ChangeAction(EAniType.IDLE);

            switch (_nowAction)
            {
                case EAniType.WALK:
                    if (Vector3.Distance(transform.position, _navAgent.destination) < 0.5f)
                    {
                        _isSelectAct = false;
                    }
                    break;
                case EAniType.IDLE:
                    _timeCheck -= Time.deltaTime;
                    if (_timeCheck <= 0)
                    {
                        _isSelectAct = false;
                    }
                    break;
                case EAniType.RUN:
                    if (Vector3.Distance(transform.position, _posBattleStart) >= _followDistance)
                    {
                        ChangeAction(EAniType.BACKHOME);
                        _navAgent.destination = _posBattleStart;
                    }

                    else if (Vector3.Distance(transform.position, _navAgent.destination) < _attackRange)
                        ChangeAction(EAniType.ATTACK);
                    else
                        _navAgent.destination = _targetPlayer.transform.position;
                    break;
                case EAniType.ATTACK:
                    if (_targetPlayer._isDead)
                    {
                        Winner();
                    }
                    else if (Vector3.Distance(transform.position, _targetPlayer.transform.position) > _attackRange)
                    {
                        ChangeAction(EAniType.RUN);
                        _navAgent.destination = _targetPlayer.transform.position;
                    }
                    else
                    {
                        AnimationState anistate = _ctrlAni[_aniList[EAniKeyType.ATTACK]];
                        if (anistate.normalizedTime * 100 % 100 > 33)
                        {
                            _hitZone.EnableTrigger(true);
                        }
                        if (anistate.normalizedTime * 100 % 100 > 60)
                        {
                            _hitZone.EnableTrigger(false);
                        }
                    }
                    break;
                case EAniType.BACKHOME:
                    if (Vector3.Distance(transform.position, _posBattleStart) < 0.5f)
                    {
                        transform.position = _posBattleStart;
                        _isSelectAct = false;
                        _isInvincibility = false;
                        _targetPlayer = null;
                    }
                    else
                    {
                        _timeCheck += Time.deltaTime;
                        if (_timeCheck < 0.5f)
                        {
                            LifeSet(1);
                        }
                    }
                    break;
            }

            SelectAIProcess();
        }

        public void SettingGoalPosition(Vector3 point, bool isRun = false)
        {
            if (isRun)
                ChangeAction(EAniType.RUN);
            else
                ChangeAction(EAniType.WALK);
            _navAgent.destination = point;
        }

        public void SetRoamPositions(Transform root, ETypeRoam type, EKindRoam kind, SpawnControl owner, bool isRandom = false)
        {
            for (int i = 0; i < root.childCount; i++)
            {
                _roamPointList.Add(root.GetChild(i).position);
            }

            _roamingType = type;
            _roamingKind = kind;
            _isRandom = isRandom;
            _ownerParent = owner;
        }

        public void OnBattle(Player p)
        {
            if (_targetPlayer != null)
                return;
            else
                _posBattleStart = transform.position;

            _targetPlayer = p;
            if (!_targetPlayer._isDead)
            {
                if (Vector3.Distance(transform.position, _targetPlayer.transform.position) <= _attackRange)
                    ChangeAction(EAniType.ATTACK);
                else
                {
                    ChangeAction(EAniType.RUN);
                    _navAgent.destination = _targetPlayer.transform.position;
                }
            }
        }

        public void Winner()
        {
            if (_targetPlayer == null)
                return;
            StartCoroutine(WinnerAction());
        }

        void InitMonsterData()
        {
            MonsterInfo monster = DataManager.Instance.GetMonster(_monsterNum);

            InitalizeData(monster._name, monster._hp, monster._att, monster._def);
            _minWaitTime = monster._minWaitTime;
            _maxWaitTime = monster._maxWaitTime;
            _runSpeed = monster._runSpeed;
            _walkSpeed = monster._walkSpeed;
            _sightRange = monster._sightRange;
            _attackRange = monster._attackRange;
            _followDistance = monster._followDistance;
            _rateMoveAct = monster._rateMoveAct;
        }

        IEnumerator WinnerAction()
        {
            ChangeAction(EAniType.IDLE);
            yield return new WaitForSeconds(2.0f);

            ChangeAction(EAniType.BACKHOME);
            _navAgent.destination = _posBattleStart;

            yield return new WaitForSeconds(0.5f);
            StopAllCoroutines();
        }

        /// <summary>
        /// 매 프레임 AI가 선택을 할 수 있는지 확인하고, 선택할 수 있다면 행동에 대한
        /// 선택(대기, 이동)을 하도록 한다.
        /// </summary>
        void SelectAIProcess()
        {
            if (!_isSelectAct)
            {
                bool isNext = true;
                switch (_roamingKind)
                {
                    case EKindRoam.Random:
                        if (_randomPos == _nowIndex)
                        {
                            float rate = Random.Range(0.0f, 100.0f);
                            if (rate <= _rateMoveAct)
                                isNext = true;
                            else
                                isNext = false;
                        }
                        break;
                    case EKindRoam.Patrol:
                        if (_moveCount >= _roamPointList.Count - 1)
                            isNext = false;
                        break;
                    case EKindRoam.Border:
                        if (_moveCount >= _roamPointList.Count * 2 - 1)
                            isNext = false;
                        break;
                }
                if (isNext)
                {
                    SettingGoalPosition(GetNextPosition());
                }
                else
                {
                    _timeCheck = Random.Range(_minWaitTime, _maxWaitTime);
                    ChangeAction(EAniType.IDLE);
                }

                _isSelectAct = true;

                if (_moveCount >= _roamPointList.Count)
                {
                    _moveCount = 0;
                    if (_isRandom)
                    {
                        _roamingType = (ETypeRoam)Random.Range(0, (int)ETypeRoam.Max);
                        _roamingKind = (EKindRoam)Random.Range(0, (int)EKindRoam.Max);
                        if (_roamingKind == EKindRoam.Random)
                            _randomPos = Random.Range(0, _roamPointList.Count);
                    }
                }
            }
        }


        Vector3 GetNextPosition()
        {
            switch (_roamingType)
            {
                case ETypeRoam.Random:
                    _nowIndex = Random.Range(0, _roamPointList.Count);
                    break;
                case ETypeRoam.Loop:
                    _nowIndex++;
                    if (_nowIndex >= _roamPointList.Count)
                        _nowIndex = 0;
                    break;
                case ETypeRoam.PingPong:
                    if (_isBack)
                    {
                        _nowIndex--;
                        if (_nowIndex < 0)
                        {
                            _isBack = false;
                            _nowIndex = 1;
                        }
                    }
                    else
                    {
                        _nowIndex++;
                        if (_nowIndex >= _roamPointList.Count)
                        {
                            _nowIndex = _roamPointList.Count - 2;
                            _isBack = true;
                        }
                    }
                    break;
            }

            _moveCount++;
            return _roamPointList[_nowIndex];
        }

        void MyAnimationList()
        {
            int cnt = 0;
            foreach (AnimationState state in _ctrlAni)
            {
                _aniList.Add((EAniKeyType)cnt, state.name);
                cnt++;
            }
        }

        void ChangeAction(EAniType type)
        {
            switch (type)
            {
                case EAniType.IDLE:
                    _navAgent.enabled = false;
                    _ctrlAni.CrossFade(_aniList[EAniKeyType.IDLE]);
                    break;
                case EAniType.WALK:
                    _navAgent.enabled = true;
                    _navAgent.speed = _walkSpeed;
                    _ctrlAni.CrossFade(_aniList[EAniKeyType.WALK]);
                    break;
                case EAniType.RUN:
                    _navAgent.enabled = true;
                    _navAgent.speed = _runSpeed;
                    _ctrlAni.CrossFade(_aniList[EAniKeyType.RUN]);
                    break;
                case EAniType.ATTACK:
                    _navAgent.enabled = false;
                    _ctrlAni.CrossFade(_aniList[EAniKeyType.ATTACK]);
                    break;
                case EAniType.BACKHOME:
                    _navAgent.enabled = true;
                    _navAgent.speed = _runSpeed * 2;
                    _isInvincibility = true;
                    _timeCheck = 0;
                    _ctrlAni.CrossFade(_aniList[EAniKeyType.RUN]);
                    break;
                case EAniType.DEAD:
                    _isDead = true;
                    _navAgent.enabled = false;
                    _ctrlAni.CrossFade(_aniList[EAniKeyType.DEAD]);
                    Vector3 pos = transform.position;
                    pos.y += 1;
                    GameObject deadEff = Instantiate(_deadEffect, pos, Quaternion.identity);
                    Destroy(deadEff, 2.0f);
                    Destroy(gameObject, 4.0f);
                    break;
            }
            _nowAction = type;
        }

        public override bool OnHitting(int hitDamage)
        {
            if (HittingMe(hitDamage))
            {
                ChangeAction(EAniType.DEAD);
                GetComponent<BoxCollider>().enabled = false;
                ++IngameManager.Instance._countMonsterKill;
            }
            else
            {
            }
            _worldMiniUI.SetHpRate(_hpRate);
            return _isDead;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("BulletObj"))
            {
                Quaternion eular = Quaternion.Euler(other.transform.eulerAngles + new Vector3(0, 180, 0));
                GameObject hitEff = Instantiate(_hitEffect, other.transform.position, eular);
                Bullet bull = other.GetComponent<Bullet>();
                Destroy(hitEff, 1.0f);
                Destroy(other.gameObject);
                if (!_isInvincibility)
                    OnHitting(bull._finalDamage);
                else
                {

                }

                Player p = (Player)other.GetComponent<Bullet>()._owner;
                _ownerParent.AttackAtOnce(p);
            }
        }
    }
}