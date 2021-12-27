using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoatManager : MonoBehaviour
{
    public Transform self;
    public Collider selfCollider;
    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public Transform spawnPoint3;
    public Transform spawnPoint4;

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
