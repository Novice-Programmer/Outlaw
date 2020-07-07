using Outlaw;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalArea : MonoBehaviour
{
    bool _isGoal = false;
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void InitGoal()
    {
        gameObject.SetActive(true);
        _isGoal = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            if (_isGoal)
            {
                _isGoal = false;
                IngameManager.Instance.GameEnd(true);
            }
        }
    }
}
