using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NotDeepWater : MonoBehaviour
{
    public Transform self;

    // WaterScript is a singleton class
    public static NotDeepWater instance;

    private void Awake()
    {
        instance = this;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            player.isInWater = true;
            if (player.hasBeenLaunched)
                player.hasBeenLaunched = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerController>().isInWater = false;
        }
    }
}
