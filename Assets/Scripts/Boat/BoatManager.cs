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
        if (instance == null)
            instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Play Boat Sound
        AudioManager.AMInstance.boatMovingSFX.Post(gameObject);
    }
}
