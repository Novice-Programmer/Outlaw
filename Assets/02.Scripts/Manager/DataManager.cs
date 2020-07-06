using Outlaw;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Outlaw
{
    public class DataManager : MonoBehaviour
    {
        [SerializeField] Sprite[] stageIcons = null;

        UserInfo _userInfo;
        Dictionary<ETypePlanet, Dictionary<int, StageInfo>> _dicStageInfo;
        Dictionary<int, AvatarInfo> _dicAvatarInfo;
        Dictionary<int, MonsterInfo> _dicMonsterInfo;


        public UserInfo _userData
        {
            get { return _userInfo; }
        }

        static DataManager _uniqueInstance;

        public static DataManager Instance
        {
            get { return _uniqueInstance; }
        }

        private void Awake()
        {
            _uniqueInstance = this;
            DontDestroyOnLoad(gameObject);
            _dicStageInfo = new Dictionary<ETypePlanet, Dictionary<int, StageInfo>>();
            _dicAvatarInfo = new Dictionary<int, AvatarInfo>();
            _dicMonsterInfo = new Dictionary<int, MonsterInfo>();
        }

        private void Start()
        {
            DataGet();
        }

        void DataGet()
        {
            AvatarDataGet();
            MonsterDataGet();
            StageDataGet();
            UserDataGet();
        }
        void AvatarDataGet()
        {
            AvatarInfo baseAvatar = new AvatarInfo("기본 슈트", 0, 100, 3, 1, 12);
            _dicAvatarInfo.Add(baseAvatar._no, baseAvatar);
            AvatarInfo upgradeAvatar = new AvatarInfo("강화 슈트", 1, 140, 5, 2, 18);
            _dicAvatarInfo.Add(upgradeAvatar._no, upgradeAvatar);
        }

        void MonsterDataGet()
        {
            MonsterInfo ghost = new MonsterInfo("고스트", 0, 20, 2, 0, 4, 0.7f, 10, 3, 15, 65);
            _dicMonsterInfo.Add(ghost._no, ghost);
            MonsterInfo orc = new MonsterInfo("오크", 1, 25, 3, 1, 6, 1, 15, 2, 20, 50);
            _dicMonsterInfo.Add(orc._no, orc);
        }

        void StageDataGet()
        {
            Dictionary<int, StageInfo> stageStanfeed = new Dictionary<int, StageInfo>();
            StageInfo stage1 = new StageInfo(ETypePlanet.스탄피드, ETypeGoal.모든적을제거, 0, 3, "SF-173", stageIcons[0], 
                new MonsterInfo[] { _dicMonsterInfo[0]});
            stageStanfeed.Add(stage1._no, stage1);
            StageInfo stage2 = new StageInfo(ETypePlanet.스탄피드, ETypeGoal.특정건물파괴, 1, 4, "SF-199", stageIcons[1],
                new MonsterInfo[] { _dicMonsterInfo[0], _dicMonsterInfo[1] });
            stageStanfeed.Add(stage2._no, stage2);

            _dicStageInfo.Add(ETypePlanet.스탄피드, stageStanfeed);

            Dictionary<int, StageInfo> stageTempleCrone = new Dictionary<int, StageInfo>();
            StageInfo stage3 = new StageInfo(ETypePlanet.템플크론, ETypeGoal.모든적을제거, 0, 4, "TC-000", stageIcons[0],
                new MonsterInfo[] { _dicMonsterInfo[0] });
            stageTempleCrone.Add(stage3._no, stage3);
            StageInfo stage4 = new StageInfo(ETypePlanet.템플크론, ETypeGoal.특정건물파괴, 1, 5, "TC-31752", stageIcons[1],
                new MonsterInfo[] { _dicMonsterInfo[0], _dicMonsterInfo[1] });
            stageTempleCrone.Add(stage4._no, stage4);

            _dicStageInfo.Add(ETypePlanet.템플크론, stageTempleCrone);
        }

        void UserDataGet()
        {
            UserInfo firstInfo = new UserInfo(_dicAvatarInfo[0], "Player", 1321, 27, 0, 1);
            _userInfo = firstInfo;
        }

        public List<StageInfo> GetStages(ETypePlanet planet)
        {
            List<StageInfo> stages = new List<StageInfo>();
            if (_dicStageInfo.ContainsKey(planet))
            {
                Dictionary<int, StageInfo> dicStage = _dicStageInfo[planet];
                foreach (int stageNum in dicStage.Keys)
                {
                    StageInfo stage = dicStage[stageNum];
                    stages.Add(stage);
                }
            }
            else
                return null;
            return stages;
        }

        public MonsterInfo GetMonster(int monsterNumber)
        {
            if (_dicMonsterInfo.ContainsKey(monsterNumber))
                return _dicMonsterInfo[monsterNumber];
            else
                return new MonsterInfo();
        }

        public List<AvatarInfo> GetAvatars()
        {
            List<AvatarInfo> avatars = new List<AvatarInfo>();
            foreach(int avatarNum in _dicAvatarInfo.Keys)
            {
                AvatarInfo avatar = _dicAvatarInfo[avatarNum];
                avatars.Add(avatar);
            }
            return avatars;
        }
    }
}