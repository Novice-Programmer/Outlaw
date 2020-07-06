using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Outlaw
{
    public struct StageInfo
    {
        public ETypePlanet _planet;
        public ETypeGoal _goal;
        public int _no;
        public int _monsterSpawnPoint;
        public string _stageName;
        public Sprite _stageIcon;
        public MonsterInfo[] _spawnMonsters;

        public StageInfo(ETypePlanet planet,ETypeGoal goal,int no,int monsterSpawnPoint,string stageName,Sprite stageIcon,MonsterInfo[] spawnMonsters)
        {
            _planet = planet;
            _goal = goal;
            _no = no;
            _monsterSpawnPoint = monsterSpawnPoint;
            _stageName = stageName;
            _stageIcon = stageIcon;
            _spawnMonsters = spawnMonsters;
        }
    }
}