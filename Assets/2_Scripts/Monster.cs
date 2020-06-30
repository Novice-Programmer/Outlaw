using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Internal;
using UnityEngine.AI;

public class Monster : UnitBase
{
    enum eAniKeyType
    {
        IDLE = 0,
        WALK,
        RUN,
        ATTACK,
        HIT,
        DEAD
    }

    public enum eTypeRoam
    {
        Random = 0,
        Loop,
        PingPong,
        Max
    }

    public enum eKindRoam
    {
        Random = 0,
        Patrol,
        Border,
        Max
    }

    [Range(0.5f, 3.0f)] [SerializeField] float _minWaitTime = 2.5f;
    [Range(3.0f, 9.9f)] [SerializeField] float _maxWaitTime = 9.9f;
    [SerializeField] HitZone _hitZone = null;
    [SerializeField] SightZone _sightZone = null;
    [SerializeField] WorldStatusWindow _worldMiniUI = null;
    [SerializeField] Marker _marker = null;
    Player _targetPlayer;
    SpawnControl _ownerParent;

    eKindRoam _roamingKind = eKindRoam.Random;
    eTypeRoam _roamingType = eTypeRoam.Random;
    eAniType _nowAction;
    Animation _ctrlAni;
    NavMeshAgent _navAgent;
    Dictionary<eAniKeyType, string> _aniList = new Dictionary<eAniKeyType, string>();
    List<Vector3> _roamPointList = new List<Vector3>();


    // Monster의 기본 정보
    [SerializeField] float _runSpeed = 4;
    [SerializeField] float _walkSpeed = 0.7f;
    [SerializeField] float _sightRange = 10;
    [SerializeField] float _attackRange = 3;
    [SerializeField] float _followDistance = 20;
    [Range(10.0f, 100.0f)] [SerializeField] float _rateMoveAct = 50.0f; // 비전투 행동중 이동을 선택할 확률
    float _timeCheck = 0;

    // Monster의 활용 정보
    int _nowIndex = -1;
    int _moveCount = 0;
    int _randomPos = 0;
    bool _isBack = false;
    bool _isSelectAct = true;  // false일때 선택을 한다. (true면 선택한 상황)
    bool _isRandom = false;
    bool _isInvincibility = false;
    Vector3 _posBattleStart;

    [SerializeField] string _hitEffName = "MonsterHit";
    [SerializeField] string _deadEffName = "MonsterDead";
    GameObject _hitEffect;
    GameObject _deadEffect;

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

