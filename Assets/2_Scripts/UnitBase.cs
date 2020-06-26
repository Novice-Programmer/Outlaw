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
        BACKHOME,
        DEAD
    }

    bool _isDie;
    string _name;
    int _life;
    int _att;
    int _def;
    int _curLife;

    protected int _baseAtt
    {
        get { return _att; }
    }

    protected bool _isDead
    {
        set { _isDie = value; }
        get { return _isDie; }
    }

    protected string _myName
    {
        get { return _name; }
    }

    public float _hpRate
    {
        get { return (float)_curLife / _life; }
    }

    public float _energyRate
    {
        get;
    }

    protected void LifeSet(int life, bool isExcess = false)
    {
        _curLife += life;

        if (_curLife >= _life)
        {
            _curLife = isExcess ? _curLife : _life;
        }
    }

    protected void InitalizeData(string name, int life, int att, int def)
    {
        _name = name;
        _life = _curLife = life;
        _att = att;
        _def = def;
    }

    protected bool HittingMe(int dam)
    {
        int finishD = dam - _def;
        if (finishD < 1) finishD = 1;


        _curLife -= finishD;
        if (_curLife <= 0)
        {
            _curLife = 0;
            return true;
        }

        return false;
    }
}
