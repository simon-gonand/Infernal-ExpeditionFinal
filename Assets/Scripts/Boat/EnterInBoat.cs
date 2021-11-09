using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnterInBoat : MonoBehaviour
{
    [SerializeField]
    private Transform playerOnBoatEntryPoint;

    private PlayerController player;

    // When the player is entering in the zone, it climbs on the boat
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            player = other.GetComponent<PlayerController>();
            if (!player.isOnBoat)
            {
                player.isClimbingOnBoat = true;
                player.self.SetParent(BoatManager.instance.self);
            }
            // Let the player getting out the boat
            else
            {
                player.self.SetParent(null);
            }

            // Update if the player is on the boat or not
            player.isOnBoat = !player.isOnBoat;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerController>();
            player.isClimbingOnBoat = false;
        }
    }
}
