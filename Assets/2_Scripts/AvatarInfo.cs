using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct AvatarInfo
{
    string _name;
    int _hp;
    int _att;
    int _def;
    int _maxBullet;

    public AvatarInfo(string name,int hp,int att,int def,int maxBullet)
    {
        _name = name;
        _hp = hp;
        _att = att;
        _def = def;
        _maxBullet = maxBullet;
    }
}
