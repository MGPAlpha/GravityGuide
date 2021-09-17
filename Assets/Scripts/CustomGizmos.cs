using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public static class CustomGizmos {
    public static void DrawEventTargets(Vector3 origin, UnityEvent e, Color color) {
        Color oldColor = Gizmos.color;
        Gizmos.color = color;
        for (int i = 0; i < e.GetPersistentEventCount(); i++) {
            Object target = e.GetPersistentTarget(i);
            GameObject o;
            if (target is GameObject) {
                o = (GameObject) target;
            } else {
                o = ((Component) target).gameObject;
            }
            DrawArrow(origin, o.transform.position);
        }
        Gizmos.color = oldColor;
    }

    public static void DrawArrow(Vector3 from, Vector3 to, float arrowHeadLength = 0.4f, float arrowHeadAngle = 30.0f)
    {
        Vector3 direction = to - from;

        Gizmos.DrawRay(from, direction);
       
        // Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0,0,180+arrowHeadAngle) * new Vector3(0,0,1);
        // Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0,0,180-arrowHeadAngle) * new Vector3(0,0,1);
        Vector3 right = Quaternion.Euler(0,0,180+arrowHeadAngle) * direction.normalized;
        Vector3 left = Quaternion.Euler(0,0,180-arrowHeadAngle) * direction.normalized;
        Gizmos.DrawRay(from + direction, right * arrowHeadLength);
        Gizmos.DrawRay(from + direction, left * arrowHeadLength);
    }
}
