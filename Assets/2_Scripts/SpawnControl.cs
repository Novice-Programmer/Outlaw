using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnControl : MonoBehaviour
{
    [SerializeField] int _maxViewCount = 3;
    [SerializeField] int _maxCreateCount = 10;
    [SerializeField] float _intervalCreateTime = 2;

    GameObject _prefabMon;
    float _timeCheck = 0;

    List<GameObject> _spawnMonList = new List<GameObject>();

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
        if (_maxCreateCount>0)
        {
            if (_spawnMonList.Count < _maxViewCount)
            {
                _timeCheck += Time.deltaTime;
                if (_timeCheck >= _intervalCreateTime)
                {
                    _timeCheck = 0;
                    GameObject go = Instantiate(_prefabMon, transform.position, transform.rotation);
                    Monster monster = go.GetComponent<Monster>();
                    monster.SetRoamPositions(transform.GetChild(0));
                    _spawnMonList.Add(go);
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
}
