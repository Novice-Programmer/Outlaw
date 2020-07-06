using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Outlaw
{
    public class ScoreWindow : MonoBehaviour
    {
        [SerializeField] Text _scoreText = null;
        [SerializeField] Text _addScoreText = null;
        CanvasGroup _scoregroup = null;
        float _hideSpeed = 2.5f;

        private void Awake()
        {
            _scoregroup = GetComponent<CanvasGroup>();
        }

        // Start is called before the first frame update
        void Start()
        {
            _scoregroup.alpha = 0.0f;
        }

        private void Update()
        {
            if (_scoregroup.alpha > 0.0f)
            {
                _scoregroup.alpha -= Time.deltaTime / _hideSpeed;
            }
        }

        public void ScoreUpdate(int score, int addScore)
        {
            _scoreText.text = score.ToString();
            string addScoreString = "";
            if (addScore >= 0)
            {
                _addScoreText.color = Color.yellow;
                addScoreString += "+";
            }
            else
            {
                _addScoreText.color = Color.red;
            }
            addScoreString += addScore;
            _addScoreText.text = addScoreString;
            _scoregroup.alpha = 1.0f;
            IngameManager.Instance._scoreTotal += addScore;
        }
    }
}