using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlaw
{
    public struct AvatarInfo
    {
        public static float MAX_HP = 200;
        public static float MAX_ATT = 20;
        public static float MAX_DEF = 10;
        public int _no;
        public string _name;
        public int _hp;
        public int _att;
        public int _def;
        public int _maxBullet;

        public AvatarInfo(string name, int no, int hp, int att, int def, int maxBullet)
        {
            _name = name;
            _no = no;
            _hp = hp;
            _att = att;
            _def = def;
            _maxBullet = maxBullet;
        }
    }
}