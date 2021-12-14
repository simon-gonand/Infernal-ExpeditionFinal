using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnBoatZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if(!player.isCarried)
                player.self.SetParent(BoatManager.instance.self);
            player.isOnBoat = true;
            // Player is carrying a treasure with someone
            if (player.isCarrying)
            {
                Treasure transportedTreasure = player.carrying as Treasure;
                if (transportedTreasure != null)
                {
                    transportedTreasure.self.SetParent(BoatManager.instance.self);
                }                
            }
        }
        if (other.CompareTag("Treasures"))
        {
            Treasure treasure = other.GetComponent<Treasure>();
            treasure.self.SetParent(BoatManager.instance.self);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (!player.isCarried)
                player.self.SetParent(null);
            player.isOnBoat = false;

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
        if (other.CompareTag("Treasures"))
        {
            Treasure treasure = other.GetComponent<Treasure>();
            if (treasure.playerInteractingWith.Count == 0)
                treasure.GetOffBoat();
        }
    }
}
