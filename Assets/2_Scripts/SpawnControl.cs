using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnControl : MonoBehaviour
{
    [SerializeField] bool _isRandom = false;
    [SerializeField] Monster.eTypeRoam _typeRoam = Monster.eTypeRoam.Random;
    [SerializeField] Monster.eKindRoam _kindRoam = Monster.eKindRoam.Random;
    [SerializeField] int _maxViewCount = 3;
    [SerializeField] int _maxCreateCount = 10;
    [SerializeField] float _intervalCreateTime = 2;

    GameObject _prefabMon;
    float _timeCheck = 0;

    List<Monster> _spawnMonList = new List<Monster>();

    private void Awake()
    {
        _prefabMon = Resources.Load("Prefabs/Character/MonGhost") as GameObject;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_maxCreateCount > 0)
        {
            if (_spawnMonList.Count < _maxViewCount)
            {
                _timeCheck += Time.deltaTime;
                if (_timeCheck >= _intervalCreateTime)
                {
                    _timeCheck = 0;
                    GameObject go = Instantiate(_prefabMon, transform.position, transform.rotation);
                    Monster monster = go.GetComponent<Monster>();
                    if (_isRandom)
                    {
                        int type, kind;
                        type = Random.Range(0, (int)Monster.eTypeRoam.Max);
                        kind = Random.Range(0, (int)Monster.eKindRoam.Max);
                        _typeRoam = (Monster.eTypeRoam)type;
                        _kindRoam = (Monster.eKindRoam)kind;
                    }
                    monster.SetRoamPositions(transform.GetChild(0), _typeRoam, _kindRoam, this);
                    _spawnMonList.Add(monster);
                    _maxCreateCount--;
                }
            }
        }
    }

    private void LateUpdate()
    {
        for (int i = 0; i < _spawnMonList.Count; i++)
        {
            if (_spawnMonList[i] == null)
            {
                _spawnMonList.RemoveAt(i);
                break;
            }
        }
    }

    public void HitMonster(Player p)
    {
        for(int i = 0; i < _spawnMonList.Count; i++)
        {
            if (_spawnMonList[i]._target == null)
            {
                _spawnMonList[i].OnBattle(p);
            }
        }
    }

    public void WinMonster()
    {
        for (int i = 0; i < _spawnMonList.Count; i++)
        {
            if (_spawnMonList[i]._target == null)
            {
                _spawnMonList[i].Winner();
            }
        }
    }
}
