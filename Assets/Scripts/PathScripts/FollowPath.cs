using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FollowPath : MonoBehaviour
{
    public Path path;
    public Transform self;
    public CinemachineVirtualCamera cam;

    private float initialPosY;
    private Vector3 initialOffset;
    private float initialFOV;

    private int linkIndex = 0;

    private float tParam = 0.0f;
    private bool coroutineAllowed = true;
    int allPointIndex = 0;
    private bool pathEnd = false;

    private Waypoint currentWaypoint;

    // Start is called before the first frame update
    void Start()
    {
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
                float xOffset = path.links[linkIndex].XCameraOffset.Evaluate(tParam);
                float zOffset = path.links[linkIndex].YCameraOffset.Evaluate(tParam);
                cam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = initialOffset + new Vector3(xOffset, 0.0f, zOffset);
                cam.m_Lens.FieldOfView = initialFOV + path.links[linkIndex].UnzoomCameraOffset.Evaluate(tParam);
            }

            yield return new WaitForEndOfFrame();
        }

        tParam = 0.0f;
        ++allPointIndex;
        if (allPointIndex == path.allPoints.Count - 1)
        {
            if (path.loop)
            {
                allPointIndex = 0;
                linkIndex = 0;
            }
            else
                pathEnd = true;
        }
        else if (linkIndex < path.links.Count - 1 && path.allPoints[allPointIndex] == path.links[linkIndex + 1].start.self.position)
        {
            ++linkIndex;
            currentWaypoint = path.waypoints[linkIndex];
            currentWaypoint.ev.Invoke();
        }

        coroutineAllowed = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (pathEnd) return;
        if (coroutineAllowed)
            StartCoroutine(FollowCurve());

        // Play boat movement sound
    }
}
