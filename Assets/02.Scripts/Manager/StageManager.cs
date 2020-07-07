using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlaw
{
    public class StageManager : MonoBehaviour
    {
        GameObject _startEffect;
        bool _isClear = false;

        // 스테이지 정보
        StageInfo _stage;

        // 스테이지 오브젝트 관련
        List<SpawnControl> _spawnPointList = new List<SpawnControl>();
        List<Animal> _animals = new List<Animal>();
        int _maxSpawnPoint = 3;

        // 클리어 관련
        ETypeGoal _goal;
        GoalArea _goalArea;
        BrokenObject _goalBrokenObject;
        int _goalBrokenScore = 0;

        // 기타
        bool _firstCheck = false;

        public bool _clearCheck
        {
            get { return _isClear; }
        }

        public StageInfo _nowStageInfo
        {
            get { return _stage; }
        }

        public string _goalString
        {
            get
            {
                string goalString = "";
                switch (_goal)
                {
                    case ETypeGoal.모든적을제거:
                        goalString = "모든 적을 제거하시오.";
                        break;
                    case ETypeGoal.특정지역방문:
                        goalString = "특정 지역을 방문하시오.";
                        break;
                    case ETypeGoal.특정건물파괴:
                        goalString = "특정 건물을 파괴하시오.";
                        break;
                    case ETypeGoal.일정건물파괴:
                        goalString = "건물을 일정량 파괴하시오.";
                        break;
                    case ETypeGoal.보스처치:
                        goalString = "몹을 일정 잡고 보스를 처치하시오.";
                        break;
                }
                return goalString;
            }
        }

        static StageManager _uniqueInstance;

        public static StageManager Instance
        {
            get { return _uniqueInstance; }
        }
        private void Awake()
        {
            _uniqueInstance = this;
            _stage = DataManager.Instance._userData._nowStage;
            _maxSpawnPoint = _stage._monsterSpawnPoint;
        }
        private void Start()
        {
            _startEffect = GameObject.Find("PlayerStartEffect");
            _startEffect.SetActive(_firstCheck);
            ListUpSpawnControl();
            AnimalGetList();
            GoalSetting();
        }
        // Update is called once per frame
        void Update()
        {
            if (!_firstCheck)
            {
                if (IngameManager.Instance._nowGameState == EGameState.SpawnPlayer)
                {
                    _firstCheck = true;
                    _startEffect.SetActive(_firstCheck);
                }
            }
        }

        private void LateUpdate()
        {
            CheckClearConditions();
        }

        void GoalSetting()
        {
            _goal = _stage._goal;

            switch (_goal)
            {
                case ETypeGoal.모든적을제거:
                    break;
                case ETypeGoal.특정지역방문:
                    GoalArea[] goalCandArea = FindObjectsOfType<GoalArea>();
                    int rid = Random.Range(0, goalCandArea.Length);
                    _goalArea = goalCandArea[rid];
                    _goalArea.InitGoal();
                    break;
                case ETypeGoal.특정건물파괴:
                    BrokenObject[] brokenObjects = FindObjectsOfType<BrokenObject>();
                    List<BrokenObject> goalCandBroken = new List<BrokenObject>();
                    for(int i = 0; i < brokenObjects.Length; i++)
                    {
                        if (brokenObjects[i]._durability > 80 && brokenObjects[i]._durability < 999)
                            goalCandBroken.Add(brokenObjects[i]);
                    }
                    int rid2 = Random.Range(0, goalCandBroken.Count);
                    _goalBrokenObject = goalCandBroken[rid2];
                    _goalBrokenObject.InitGoal();
                    break;
                case ETypeGoal.일정건물파괴:
                    _goalBrokenScore = Random.Range(50 * _stage._no, 100 * _stage._no);
                    break;
                case ETypeGoal.보스처치:
                    break;
            }
        }

        void ListUpSpawnControl()
        {
            int removeCount = _spawnPointList.Count - _maxSpawnPoint;
            SpawnControl[] scArray = FindObjectsOfType<SpawnControl>();
            for (int i = 0; i < scArray.Length; i++)
            {
                _spawnPointList.Add(scArray[i]);
            }
            for (int i = 0; i < removeCount; i++)
            {
                int rid = Random.Range(0, _spawnPointList.Count);
                Destroy(_spawnPointList[rid].gameObject);
                _spawnPointList.RemoveAt(rid);
            }
        }

        void AnimalGetList()
        {
            GameObject[] animalObjects = GameObject.FindGameObjectsWithTag("Animal");
            for (int i = 0; i < animalObjects.Length; i++)
            {
                Animal animal = animalObjects[i].GetComponent<Animal>();
                _animals.Add(animal);
            }
        }

        void CheckClearConditions()
        {
            bool result = false;
            switch (_goal)
            {
                case ETypeGoal.모든적을제거:
                    int cnt = 0;
                    foreach (SpawnControl item in _spawnPointList)
                    {
                        if (item._checkRemainingCount)
                            cnt++;
                    }

                    if (cnt >= _maxSpawnPoint)
                        result = true;
                    break;
                case ETypeGoal.일정건물파괴:
                    result = IngameManager.Instance._scoreBroken > _goalBrokenScore;
                    break;
                case ETypeGoal.보스처치:
                    break;

            }

            if (result)
            {
                ObjectClear();
            }

            _isClear = result;
        }

        void ObjectClear()
        {
            for (int i = 0; i < _spawnPointList.Count; i++)
            {
                _spawnPointList[i].MonsterDestroy();
            }
            for (int i = 0; i < _animals.Count; i++)
            {
                if (_animals[i] != null)
                {
                    Destroy(_animals[i]);
                }
            }
        }


        public void ReceivePlayerDie()
        {
            for (int i = 0; i < _spawnPointList.Count; i++)
            {
                _spawnPointList[i].AllNotificationPlayerDeath();
            }
        }
    }
}