using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlaw
{
    public class SpawnControl : MonoBehaviour
    {
        [SerializeField] bool _isRandom = false;
        [SerializeField] ETypeRoam _typeRoam = ETypeRoam.Random;
        [SerializeField] EKindRoam _kindRoam = EKindRoam.Random;
        [SerializeField] int _maxViewCount = 3;
        [SerializeField] int _maxCreateCount = 10;
        [SerializeField] float _intervalCreateTime = 2;
        [SerializeField] string[] _prefabMonsterNames = new string[] { "MonGhost", "MonOrc" };

        List<GameObject> _prefabMon = new List<GameObject>();
        float _timeCheck = 0;

        List<GameObject> _spawnMonList = new List<GameObject>();

        public bool _checkRemainingCount
        {
            get { return _maxCreateCount == 0 && _spawnMonList.Count == 0; }
        }

        private void Awake()
        {
            string path = "Prefabs/Characters/";
            for (int i = 0; i < _prefabMonsterNames.Length; i++)
            {
                _prefabMon.Add(Resources.Load(path + _prefabMonsterNames[i]) as GameObject);
            }
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (IngameManager.Instance._nowGameState != EGameState.Play)
                return;

            if (_maxCreateCount > 0)
            {
                if (_spawnMonList.Count < _maxViewCount)
                {
                    _timeCheck += Time.deltaTime;
                    if (_timeCheck >= _intervalCreateTime)
                    {
                        _timeCheck = 0;

                        GameObject go = Instantiate(_prefabMon[Random.Range(0, _prefabMon.Count)], transform.position, transform.rotation);
                        Monster monster = go.GetComponent<Monster>();
                        if (_isRandom)
                        {
                            int type, kind;
                            type = Random.Range(0, (int)ETypeRoam.Max);
                            kind = Random.Range(0, (int)EKindRoam.Max);
                            _typeRoam = (ETypeRoam)type;
                            _kindRoam = (EKindRoam)kind;
                        }
                        monster.SetRoamPositions(transform.GetChild(0), _typeRoam, _kindRoam, this);
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

        public void AttackAtOnce(Player p)
        {
            for (int i = 0; i < _spawnMonList.Count; i++)
            {
                Monster mon = _spawnMonList[i].GetComponent<Monster>();
                mon.OnBattle(p);
            }
        }

        public void AllNotificationPlayerDeath()
        {
            for (int i = 0; i < _spawnMonList.Count; i++)
            {
                Monster mon = _spawnMonList[i].GetComponent<Monster>();
                mon.Winner();
            }
        }

        public void MonsterDestroy()
        {
            while (_spawnMonList.Count != 0)
            {
                Destroy(_spawnMonList[0].gameObject);
                _spawnMonList.RemoveAt(0);
            }
        }
    }
}