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
        HIT
    }

    public enum eTypeRoam
    {
        Random  = 0,
        Loop,
        PingPong
    }

    eAniType _nowAction;
    Animation _ctrlAni;
    NavMeshAgent _navAgent;
    Dictionary<eAniKeyType, string> _aniList = new Dictionary<eAniKeyType, string>();
    List<Vector3> _roamPointList = new List<Vector3>();

    [SerializeField] float _runSpeed = 4;
    [SerializeField] float _walkSpeed = 0.7f;
    int _nowIndex = 0;

    private void Awake()
    {
        _ctrlAni = GetComponent<Animation>();
        _navAgent = GetComponent<NavMeshAgent>();
        MyAnimationList();
    }
    void Start()
    {
        _ctrlAni.Play(_aniList[eAniKeyType.IDLE]);

        // 임시
        Transform tf = GameObject.Find("rootRoam").transform;
        SetRoamPositions(tf);
        SettingGoalPosition(GetNextPosition());
        // === === === 
    }

    // Update is called once per frame
    void Update()
    {
        if (_isDead)
            return;

        if (!_ctrlAni.isPlaying)
            ChangeAction(eAniType.IDLE);

        if (Vector3.Distance(transform.position, _navAgent.destination) < 0.1f)
        {
            SettingGoalPosition(GetNextPosition());
        }
                   
    }

    public void SettingGoalPosition(Vector3 point, bool isRun = false)
    {
        if (isRun)
            ChangeAction(eAniType.RUN);
        else
            ChangeAction(eAniType.WALK);
        _navAgent.destination = point;
    }

    public void SetRoamPositions(Transform root)
    {
        for(int i = 0; i < root.childCount; i++)
        {
            _roamPointList.Add(root.GetChild(i).position);
        }
    }

    Vector3 GetNextPosition()
    {
        _nowIndex = Random.Range(0, _roamPointList.Count);
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
            case eAniType.DEAD:
                _navAgent.enabled = false;
                _ctrlAni.CrossFade(_aniList[eAniKeyType.HIT]);
                break;
        }
        _nowAction = type;
    }
}
