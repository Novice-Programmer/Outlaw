using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Marker : MonoBehaviour
{
    Vector3 _scale;

    private void Awake()
    {
        _scale = transform.localScale;
    }

    public void ScaleSetting(float size)
    {
        transform.localScale = _scale * size;
    }

    public void OnDestroy()
    {
        MinimapController.Instance.RemoveMarker(this);
    }
}
