using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Animal : MonoBehaviour
{
    public enum eTypeRoam
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
    [SerializeField] int _hp = 10;
    int _nowIndex = -1;
    int _moveCount = 0;

    List<Vector3> _movePoint = new List<Vector3>();
    private void Awake()
    {
        _navAgent = GetComponent<NavMeshAgent>();
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
        SettingGoalPosition(GetNextPosition());
        _navAgent.speed = _moveSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, _navAgent.destination) < 0.3f)
        {
            SettingGoalPosition(GetNextPosition());
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
        }

        return _movePoint[_nowIndex];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("BulletObj"))
        {
            _hp--;
            GameObject go = Instantiate(_effhit, other.transform.position, Quaternion.identity);
            Destroy(go, 2.0f);
            if (_hp <= 0)
            {

            }
        }
    }
}
