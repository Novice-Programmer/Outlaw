using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    Monster _testMon;
    // Start is called before the first frame update
    void Start()
    {
        _testMon = GameObject.FindGameObjectWithTag("Monster").GetComponent<Monster>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            int lMask = 1 << LayerMask.NameToLayer("FIELD");
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, lMask))
            {
                _testMon.SettingGoalPosition(hit.point);
            }
        }
    }
}
