using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawPoint : MonoBehaviour
{
    public float Radius = 1f;
    public bool WireMode;
    public Color Color = Color.yellow;
#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color;

        if (WireMode)
            Gizmos.DrawWireSphere(transform.position, Radius);
        else
            Gizmos.DrawSphere(transform.position, Radius);
    }

#endif
}
