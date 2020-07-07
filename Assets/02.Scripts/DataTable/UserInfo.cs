using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlaw
{
    public struct UserInfo
    {
        public AvatarInfo _playerAvatar;
        public string _name;
        public int _ownGold;
        public int _ownDiamond;
        public int _bestStageClear;
        public StageInfo _nowStage;

        public UserInfo(AvatarInfo playerAvatar, string name,int ownGold, int ownDiamond, int bestStageClear, StageInfo nowStage)
        {
            _playerAvatar = playerAvatar;
            _name = name;
            _ownGold = ownGold;
            _ownDiamond = ownDiamond;
            _bestStageClear = bestStageClear;
            _nowStage = nowStage;
        }
    }
}