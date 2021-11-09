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

            // Rotate player as he his swimming
            Vector3 swimRotation = player.playerGraphics.eulerAngles;
            swimRotation.x = 75.0f;
            player.playerGraphics.eulerAngles = swimRotation;

            // Remove gravity to avoid to fall inside the water
            player.selfRigidBody.useGravity = false;
            player.selfRigidBody.velocity = Vector3.zero;

            if (player.isCarrying)
            {
                player.transportedTreasure.UninteractWith(player);
            }
        }
        else if (other.CompareTag("Treasure"))
        {
            other.GetComponent<Treasure>().isInDeepWater = true;
        }
    }
}
