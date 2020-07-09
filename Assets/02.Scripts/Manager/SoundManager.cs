using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlaw
{
    public class SoundManager : MonoBehaviour
    {
        float _volumeBgm = 0.5f;
        float _volumeEff = 1.0f;

        bool _muteBgm = false;
        bool _muteEff = false;

        [SerializeField] AudioClip[] _bgmClips = null;
        [SerializeField] AudioClip[] _effClips = null;

        AudioSource _playerBGM; // 배경음

        List<AudioSource> _ltPlayEffect; // 효과음

        public float VolumeBGM
        {
            get { return _volumeBgm; }
            set
            {
                _volumeBgm = value;
                _playerBGM.volume = value;
            }
        }

        public float VolumeEffect
        {
            get { return _volumeEff; }
            set { _volumeEff = value; }
        }

        public bool MuteBGM
        {
            get { return _muteBgm; }
            set
            {
                _muteBgm = value;
                _playerBGM.mute = value;
            }
        }

        public bool MuteEff
        {
            get { return _muteEff; }
            set { _muteEff = value; }
        }

        static SoundManager _uniqueInstance;

        public static SoundManager Instance
        {
            get { return _uniqueInstance; }
        }

        private void Awake()
        {
            _uniqueInstance = this;
            DontDestroyOnLoad(this);

            _ltPlayEffect = new List<AudioSource>();
        }

        // Start is called before the first frame update

        private void LateUpdate()
        {
            for (int i = 0; i < _ltPlayEffect.Count; i++)
            {
                if (!_ltPlayEffect[i].isPlaying)
                {
                    Destroy(_ltPlayEffect[i].gameObject);
                    _ltPlayEffect.RemoveAt(i);
                    break;
                }
            }
        }

        void ListUpBGMSound()
        {

        }

        void ListUpEffectSound()
        {

        }

        public AudioSource PlayerBGMSound(ETypeBGMSound eTypeClip)
        {
            if (_playerBGM != null)
                Destroy(_playerBGM.gameObject);
            GameObject obj = new GameObject("BGMPlayer");
            obj.transform.parent = Camera.main.transform;
            obj.transform.localPosition = Vector3.zero;

            _playerBGM = obj.AddComponent<AudioSource>();
            _playerBGM.clip = _bgmClips[(int)eTypeClip];
            _playerBGM.volume = _volumeBgm;
            _playerBGM.mute = _muteBgm;
            _playerBGM.loop = true;
            _playerBGM.Play();
  
            return _playerBGM;
        }

        public AudioSource PlayEffectSound(ETypeEffectSound type, Transform owner, bool loop = false, float soundDistance = 50.0f)
        {
            GameObject obj = new GameObject("EffectSound");
            obj.transform.parent = owner;
            transform.localPosition = Vector3.zero;
            AudioSource effPlayer = obj.AddComponent<AudioSource>();
            effPlayer.clip = _effClips[(int)type];
            effPlayer.volume = _volumeEff;
            effPlayer.mute = _muteEff;
            effPlayer.loop = loop;
            effPlayer.minDistance = soundDistance;
            effPlayer.Play();
            _ltPlayEffect.Add(effPlayer);

            return effPlayer;
        }
    }
}