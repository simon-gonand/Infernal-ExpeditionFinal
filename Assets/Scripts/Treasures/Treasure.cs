using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Transform self;
    [SerializeField]
    private Rigidbody selfRigidbody;
    [SerializeField]
    private FixedJoint fixedJoint;
    public TreasuresCategory category;

    [System.NonSerialized]
    public PlayerController playerInteractingWith;
    private bool isGrounded = false;

    // When player is interacting with the treasure
    public void InteractWith(PlayerController player)
    {
        // Update player values
        playerInteractingWith = player;
        player.isCarrying = true;
        player.treasureCarried = category;

        // Snap treasure to the player
        self.position = player.carryingSnapPoint.position;
        self.forward = player.transform.forward;

        player.fixedJoint.connectedBody = selfRigidbody;

        // Set the player as parent to move with it
        //self.SetParent(player.self);
    }

    // When the player pressed the action button when he's on the treasure
    // Launch the treasure
    public void OnAction()
    {
        // Remove the parent
        self.SetParent(null);

        // Enable rigidbody
        selfRigidbody.isKinematic = false;
        selfRigidbody.AddForce((self.forward + self.up) * category.launchForce, ForceMode.Impulse);

        // Update player values
        playerInteractingWith.isCarrying = false;
        playerInteractingWith.treasureCarried = null;
        playerInteractingWith = null;
        isGrounded = false;
    }

    // When the player is moving when he's on the treasure
    // Nothing on move
    public void OnMove(Vector2 movements)
    {
    }

    // When the player is not interacting with the treasure anymore
    public void UninteractWith(PlayerController player)
    {
        // Update player values
        player.isCarrying = false;
        player.treasureCarried = null;
        playerInteractingWith = null;

        // Remove parent
        self.SetParent(null);

        // Enable rigidbody
        selfRigidbody.isKinematic = false;
        isGrounded = false;
    }

    private void FixedUpdate()
    {
        // Check if the treasure is touching the ground
        if (!isGrounded)
        {
            // Set the position of the raycast
            Vector3 raycastStartPos = self.position;
            raycastStartPos.y -= self.lossyScale.y / 2;
            RaycastHit hit;
            if (Physics.Raycast(raycastStartPos, -Vector3.up, out hit, 0.05f))
            {
                if (hit.collider.CompareTag("Boat"))
                {
                    self.SetParent(hit.collider.transform);
                }
                // Disable rigidbody
                selfRigidbody.isKinematic = true;
                isGrounded = true;
            }
            
        }
    }
}