        //임시
        InitalizeData("몬스터", 30, 2, 1);
        //
    }
    void Start()
    {
        _ctrlAni.Play(_aniList[eAniKeyType.IDLE]);
        _sightZone.InitSettings(this);
        _hitZone.InitSettings(this);
        _hitZone.EnableTrigger(false);
        if (_roamingKind == eKindRoam.Random)
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
            ChangeAction(eAniType.IDLE);

        switch (_nowAction)
        {
            case eAniType.WALK:
                if (Vector3.Distance(transform.position, _navAgent.destination) < 0.5f)
                {
                    _isSelectAct = false;
                }
                break;
            case eAniType.IDLE:
                _timeCheck -= Time.deltaTime;
                if (_timeCheck <= 0)
                {
                    _isSelectAct = false;
                }
                break;
            case eAniType.RUN:
                if (Vector3.Distance(transform.position, _posBattleStart) >= _followDistance)
                {
                    ChangeAction(eAniType.BACKHOME);
                    _navAgent.destination = _posBattleStart;
                }

                else if (Vector3.Distance(transform.position, _navAgent.destination) < _attackRange)
                    ChangeAction(eAniType.ATTACK);
                else
                    _navAgent.destination = _targetPlayer.transform.position;
                break;
            case eAniType.ATTACK:
                if (_targetPlayer._isDead)
                {
                    Winner();
                }
                else if (Vector3.Distance(transform.position, _targetPlayer.transform.position) > _attackRange)
                {
                    ChangeAction(eAniType.RUN);
                    _navAgent.destination = _targetPlayer.transform.position;
                }
                else
                {
                    AnimationState anistate = _ctrlAni[_aniList[eAniKeyType.ATTACK]];
                    if(anistate.normalizedTime * 100 % 100 > 33)
                    {
                        _hitZone.EnableTrigger(true);
                    }
                    if(anistate.normalizedTime * 100 % 100 > 60)
                    {
                        _hitZone.EnableTrigger(false);
                    }
                }
                    break;
            case eAniType.BACKHOME:
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
            ChangeAction(eAniType.RUN);
        else
            ChangeAction(eAniType.WALK);
        _navAgent.destination = point;
    }

    public void SetRoamPositions(Transform root, eTypeRoam type, eKindRoam kind, SpawnControl owner, bool isRandom = false)
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
                ChangeAction(eAniType.ATTACK);
            else
            {
                ChangeAction(eAniType.RUN);
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
    
    IEnumerator WinnerAction()
    {
        ChangeAction(eAniType.IDLE);
        yield return new WaitForSeconds(2.0f);

        ChangeAction(eAniType.BACKHOME);
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
                case eKindRoam.Random:
                    if (_randomPos == _nowIndex)
                    {
                        float rate = Random.Range(0.0f, 100.0f);
                        if (rate <= _rateMoveAct)
                            isNext = true;
                        else
                            isNext = false;
                    }
                    break;
                case eKindRoam.Patrol:
                    if (_moveCount >= _roamPointList.Count - 1)
                        isNext = false;
                    break;
                case eKindRoam.Border:
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
                ChangeAction(eAniType.IDLE);
            }

            _isSelectAct = true;

            if (_moveCount >= _roamPointList.Count)
            {
                _moveCount = 0;
                if (_isRandom)
                {
                    _roamingType = (eTypeRoam)Random.Range(0, (int)eTypeRoam.Max);
                    _roamingKind = (eKindRoam)Random.Range(0, (int)eKindRoam.Max);
                    if (_roamingKind == eKindRoam.Random)
                        _randomPos = Random.Range(0, _roamPointList.Count);
                }
            }
        }
    }


    Vector3 GetNextPosition()
    {
        switch (_roamingType)
        {
            case eTypeRoam.Random:
                _nowIndex = Random.Range(0, _roamPointList.Count);
                break;
            case eTypeRoam.Loop:
                _nowIndex++;
                if (_nowIndex >= _roamPointList.Count)
                    _nowIndex = 0;
                break;
            case eTypeRoam.PingPong:
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
            _aniList.Add((eAniKeyType)cnt, state.name);
            cnt++;
        }
    }

    void ChangeAction(eAniType type)
    {
        switch (type)
        {
            case eAniType.IDLE:
                _navAgent.enabled = false;
                _ctrlAni.CrossFade(_aniList[eAniKeyType.IDLE]);
                break;
            case eAniType.WALK:
                _navAgent.enabled = true;
                _navAgent.speed = _walkSpeed;
                _ctrlAni.CrossFade(_aniList[eAniKeyType.WALK]);
                break;
            case eAniType.RUN:
                _navAgent.enabled = true;
                _navAgent.speed = _runSpeed;
                _ctrlAni.CrossFade(_aniList[eAniKeyType.RUN]);
                break;
            case eAniType.ATTACK:
                _navAgent.enabled = false;
                _ctrlAni.CrossFade(_aniList[eAniKeyType.ATTACK]);
                break;
            case eAniType.BACKHOME:
                _navAgent.enabled = true;
                _navAgent.speed = _runSpeed * 2;
                _isInvincibility = true;
                _timeCheck = 0;
                _ctrlAni.CrossFade(_aniList[eAniKeyType.RUN]);
                break;
            case eAniType.DEAD:
                _isDead = true;
                _navAgent.enabled = false;
                _ctrlAni.CrossFade(_aniList[eAniKeyType.DEAD]);
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
            ChangeAction(eAniType.DEAD);
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
