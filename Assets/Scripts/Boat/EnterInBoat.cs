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
                // Player is carrying a treasure with someone
                if (player.isCarrying && player.transportedTreasure.playerInteractingWith.Count > 1)
                {
                    player.transportedTreasure.GetOnBoat(playerOnBoatEntryPoint);
                }
                // Player carries a treasure solo or does not carry anything
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
                // Player is carrying a treasure with someone
                if (player.isCarrying && player.transportedTreasure.playerInteractingWith.Count > 1)
                    player.transportedTreasure.GetOffBoat();
                // Player carries a treasure solo or does not carry anything
                else
                {
                    player.self.SetParent(null);
                    player.isOnBoat = false;
                }
            }
        }
        if (other.CompareTag("Treasures"))
        {
            Treasure treasure = other.GetComponent<Treasure>();
            if (treasure.playerInteractingWith.Count > 1)
                treasure.GetOnBoat(playerOnBoatEntryPoint);
        }
    }
}
