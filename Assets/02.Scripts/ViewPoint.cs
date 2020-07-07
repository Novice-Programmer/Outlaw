using Outlaw;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Outlaw {
    public class ViewPoint : MonoBehaviour
    {
        [SerializeField] Sprite[] _btnImage = null;

        EViewPoint _nowViewPoint = EViewPoint.ThirdPerson;

        public EViewPoint _viewPoint
        {
            get { return _nowViewPoint; }
        }

        static ViewPoint _uniqueInstance;

        public static ViewPoint Instance
        {
            get { return _uniqueInstance; }
        }
        private void Awake()
        {
            _uniqueInstance = this;
        }

        public void ViewPointChange(Button button)
        {
            switch (_nowViewPoint)
            {
                case EViewPoint.ThirdPerson:
                    button.image.sprite = _btnImage[(int)EViewPoint.ThirdPerson];
                    IngameManager.Instance.ViewChange();
                    _nowViewPoint = EViewPoint.FirstPerson;
                    break;
                case EViewPoint.FirstPerson:
                    button.image.sprite = _btnImage[(int)EViewPoint.FirstPerson];
                    _nowViewPoint = EViewPoint.ThirdPerson;
                    Camera.main.GetComponent<CameraController>().ThirdViewChange();
                    break;
            }
        }
    }
}