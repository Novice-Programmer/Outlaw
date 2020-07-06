using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyGizmo : MonoBehaviour
{
    [SerializeField] Color _color = Color.yellow;
    [SerializeField] float _radius = 0.5f;
    // Start is called before the first frame update

    private void OnDrawGizmos()
    {
        Gizmos.color = _color;
        Gizmos.DrawSphere(transform.position, _radius);
    }
}
