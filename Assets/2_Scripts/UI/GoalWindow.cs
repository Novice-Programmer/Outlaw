using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Outlaw
{
    public class GoalWindow : MonoBehaviour
    {
        [SerializeField] Text _explanText = null;
        CanvasGroup _goalGroup = null;

        float _hideSpeed = 5.0f;

        private void Awake()
        {
            _goalGroup = GetComponent<CanvasGroup>();
        }

        // Start is called before the first frame update
        void Start()
        {
            _goalGroup.alpha = 0.0f;
        }

        // Update is called once per frame
        void Update()
        {
            if (_goalGroup.alpha > 0.0f)
            {
                _goalGroup.alpha -= Time.deltaTime / _hideSpeed;
            }
        }

        public void GoalUpdate(string explanString = "모든 적들을 찾아 제거하시오.", bool isWarning = false)
        {
            if (isWarning)
                _explanText.color = Color.red;
            else
                _explanText.color = Color.white;
            _explanText.text = explanString;
            _goalGroup.alpha = 1.0f;
        }
    }
}