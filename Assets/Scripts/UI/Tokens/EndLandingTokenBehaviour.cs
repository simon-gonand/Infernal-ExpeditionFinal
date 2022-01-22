using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLandingTokenBehaviour : MonoBehaviour
{
    [SerializeField]
    private Transform self;

    private void Update()
    {
        self.LookAt(Camera.main.transform);
        var distance = (Camera.main.transform.position - transform.position).magnitude;
        var size = distance * 0.0005f * Camera.main.fieldOfView;
        transform.localScale = Vector3.one * size;
        transform.forward = transform.position - Camera.main.transform.position;
    }
}
