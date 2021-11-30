using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeepWater : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // When the player touch the collider then it means that he is swimming
            PlayerController player = other.GetComponent<PlayerController>();

            player.isSwimming = true;

            // Remove gravity to avoid to fall inside the water
            player.selfRigidBody.useGravity = false;
            player.selfRigidBody.velocity = Vector3.zero;

            // Drop treasure
            if (player.isCarrying)
            {
                if (player.transportedTreasure != null)
                    player.transportedTreasure.UninteractWith(player);
                else
                    player.interactingWith.UninteractWith(player);
            }

            // Enemies stop attacking him
            while (player.isAttackedBy.Count > 0)
            {
                player.isAttackedBy[0].ResetCurrentFollowedPlayer();
                player.isAttackedBy.Remove(player.isAttackedBy[0]);              
            }
        }
        else if (other.CompareTag("Treasures"))
        {
            other.GetComponent<Treasure>().isInDeepWater = true;
        }
    }
}
