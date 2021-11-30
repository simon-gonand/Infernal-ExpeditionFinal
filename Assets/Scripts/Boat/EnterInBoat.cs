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
                if (player.isCarrying)
                {
                    if (player.transportedTreasure != null && player.transportedTreasure.playerInteractingWith.Count > 1)
                        player.transportedTreasure.GetOnBoat(playerOnBoatEntryPoint);
                    else
                    {
                        if (player.transportedTreasure == null)
                        {
                            CarryPlayer carriedPlayer = player.interactingWith as CarryPlayer;
                            carriedPlayer.GetOnBoat();
                        }
                        player.self.SetParent(BoatManager.instance.self);
                        player.isOnBoat = true;
                    }
                }
                // Player carries a treasure solo or does not carry anything
                else
                {
                    player.isOnBoat = true;
                    player.selfRigidBody.velocity += Vector3.up;
                    player.UpdateSwimming();
                    player.self.position = playerOnBoatEntryPoint.position;
                    player.self.SetParent(BoatManager.instance.self);
                }
            }
            // Let the player getting out the boat
            else
            {
                // Player is carrying a treasure with someone
                if (player.isCarrying)
                {
                    if (player.transportedTreasure != null && player.transportedTreasure.playerInteractingWith.Count > 1)
                        player.transportedTreasure.GetOffBoat();
                    else
                    {
                        player.self.SetParent(null);
                        player.isOnBoat = false;
                        if (player.transportedTreasure == null)
                        {
                            CarryPlayer carriedPlayer = player.interactingWith as CarryPlayer;
                            carriedPlayer.GetOffBoat();
                        }
                    }
                }
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
