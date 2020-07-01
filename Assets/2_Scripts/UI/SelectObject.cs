using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectObject : MonoBehaviour
{
    enum ESelectType
    {
        Normal = 0,
        Select,
    }

    [SerializeField] LobbyManager.ETypeWindow _windowType = LobbyManager.ETypeWindow.StageWnd;
    [SerializeField] Material[] _mats = null;
    MeshRenderer _modelRenderer;

    bool _isIn = false;
    

    private void Start()
    {
        _modelRenderer = transform.GetComponentInChildren<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        _modelRenderer.material = _mats[(int)ESelectType.Select];
    }

    private void OnMouseUp()
    {
        if (_isIn)
        {
            LobbyManager.Instance.OpenWindow(_windowType);
        }
        _modelRenderer.material = _mats[(int)ESelectType.Normal];
    }

    private void OnMouseEnter()
    {
        _isIn = true;
    }

    private void OnMouseExit()
    {
        _isIn = false;
    }
}
