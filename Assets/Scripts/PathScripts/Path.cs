using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public int startWaypoint;

    public List<Waypoint> waypoints = new List<Waypoint>();
    public List<Link> links = new List<Link>();

    public List<Vector3> allPoints = new List<Vector3>();
    public List<Vector3> allAnchors = new List<Vector3>();

    public bool loop;
    public Link loopLink;


    /*[ContextMenu ("Set Transform of waypoints")]
    public void SepPosOfEachWaypoint()
    {
        foreach (Waypoint point in waypoints)
        {
            point.SetSelfTransform();
        }
    }*/


    public void InitializePath()
    {
        allPoints.Clear();
        allAnchors.Clear();
        for (int i = 0; i < links.Count; ++i)
        {
            if (i == 0)
            {
                allPoints.Add(links[i].pathPoints[0]);
            }
            for (int j = 1; j < links[i].pathPoints.Count; ++j)
            {
                allPoints.Add(links[i].pathPoints[j]);
            }
            for (int j = 0; j < links[i].anchors.Count; ++j)
            {
                allAnchors.Add(links[i].anchors[j]);
            }
        }
    }

}
