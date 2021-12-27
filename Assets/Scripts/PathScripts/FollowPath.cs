using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FollowPath : MonoBehaviour
{
    [HideInInspector] public Path path;
    public Transform self;
    [HideInInspector] public CinemachineVirtualCamera cam;

    private float initialPosY;
    private Vector3 initialOffset;
    private float initialFOV;

    private int linkIndex = 0;
    private float lastTValue = 0.0f;

    private float tParam = 0.0f;
    private bool coroutineAllowed = true;
    int allPointIndex = 0;
    private bool pathEnd = false;

    private Waypoint currentWaypoint;

    // Start is called before the first frame update
    void Start()
    {
        InitializePath();
    }

    public void InitializePath()
    {
        if (path == null) return;
        if (path.allPoints.Count < 2)
        {
            pathEnd = true;
            return;
        }
        currentWaypoint = path.waypoints[0];
        currentWaypoint.ev.Invoke();

        initialPosY = self.position.y;
        if (cam != null)
        {
            initialOffset = cam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
            initialFOV = cam.m_Lens.FieldOfView;
        }
    }

    private IEnumerator FollowCurve()
    {
        coroutineAllowed = false;

        while (tParam < 1)
        {
            // Object position
            tParam += Time.deltaTime * path.links[linkIndex].speed;
            Vector3 posOnCurve = Mathf.Pow(1 - tParam, 3) * path.allPoints[allPointIndex] +
                3 * Mathf.Pow(1 - tParam, 2) * tParam * path.allAnchors[allPointIndex * 2] +
                3 * (1 - tParam) * Mathf.Pow(tParam, 2) * path.allAnchors[allPointIndex * 2 + 1] +
                Mathf.Pow(tParam, 3) * path.allPoints[allPointIndex + 1];
            posOnCurve.y = initialPosY;
            Vector3 oldPos = self.position;
            self.position = posOnCurve;

            Vector3 rotation = self.position - oldPos;
            rotation.y = 0.0f;
            self.rotation = Quaternion.LookRotation(rotation);
            

            // Camera position
            if (cam != null)
            {
                float t = tParam / (path.links[linkIndex].pathPoints.Count - 1);
                float xOffset = path.links[linkIndex].XCameraOffset.Evaluate(t + lastTValue);
                float zOffset = path.links[linkIndex].YCameraOffset.Evaluate(t + lastTValue);
                cam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = initialOffset + new Vector3(xOffset, 0.0f, zOffset);
                cam.m_Lens.FieldOfView = initialFOV + path.links[linkIndex].UnzoomCameraOffset.Evaluate(t + lastTValue);
            }

            yield return new WaitForEndOfFrame();
        }
        if (path.links[linkIndex].pathPoints.Count > 1)
            lastTValue = tParam / (path.links[linkIndex].pathPoints.Count - 1);
        tParam = 0.0f;
        ++allPointIndex;
        if (allPointIndex == path.allPoints.Count - 1)
        {
            if (path.loop)
            {
                allPointIndex = 0;
                linkIndex = 0;
                lastTValue = 0.0f;
            }
            else
                pathEnd = true;
        }
        else if (linkIndex < path.links.Count - 1 && path.allPoints[allPointIndex] == path.links[linkIndex + 1].start.self.position)
        {
            ++linkIndex;
            lastTValue = 0.0f;
            currentWaypoint = path.waypoints[linkIndex];
            currentWaypoint.ev.Invoke();
        }

        coroutineAllowed = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (pathEnd || path == null) return;
        if (coroutineAllowed)
            StartCoroutine(FollowCurve());

        // Play boat movement sound
    }
}
