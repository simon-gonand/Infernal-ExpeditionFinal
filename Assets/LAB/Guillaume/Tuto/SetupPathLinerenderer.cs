using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupPathLinerenderer : MonoBehaviour
{
    public LineRenderer boatPath;
    public Path selfPath;

    void Start()
    {
        boatPath.positionCount = selfPath.waypoints.Count;

        for (int i = 0; i < selfPath.waypoints.Count; i++)
        {
            boatPath.SetPosition(i, selfPath.waypoints[i].self.position);
        }
    }
}
