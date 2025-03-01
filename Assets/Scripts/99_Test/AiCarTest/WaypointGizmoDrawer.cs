using UnityEngine;
using System.Collections.Generic;

public class WaypointGizmoDrawer : MonoBehaviour
{
    public Color gizmoColor = Color.green;
    public Transform waypointsParent;
    public int curveResolution = 30;
    public bool loop = true;
    public float controlPointFactor = 0.5f;

    public List<Vector3> bezierPoints = new List<Vector3>(); // Bezier 曲線のポイントリスト

    private Transform[] waypoints;

    private void OnDrawGizmos()
    {
        if (waypointsParent == null)
        {
            Debug.LogWarning("Waypoints Parent is not assigned!");
            return;
        }

        bezierPoints.Clear(); // 毎フレーム更新するためクリア

        List<Transform> waypointList = new List<Transform>();
        foreach (Transform child in waypointsParent)
        {
            waypointList.Add(child);
        }
        waypoints = waypointList.ToArray();

        if (waypoints.Length < 2) return;

        Gizmos.color = gizmoColor;

        for (int i = 0; i < waypoints.Length; i++)
        {
            Vector3 p0 = waypoints[i].position;
            Vector3 p1 = waypoints[GetWrappedIndex(i + 1)].position;

            Vector3 cp0 = p0 + (waypoints[GetWrappedIndex(i - 1)].position - waypoints[GetWrappedIndex(i + 1)].position) * controlPointFactor;
            Vector3 cp1 = p1 + (waypoints[GetWrappedIndex(i + 2)].position - waypoints[i].position) * controlPointFactor;

            Vector3 previousPoint = p0;
            bezierPoints.Add(previousPoint); // 最初の点を追加

            for (int j = 1; j <= curveResolution; j++)
            {
                float t = j / (float)curveResolution;
                Vector3 newPoint = BezierCurve(p0, cp0, cp1, p1, t);
                Gizmos.DrawLine(previousPoint, newPoint);
                bezierPoints.Add(newPoint);
                previousPoint = newPoint;
            }

            Gizmos.DrawSphere(p0, 0.3f);
        }
    }

    private int GetWrappedIndex(int index)
    {
        if (loop)
        {
            return (index + waypoints.Length) % waypoints.Length;
        }
        else
        {
            return Mathf.Clamp(index, 0, waypoints.Length - 1);
        }
    }

    private Vector3 BezierCurve(Vector3 p0, Vector3 cp0, Vector3 cp1, Vector3 p1, float t)
    {
        float u = 1 - t;
        float uu = u * u;
        float uuu = uu * u;
        float tt = t * t;
        float ttt = tt * t;

        return (uuu * p0) + (3 * uu * t * cp0) + (3 * u * tt * cp1) + (ttt * p1);
    }
}
