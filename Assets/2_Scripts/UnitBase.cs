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

    protected bool _isDead
    {
        set { _isDie = value; }
        get { return _isDie; }
    }
}
