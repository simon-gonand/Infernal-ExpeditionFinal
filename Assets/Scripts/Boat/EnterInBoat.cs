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

            // Let the player going out the boat
            if (!player.isOnBoat)
            {
                player.self.position = playerOnBoatEntryPoint.position;
            }
            else
            {
                player.isGrounded = false;
                // Update rigidbody constraints to apply gravity
                other.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                player.self.SetParent(null);
            }

            // Update if the player is on the boat or not
            player.isOnBoat = !player.isOnBoat;
        }
    }
}
