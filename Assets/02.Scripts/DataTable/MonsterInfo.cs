using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlaw
{
    public struct MonsterInfo
    {
        public int _no;
        public string _name;
        public string _fileName;
        public int _hp;
        public int _att;
        public int _def;
        public float _runSpeed;
        public float _walkSpeed;
        public float _sightRange;
        public float _attackRange;
        public float _followDistance;
        public float _rateMoveAct;
        public float _minWaitTime;
        public float _maxWaitTime;

        public MonsterInfo(string name, string fileName, int no, int hp, int att, int def, float runSpeed, float walkSpeed, float sightRange, float attackRange,
            float returnRange, float restRate, float minWaitTime, float maxWaitTime)
        {
            _name = name;
            _fileName = fileName;
            _no = no;
            _hp = hp;
            _att = att;
            _def = def;
            _runSpeed = runSpeed;
            _walkSpeed = walkSpeed;
            _sightRange = sightRange;
            _attackRange = attackRange;
            _followDistance = returnRange;
            _rateMoveAct = restRate;
            _minWaitTime = minWaitTime;
            _maxWaitTime = maxWaitTime;
        }
    }
}