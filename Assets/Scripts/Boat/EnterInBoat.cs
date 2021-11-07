using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class EnterInBoat : MonoBehaviour
{
    [SerializeField]
    private Transform playerOnBoatEntryPoint;

    private bool isPlayerClimbingInBoat = false;
    private PlayerController player;

    // When the player is entering in the zone, it climbs on the boat
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isPlayerClimbingInBoat) {
            player = other.GetComponent<PlayerController>();

            if (!player.isOnBoat)
            {
                player.self.position = playerOnBoatEntryPoint.position;
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
}
