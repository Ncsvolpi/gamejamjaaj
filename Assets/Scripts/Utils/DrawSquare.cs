using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawSquare : MonoBehaviour
{
    public Color Color = Color.yellow;
    public bool WireMode;
    private BoxCollider2D collider;
#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (collider == null)
            collider = GetComponent<BoxCollider2D>();

        Gizmos.color = Color;

        if (WireMode)
            Gizmos.DrawWireCube(transform.position, collider.size);
        else
            Gizmos.DrawCube(transform.position, collider.size);
    }

#endif
}
