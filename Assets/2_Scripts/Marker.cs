using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
    Vector3 _scale;
    float _scaleSize = 1;

    public float _sizeScale
    {
        set { _scaleSize = value; }
    }

    private void Awake()
    {
        _scale = transform.localScale;
    }

    private void Start()
    {
        transform.localScale = _scale * _scaleSize;
    }

    public void ScaleSetting(float size)
    {
        transform.localScale = _scale * _scaleSize * size;
    }

    public void OnDestroy()
    {
        MinimapController.Instance.RemoveMarker(this);
    }
}
