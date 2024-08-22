using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTeleportPoint : MonoBehaviour
{
    private void OnEnable() {
        teleportPoints.Add(index, this);
    }

    private void OnDisable() {
        int indexInList = teleportPoints.IndexOfValue(this);
        if (indexInList < 0) return;
        teleportPoints.RemoveAt(indexInList);    
    }

    [SerializeField] private int index;
    [SerializeField] private float closeRadius = .5f;

    private static SortedList<int, DebugTeleportPoint> teleportPoints = new SortedList<int, DebugTeleportPoint>();
    
    private static (int, int) GetTwoClosestTeleportPoints(Vector2 pos) {
        if (teleportPoints.Count == 0) return (-1, -1);
        int closest = -1;
        int nextClosest = -1;
        float closestDist = float.MaxValue;
        float nextClosestDist = float.MaxValue;
        foreach (KeyValuePair<int,DebugTeleportPoint> p in teleportPoints) {
            Vector2 pointPos = p.Value.transform.position;
            float dist = (pointPos - pos).magnitude;
            if (dist < closestDist) {
                nextClosestDist = closestDist;
                nextClosest = closest;
                closestDist = dist;
                closest = teleportPoints.IndexOfValue(p.Value);
            } else if (dist < nextClosestDist) {
                nextClosestDist = dist;
                nextClosest = teleportPoints.IndexOfValue(p.Value);
            }
        }
        return (closest, nextClosest);
    }

    public static int GetNextTeleportPoint(Vector2 pos) {
        (int, int) twoClosest = GetTwoClosestTeleportPoints(pos);
        if (twoClosest.Item1 == -1) return -1;
        if (twoClosest.Item2 == -1) return twoClosest.Item1;
        return (Mathf.Max(twoClosest.Item1, twoClosest.Item2));
    }

    public static int GetPreviousTeleportPoint(Vector2 pos) {
        (int, int) twoClosest = GetTwoClosestTeleportPoints(pos);
        if (twoClosest.Item1 == -1) return -1;
        if (twoClosest.Item2 == -1) return twoClosest.Item1;
        return (Mathf.Min(twoClosest.Item1, twoClosest.Item2));
    }

    public static int GetNextTeleportPoint(int index) {
        if (teleportPoints.Count == 0) return -1;
        int nextIndex = index + 1;
        if (nextIndex >= teleportPoints.Count) nextIndex = teleportPoints.Count - 1;
        if (nextIndex < 0) nextIndex = 0;
        return nextIndex;
    }

    public static int GetPreviousTeleportPoint(int index) {
        if (teleportPoints.Count == 0) return -1;
        int nextIndex = index - 1;
        if (nextIndex >= teleportPoints.Count) nextIndex = teleportPoints.Count - 1;
        if (nextIndex < 0) nextIndex = 0;
        return nextIndex;
    }

    public static Vector2 GetTeleportPoint(int index) {
        if (teleportPoints.Count == 0) return Vector2.zero;
        if (index >= teleportPoints.Count) index = teleportPoints.Count - 1;
        if (index < 0) index = 0;
        return teleportPoints[teleportPoints.Keys[index]].transform.position;
    }

    public static bool CloseToTeleportPoint(Vector2 pos, int index) {
        if (index >= teleportPoints.Count || index < 0) return false;
        DebugTeleportPoint point = teleportPoints[teleportPoints.Keys[index]];
        Vector2 pointPos = teleportPoints[teleportPoints.Keys[index]].transform.position;
        if ((pointPos - pos).magnitude <= point.closeRadius) return true;
        return false;
    }
    
}
