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
    private Dictionary<PlayerController, GameObject> associateColliders = new Dictionary<PlayerController, GameObject>();
    private bool isGrounded = false;
    private bool isLoadingLaunch = false;
    private float launchForce = 0.0f;

    private bool _isInDeepWater = false;
    public bool isInDeepWater { set { _isInDeepWater = value; } }

    // Disable collider on the side where the player is interacting with the treasure
    private void DealWithCollider(PlayerController player, GameObject interactingWith)
    {
        interactingWith.GetComponent<BoxCollider>().enabled = false;

        // Snap the player to the center of the side of the treasure
        interactingWith.GetComponent<GetSnappingPosition>().SnapPlayerToPosition(player);
        associateColliders.Add(player, interactingWith);
    }

    private void UpTreasure(PlayerController player)
    {
        // Snap treasure to the player
        Vector3 upTreasure = self.position;
        upTreasure.y = player.self.position.y + self.lossyScale.y / 2;
        self.position = upTreasure;
    }

    // When player is interacting with the treasure
    public bool InteractWith(PlayerController player, GameObject interactingWith)
    {
        // Update player values
        playerInteractingWith.Add(player);
        player.isCarrying = true;

        player.anim.SetBool("isCarrying", true);
        player.anim.SetTrigger("startCarrying");
        player.sword.SetActive(false);

        player.transportedTreasure = this;

        // Update speed malus
        ApplySpeedMalus();

        // If the player is alone to carry it just snap the treasure as child of the player
        if (playerInteractingWith.Count == 1)
        {
            DealWithCollider(player, interactingWith);
            UpTreasure(player);
            self.SetParent(player.self);
            return true;
        }
        // If there is more than one player to carry it, treasures to the players' joint
        else if (playerInteractingWith.Count > 1 && playerInteractingWith.Count <= category.maxPlayerCarrying)
        {
            self.SetParent(null);
            selfRigidbody.isKinematic = false;
            DealWithCollider(player, interactingWith);

            // Snap to joints
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

        // If the player cannot carry the treasure due to the number of players already carrying it
        player.isCarrying = false;

        player.anim.SetBool("isCarrying", false);
        player.sword.SetActive(true);

        player.transportedTreasure = null;
        return false;
    }

    // When the player pressed the action button when he's on the treasure
    // Launch the treasure
    public void OnAction(PlayerController player)
    {
        if (playerInteractingWith.Count == 1)
        {
            isLoadingLaunch = true;
            StartCoroutine(LoadingLaunchForce());
        }
    }

    IEnumerator LoadingLaunchForce()
    {
        // Increase every 0.1 seconds
        float offsetTime = 0.1f;
        // Calculate how many the launch force will increase every 0.1 seconds
        float offsetLaunch = category.maxLaunchForce * offsetTime / category.fullChargeTime;
        while (isLoadingLaunch && launchForce != category.maxLaunchForce)
        {
            launchForce += offsetLaunch;
            if (launchForce > category.maxLaunchForce)
                launchForce = category.maxLaunchForce;
            Debug.Log(launchForce);
            yield return new WaitForSeconds(offsetTime);
        }
    }

    public void LaunchObject(PlayerController player)
    {
        if (isLoadingLaunch)
        {
            isLoadingLaunch = false;
            // Remove the parent
            self.SetParent(null);

            // Enable rigidbody
            selfRigidbody.isKinematic = false;
            selfRigidbody.AddForce((player.self.forward + player.self.up) * launchForce, ForceMode.Impulse);
            launchForce = 0.0f;

            // Update lists values
            playerInteractingWith.Remove(player);
            associateColliders[player].GetComponent<BoxCollider>().enabled = true;
            associateColliders.Remove(player);

            // Update player values
            player.isCarrying = false;

            player.anim.SetBool("isCarrying", false);
            player.sword.SetActive(true);

            player.transportedTreasure = null;
            isGrounded = false;
        }
    }

    // When the player is not interacting with the treasure anymore
    public void UninteractWith(PlayerController player)
    {
        // Update player values
        player.isCarrying = false;

        player.anim.SetBool("isCarrying", false);
        player.sword.SetActive(true);

        player.transportedTreasure = null;

        // Player does not interact with the treasure anymore
        playerInteractingWith.Remove(player);

        associateColliders[player].GetComponent<BoxCollider>().enabled = true;
        associateColliders.Remove(player);

        // Remove player joints
        if (playerInteractingWith.Count >= 1)
        {
            Destroy(associateJoints[player]);
            associateJoints.Remove(player);
        }
        // If the player is alone to carry the treasure, remove joints and set the player as parent of the treasure
        if (playerInteractingWith.Count == 1)
        {
            Destroy(associateJoints[playerInteractingWith[0]]);
            associateJoints.Remove(playerInteractingWith[0]);
            self.SetParent(playerInteractingWith[0].self);
            UpTreasure(playerInteractingWith[0]);
            selfRigidbody.isKinematic = true;
        }

        // Update speed malus
        ApplySpeedMalus();

        if (playerInteractingWith.Count < 1)
        {
            // Remove parent
            self.SetParent(null);

            // Enable rigidbody
            selfRigidbody.isKinematic = false;
            isGrounded = false;
        }
    }

    private void ApplySpeedMalus()
    {
        // Deal with speed according to the number of player carrying the treasure
        if (playerInteractingWith.Count == category.maxPlayerCarrying)
            speedMalus = 0;
        else
            speedMalus = category.speedMalus / playerInteractingWith.Count;
    }

    private void FixedUpdate()
    {
        // Destroy object if is completely in water
        if (_isInDeepWater)
        {
            float topTreasureY = self.position.y + self.lossyScale.y / 2;
            if (topTreasureY < NotDeepWater.instance.self.position.y)
                Destroy(gameObject);
        }

        // Check if the treasure is touching the ground
        if (!isGrounded)
        {
            // Set the position of the raycast
            Vector3 raycastStartPos = self.position;
            raycastStartPos.y -= self.lossyScale.y / 2;
            RaycastHit hit;
            if (Physics.Raycast(raycastStartPos, -Vector3.up, out hit, 0.05f))
            {
                // Set boat as parent if it's touching the ground of it
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
