using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Outlaw
{
    public class StageLight : MonoBehaviour
    {
        [SerializeField] Vector3 _eularLeft = Vector3.zero;
        [SerializeField] Vector3 _eularRight = Vector3.zero;
        [SerializeField] float _rotateSpeed = 20.0f;

        Quaternion _lookRotate;

        bool _isTurn = false;

        float _timeCheck = 0;

        private void Awake()
        {
        }
        // Start is called before the first frame update
        void Start()
        {
            _lookRotate = Quaternion.Euler(_eularRight);
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void LateUpdate()
        {
            _timeCheck += Time.deltaTime;
            if (_timeCheck > 0.5f)
            {
                _timeCheck = 0;
                Quaternion nextQ = Quaternion.RotateTowards(transform.rotation, _lookRotate, Time.deltaTime * _rotateSpeed);
                if (transform.rotation != _lookRotate)
                    transform.rotation = nextQ;
                else
                {
                    _isTurn = !_isTurn;
                    _lookRotate = Quaternion.Euler(_isTurn ? _eularLeft : _eularRight);
                }
            }
        }
        private void OnMouseEnter()
        {
            
        }
    }
}