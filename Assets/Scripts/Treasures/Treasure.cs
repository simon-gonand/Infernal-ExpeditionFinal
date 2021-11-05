using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour, IInteractable
{
    [SerializeField]
    private Transform self;
    [SerializeField]
    private Rigidbody selfRigidbody;
    public TreasuresCategory category;
    [System.NonSerialized]
    public float speedMalus = 0.0f;
    
    private List<PlayerController> playerInteractingWith = new List<PlayerController>();
    private Dictionary<PlayerController, FixedJoint> associateJoints = new Dictionary<PlayerController, FixedJoint>();
    private bool isGrounded = false;

    // When player is interacting with the treasure
    public bool InteractWith(PlayerController player)
    {
        // Update player values
        playerInteractingWith.Add(player);
        player.isCarrying = true;
        player.transportedTreasure = this;

        if (playerInteractingWith.Count == 1)
        {
            // Snap treasure to the player
            self.position = player.carryingSnapPoint.position;
            self.forward = player.transform.forward;
            self.SetParent(player.self);
            speedMalus = category.speedMalus;
            return true;
        }
        else if (playerInteractingWith.Count > 1 && playerInteractingWith.Count <= category.maxPlayerCarrying)
        {
            self.SetParent(null);
            selfRigidbody.isKinematic = false;

            if (playerInteractingWith.Count == category.maxPlayerCarrying)
                speedMalus = 0;
            else
                speedMalus = category.speedMalus / playerInteractingWith.Count;

            for (int i = 0; i < playerInteractingWith.Count; ++i)
            {
                if (!associateJoints.ContainsKey(playerInteractingWith[i])){
                    FixedJoint joint = playerInteractingWith[i].gameObject.AddComponent<FixedJoint>();
                    joint.connectedBody = selfRigidbody;
                    associateJoints.Add(playerInteractingWith[i], joint);
                }
            }
            return true;
        }

        player.isCarrying = false;
        player.transportedTreasure = null;
        return false;
    }

    // When the player pressed the action button when he's on the treasure
    // Launch the treasure
    public void OnAction(PlayerController player)
    {
        // Remove the parent
        self.SetParent(null);

        // Enable rigidbody
        selfRigidbody.isKinematic = false;
        selfRigidbody.AddForce((self.forward + self.up) * category.launchForce, ForceMode.Impulse);

        // Update player values
        player.isCarrying = false;
        player.transportedTreasure = null;
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
        player.transportedTreasure = null;
        playerInteractingWith.Remove(player);
        if (playerInteractingWith.Count >= 1)
        {
            Destroy(associateJoints[player]);
            associateJoints.Remove(player);
        }
        if (playerInteractingWith.Count == 1)
        {
            Destroy(associateJoints[playerInteractingWith[0]]);
            associateJoints.Remove(playerInteractingWith[0]);
            self.SetParent(playerInteractingWith[0].self);
            self.position = playerInteractingWith[0].carryingSnapPoint.position;
            self.forward = playerInteractingWith[0].self.forward;
            selfRigidbody.isKinematic = true;
        }
        if (playerInteractingWith.Count < 1)
        {
            // Remove parent
            self.SetParent(null);

            // Enable rigidbody
            selfRigidbody.isKinematic = false;
            isGrounded = false;
        }
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
