using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatManager : MonoBehaviour
{
    public Transform self;
    public Collider selfCollider;
    public Transform spawnPoint;

    // BoatManager is a singleton class
    public static BoatManager instance;

    private void Awake()
    {
        instance = this;
    }
}
