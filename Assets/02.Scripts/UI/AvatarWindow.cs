using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Outlaw
{
    public class AvatarWindow : MonoBehaviour
    {
        [SerializeField] Text _txtName = null;
        [SerializeField] Text _txtValueHP = null;
        [SerializeField] Text _txtValueAttack = null;
        [SerializeField] Text _txtValueDefense = null;
        [SerializeField] Text _txtValueAmmunition = null;
        [SerializeField] Image _imgValueHP = null;
        [SerializeField] Image _imgValueAttack = null;
        [SerializeField] Image _imgValueDefense = null;
        [SerializeField] GameObject _groupAmmuFirst = null;
        [SerializeField] GameObject _groupAmmuSecond = null;
        [SerializeField] GameObject _prefabAmmu = null;

        AvatarInfo _avatarInfo;

        public void OpenWindow()
        {
            _avatarInfo = DataManager.Instance._userData._playerAvatar;
            ValueSetting();
        }

        void ValueSetting()
        {
            _txtName.text = _avatarInfo._name;
            _txtValueHP.text = _avatarInfo._hp.ToString();
            _txtValueAttack.text = _avatarInfo._att.ToString();
            _txtValueDefense.text = _avatarInfo._def.ToString();
            _txtValueAmmunition.text = _avatarInfo._maxBullet.ToString();
            _imgValueHP.fillAmount = _avatarInfo._hp / AvatarInfo.MAX_HP;
            _imgValueAttack.fillAmount = _avatarInfo._att / AvatarInfo.MAX_ATT;
            _imgValueDefense.fillAmount = _avatarInfo._def / AvatarInfo.MAX_DEF;
            int divideBullet = _avatarInfo._maxBullet / 2;
            for (int i = 0; i < divideBullet; i++)
            {
                Instantiate(_prefabAmmu, _groupAmmuFirst.transform);
            }
            for(int i = divideBullet; i < _avatarInfo._maxBullet; i++)
            {
                Instantiate(_prefabAmmu, _groupAmmuSecond.transform);
            }
        }

        public void CloseWnd()
        {
            Destroy(gameObject);
        }
    }
}