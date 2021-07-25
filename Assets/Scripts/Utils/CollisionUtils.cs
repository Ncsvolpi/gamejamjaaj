using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CollisionUtils 
{
    public static RaycastHit2D CustomRaycast(Vector2 positionToCast,Vector2 offset, Vector2 rayDirection, float length, LayerMask mask, bool DebugRayCast)
    {
        RaycastHit2D hit = Physics2D.Raycast(positionToCast + offset, rayDirection, length, mask);

        if (DebugRayCast)
        {
            Color color = hit ? Color.green : Color.red;
            Debug.DrawRay(positionToCast + offset, rayDirection * length, color);
        }

        return hit;
    }

}
