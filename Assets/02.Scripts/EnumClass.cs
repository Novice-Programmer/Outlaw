using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlaw
{
    public enum ETypePlanet
    {
        None = 0,
        스탄피드,
        템플크론,
        마그네온
    }

    public enum ETypeGoal
    {
        모든적을제거 = 0,
        특정지역방문,
        특정건물파괴,
        일정건물파괴,
        보스처치
    }

    public enum ESceneType
    {
        START = 0,
        LOBBY,
        INGAME
    }

    public enum ELoaddingState
    {
        None = 0,
        LoadSceneStart,
        LoaddingScene,
        LoadSceneEnd,
        LoadStageStart,
        LoaddingStage,
        LoadStageEnd,
        LoadEnd
    }

    public enum ELoadType
    {
        Lobby,
        Planet
    }

    public enum EGameState
    {
        None = 0,
        Ready,
        SpawnPlayer,
        Start,
        Play,
        End,
        Result
    }

    public enum EActionScore
    {
        MonsterKill,
        AnimalKill,
        BrokenObject
    }

    public enum EAniKeyType
    {
        IDLE = 0,
        WALK,
        RUN,
        ATTACK,
        HIT,
        DEAD
    }

    public enum ETypeRoam
    {
        Random = 0,
        Loop,
        PingPong,
        Max
    }

    public enum EKindRoam
    {
        Random = 0,
        Patrol,
        Border,
        Max
    }

    public enum ETypeAnimalRoam
    {
        Random = 0,
        Loop,
        Max
    }

    public enum EAniType
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

    public enum ETypeBGMSound
    {
        Lobby = 0,
        Stage1,
        Stage2,
        Stage3,
        Stage4,
        Stage5,
        Stage6
    }

    public enum ETypeEffectSound
    {
        OpenDoor,
        CloseDoor
    }

    public enum ETypeWindow
    {
        StageWnd = 0,
        CharacterInfoWnd,
        EnhanceWnd
    }

    public enum ESelectType
    {
        Normal = 0,
        Select,
    }

    public enum EViewPoint
    {
        ThirdPerson = 0,
        FirstPerson
    }
    public class EnumClass
    {
    }

}