using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetupPathLinerenderer : MonoBehaviour
{
    public LineRenderer boatPath;
    public Path selfPath;

    void Start()
    {
        SetupLineRendererPos();
    }

    [ContextMenu ("Setup Line")]
    public void SetupLineRendererPos()
    {
        boatPath.positionCount = selfPath.waypoints.Count;

        for (int i = 0; i < selfPath.waypoints.Count; i++)
        {
            boatPath.SetPosition(i, selfPath.waypoints[i].self.position + new Vector3(0,0.1f,0));
        }
    }
}
