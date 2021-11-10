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
                if (player.isCarrying && player.transportedTreasure.playerInteractingWith.Count > 1)
                {
                    player.transportedTreasure.GetOnBoat(playerOnBoatEntryPoint);
                }
                else
                {
                    player.self.position = playerOnBoatEntryPoint.position;
                    player.self.SetParent(BoatManager.instance.self);
                    player.isOnBoat = true;
                }
            }
            // Let the player getting out the boat
            else
            {
                player.self.SetParent(null);
                player.isOnBoat = false;
            }
        }
    }
}
