using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;

public class Animal : UnitBase
{
    enum eTypeRoam
    {
        Random = 0,
        Loop,
        Max
    }

    [SerializeField] eTypeRoam _roamingType = eTypeRoam.Random;
    [SerializeField] GameObject _movePoints = null;
    [SerializeField] string _hitEffectName = "Hit";

    GameObject _effhit;
    Transform _rootPoint;
    NavMeshAgent _navAgent;

    [SerializeField] float _moveSpeed = 0;
    [Range(0.5f, 2.0f)] [SerializeField] float _minRate = 0.5f;
    [Range(2.0f, 4.0f)] [SerializeField] float _maxRate = 2.0f;
    [SerializeField] string _animalName = "Animal";
    [SerializeField] int _animalHP = 10;
    [SerializeField] int _animalDef = 0;

    float _rateTime = 0;
    int _nowIndex = -1;
    int _moveCount = 0;

    Animator _ctrlAni;
    eAniType _nowAniType = eAniType.RUN;

    List<Vector3> _movePoint = new List<Vector3>();
    private void Awake()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        _ctrlAni = GetComponent<Animator>();
        InitalizeData(_animalName, _animalHP, 0, _animalDef);
    }
    void Start()
    {
        string path = "Prefabs/ParticleEffects/" + _hitEffectName;
        _effhit = Resources.Load(path) as GameObject;
        GameObject go = GameObject.Find("AnimalMovePoint");
        _rootPoint = Instantiate(_movePoints, transform.position, transform.rotation,go.transform).transform;
        for(int i = 0; i < _rootPoint.childCount; i++)
        {
            _movePoint.Add(_rootPoint.GetChild(i).transform.position);
        }
        ChangeAnimation(eAniType.RUN);
        SettingGoalPosition(GetNextPosition());
        _navAgent.speed = _moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        switch (_nowAniType)
        {
            case eAniType.IDLE:
                _rateTime -= Time.deltaTime;
                if (_rateTime <= 0)
                {
                    ChangeAnimation(eAniType.RUN);
                }

                break;
            case eAniType.RUN:
                if (Vector3.Distance(transform.position, _navAgent.destination) < 0.3f)
                    SettingGoalPosition(GetNextPosition());
                break;
        }

    }

    public void SettingGoalPosition(Vector3 point, bool isRun = false)
    {
        _navAgent.destination = point;
    }

    Vector3 GetNextPosition()
    {
        switch (_roamingType)
        {
            case eTypeRoam.Random:
                _nowIndex = Random.Range(0, _movePoint.Count);
                break;
            case eTypeRoam.Loop:
                _nowIndex++;
                if (_nowIndex >= _movePoint.Count)
                    _nowIndex = 0;
                break;
        }

        _moveCount++;
        if (_moveCount == _movePoint.Count * 2)
        {
            _moveCount = 0;
            _roamingType = (eTypeRoam)Random.Range(0, (int)eTypeRoam.Max);
            ChangeAnimation(eAniType.IDLE);
            _rateTime = Random.Range(_minRate, _maxRate);
        }

        return _movePoint[_nowIndex];
    }

    void ChangeAnimation(eAniType aniType)
    {
        switch (aniType)
        {
            case eAniType.IDLE:
                _ctrlAni.SetBool("IsRun", false);
                break;
            case eAniType.RUN:
                _ctrlAni.SetBool("IsRun", true);
                break;
            case eAniType.DEAD:
                _ctrlAni.SetTrigger("Die");
                Destroy(gameObject, 1.0f);
                break;
        }

        _nowAniType = aniType;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BulletObj"))
        {
            Bullet bull = other.GetComponent<Bullet>();
            Destroy(other.gameObject);
            Quaternion eular = Quaternion.Euler(other.transform.eulerAngles + new Vector3(0, 180, 0));
            GameObject go = Instantiate(_effhit, other.transform.position, eular);
            Destroy(go, 2.0f);
            if (HittingMe(bull._finalDamage))
            {
                ChangeAnimation(eAniType.DEAD);
                GetComponent<BoxCollider>().enabled = false;
            }
        }
    }
}
