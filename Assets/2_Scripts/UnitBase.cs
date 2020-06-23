using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitBase : MonoBehaviour
{
    public enum eAniType
    {
        IDLE = 0,
        RUN,
        WALK_BACK,
        WALK_LEFT,
        WALK_RIGHT,
        ATTACK,
        RELOAD,
        WALK,
        DEAD
    }

    bool _isDie;
    string _name;
    int _life;
    int _att;
    int _def;
    int _curLife;

    protected bool _isDead
    {
        set { _isDie = value; }
        get { return _isDie; }
    }

    protected void InitalizeData(string name, int life, int att, int def)
    {
        _name = name;
        _life = _curLife = life;
        _att = att;
        _def = def;
    }
}
