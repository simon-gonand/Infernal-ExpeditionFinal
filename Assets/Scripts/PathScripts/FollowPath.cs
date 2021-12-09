using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    public Path path;
    public Transform self;
    public Transform camPosition;

    private float moveAmount = 0.0f;
    private int linkIndex = 0;

    private Vector3 initialOffset;
    private Vector3 initialPos;

    // Start is called before the first frame update
    void Start()
    {
        if (path.allPoints.Count > 0)
            self.position = path.allPoints[0];

        // Get the offset between the camera and the boat
        initialOffset = camPosition.position - self.position;
        initialPos = camPosition.position;
    }

    private Vector3 CalculatePositionOnBeziers(Vector3 a, Vector3 b, Vector3 startAnchor, Vector3 endAnchor, float t)
    {
        Vector3 A = Vector3.Lerp(a, startAnchor, t);
        Vector3 B = Vector3.Lerp(startAnchor, endAnchor, t);
        Vector3 C = Vector3.Lerp(endAnchor, b, t);

        Vector3 AB = Vector3.Lerp(A, B, t);
        Vector3 BC = Vector3.Lerp(B, C, t);

        transform.forward = BC - AB;

        return Vector3.Lerp(AB, BC, t);
    }

    private void CameraFollow(float moveAmount)
    {
        // Update the position of the camera according to the boat on Z
        camPosition.position = new Vector3(self.position.x + initialOffset.x, initialPos.y,
            self.position.z + initialOffset.z);
        Vector3 camOffset = camPosition.position;
        camOffset.x += path.links[linkIndex].XCameraOffset.Evaluate(moveAmount);
        camOffset.z += path.links[linkIndex].YCameraOffset.Evaluate(moveAmount);
        camPosition.position = camOffset;
    }

    // Update is called once per frame
    void Update()
    {
        if (linkIndex >= path.links.Count) return;
        moveAmount = (moveAmount + (Time.deltaTime * path.links[linkIndex].speed)) % 1.0f;
        if (self.position - path.allPoints[0] == Vector3.zero) moveAmount += 0.1f;
        
        float fullMoveAmount = moveAmount * path.allPoints.Count - 1;
        int indexPoint = Mathf.FloorToInt(fullMoveAmount);
        if (indexPoint < 0) indexPoint = 0;
        float moveAmountPoint = fullMoveAmount - indexPoint;
        if (path.allPoints.Count < 2) return;
        Vector3 nextPoint;
        if (indexPoint == 0 && indexPoint < path.allPoints.Count - 1)
        {
            nextPoint = path.allPoints[indexPoint + 1];
        }
        else
        {
            nextPoint = path.allPoints[indexPoint + 1];
        }

        self.position = CalculatePositionOnBeziers(path.allPoints[indexPoint], nextPoint, path.allAnchors[indexPoint * 2], path.allAnchors[indexPoint * 2 + 1], moveAmountPoint);
        CameraFollow(moveAmount);
        if (moveAmount > 0.98f * (((float)linkIndex + 1) / ((float)path.links.Count)))
        { 
            ++linkIndex;
        }
        
        // Play boat movement sound
    }
}
