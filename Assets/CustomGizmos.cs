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
            Gizmos.DrawLine(origin, o.transform.position);
        }
        Gizmos.color = oldColor;
    }
}
