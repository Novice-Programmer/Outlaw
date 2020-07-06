using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlaw
{
    public struct MonsterInfo
    {
        public int _no;
        public string _name;
        public int _hp;
        public int _att;
        public int _def;
        public float _runSpeed;
        public float _walkSpeed;
        public float _sightRange;
        public float _attackRange;
        public float _returnRange;
        public float _restRate;

        public MonsterInfo(string name,int no,int hp,int att,int def,float runSpeed,float walkSpeed,float sightRange,float attackRange,float returnRange,float restRate)
        {
            _name = name;
            _no = no;
            _hp = hp;
            _att = att;
            _def = def;
            _runSpeed = runSpeed;
            _walkSpeed = walkSpeed;
            _sightRange = sightRange;
            _attackRange = attackRange;
            _returnRange = returnRange;
            _restRate = restRate;
        }
    }
}