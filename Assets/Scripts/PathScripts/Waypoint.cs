using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Waypoint : MonoBehaviour
{
    public Transform self;
    public UnityEvent NPCEvent;
    public UnityEvent modifierEvent;

    // Start is called before the first frame update
    void Awake()
    {
        self = transform;
    }

    public void SetSelfTransform()
    {
        self = transform;
    }

}
