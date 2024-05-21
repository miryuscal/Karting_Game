using System.Collections.Generic;
using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public Transform[] waypoints;

    private void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length == 0)
            return;

        for (int i = 0; i < waypoints.Length; i++)
        {
            Gizmos.DrawSphere(waypoints[i].position, 1f);
            if (i < waypoints.Length - 1)
                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
            else
                Gizmos.DrawLine(waypoints[i].position, waypoints[0].position);
        }
    }
}
