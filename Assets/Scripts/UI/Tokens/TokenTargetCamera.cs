using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TokenTargetCamera : MonoBehaviour
{
    public Transform cam;

    

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(cam);
    }
}
