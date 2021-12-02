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
            if (!player.isOnBoat && !player.hasBeenLaunched)
            {
                // Player is carrying a treasure with someone
                if (player.isCarrying)
                {
                    Treasure transportedTreasure = player.carrying as Treasure;
                    if (transportedTreasure != null && transportedTreasure.playerInteractingWith.Count > 1)
                        transportedTreasure.GetOnBoat(playerOnBoatEntryPoint);
                    else
                    {
                        if (transportedTreasure == null)
                        {
                            CarryPlayer carriedPlayer = player.interactingWith as CarryPlayer;
                            carriedPlayer.GetOnBoat(null);
                        }
                    }
                }
                // Player carries a treasure solo or does not carry anything
                else
                {
                    player.selfRigidBody.velocity += Vector3.up;
                    player.UpdateSwimming();
                    player.self.position = playerOnBoatEntryPoint.position;
                }
            }
            // Let the player getting out the boat
            else
            {
                // Player is carrying a treasure with someone
                if (player.isCarrying)
                {
                    Treasure transportedTreasure = player.carrying as Treasure;
                    if (transportedTreasure != null && transportedTreasure.playerInteractingWith.Count > 1)
                        transportedTreasure.GetOffBoat();
                    else
                    {
                        if (transportedTreasure == null)
                        {
                            CarryPlayer carriedPlayer = player.interactingWith as CarryPlayer;
                            carriedPlayer.GetOffBoat();
                        }
                    }
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
