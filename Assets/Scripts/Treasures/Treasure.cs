using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour, ICarriable
{
    [SerializeField]
    private Transform self;
    public Rigidbody selfRigidbody;
    [SerializeField]
    private Collider selfCollider;
    public TreasuresCategory category;
    [System.NonSerialized]
    public float speedMalus = 0.0f;

    private List<PlayerController> _playerInteractingWith = new List<PlayerController>();
    public List<PlayerController> playerInteractingWith { get { return _playerInteractingWith; } }

    private List<PlayerController> playerColliding = new List<PlayerController>();

    private Dictionary<PlayerController, GameObject> associateColliders = new Dictionary<PlayerController, GameObject>();
    private bool isGrounded = false;
    private bool isLoadingLaunch = false;
    private bool _isColliding = false;
    public bool isColliding { set { _isColliding = value; } }

    private float launchForce = 0.0f;
    private Vector3 lastPosition;

    private Vector3 startPlayerPosition;
    private Quaternion startPlayerRotation;
    private Matrix4x4 playerMatrix;
    private Vector3 startSelfPosition;
    private Quaternion startSelfRotation;

    private Vector3 collisionDirection;
    private Rigidbody collidingWith;
    private bool isMovingWhenColliding;

    private bool _isInDeepWater = false;
    public bool isInDeepWater { set { _isInDeepWater = value; } }

    #region CollisionCallbacks
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            foreach (PlayerController player in _playerInteractingWith)
            {
                if (collision.collider == player.GetComponent<CapsuleCollider>())
                {
                    _isColliding = false;
                    return;
                }
            }
            playerColliding.Add(collision.collider.GetComponent<PlayerController>());
        }
        collisionDirection = collision.GetContact(0).normal;
        _isColliding = true;
        collidingWith = collision.collider.GetComponent<Rigidbody>();
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collidingWith != null && collidingWith.velocity != Vector3.zero && isMovingWhenColliding)
        {
            collisionDirection = -collisionDirection;
            isMovingWhenColliding = false;
        }
        else if (!isMovingWhenColliding)
        {
            collisionDirection = -collisionDirection;
            isMovingWhenColliding = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            foreach (PlayerController player in _playerInteractingWith)
            {
                if (collision.collider == player.GetComponent<CapsuleCollider>())
                    playerColliding.Remove(player);
            }
        }
        _isColliding = false;
    }
    #endregion

    private void Start()
    {
        lastPosition = self.position;
    }

    public void UpdatePlayerRotation(PlayerController player, Transform playerTransform)
    {
        if (_playerInteractingWith.Count == 1)
        {
            if (!_isColliding)
            {
                playerMatrix = Matrix4x4.TRS(playerTransform.position, playerTransform.rotation, playerTransform.lossyScale);

                self.position = playerMatrix.MultiplyPoint3x4(startSelfPosition);
                self.rotation = (playerTransform.rotation * Quaternion.Inverse(startPlayerRotation)) * startSelfRotation;
            }
        }
        else
            if (associateColliders[player] != null)
                playerTransform.forward = associateColliders[player].transform.forward;
    }

    public void UpdatePlayerMovement(PlayerController player)
    {
        if (_playerInteractingWith.Count == 1)
        {
            if (_isColliding)
                player.selfRigidBody.velocity = Vector3.zero;
            return;
        }
        if (associateColliders[player] != null)
        {
            Vector3 newPlayerPos = associateColliders[player].transform.position;
            newPlayerPos.y = player.self.position.y;
            player.self.position = newPlayerPos;
        }
    }

    // Get the treasure and all the players that are carrying it on boat
    public void GetOnBoat(Transform entryPosition)
    {
        Vector3 treasureOnBoat = entryPosition.position;
        treasureOnBoat.y += self.lossyScale.y / 2;
        self.position = treasureOnBoat;
        self.SetParent(BoatManager.instance.self);
        foreach(PlayerController player in playerInteractingWith)
        {
            player.isOnBoat = false;
            player.self.SetParent(BoatManager.instance.self);
        }
    }

    // Remove boat parent from treasure and all players that are carrying it
    public void GetOffBoat()
    {
        foreach (PlayerController player in playerInteractingWith)
        {
            player.isOnBoat = true;
            player.self.SetParent(null);
        }
    }

    // Disable collider on the side where the player is interacting with the treasure
    private void DealWithCollider(PlayerController player, GameObject interactingWith)
    {
        interactingWith.GetComponent<BoxCollider>().enabled = false;

        // Snap the player to the center of the side of the treasure
        //interactingWith.GetComponent<GetSnappingPosition>().SnapPlayerToPosition(player);
        associateColliders.Add(player, interactingWith);
    }

    private void UpTreasure(PlayerController player)
    {
        // Snap treasure to the player
        Vector3 upTreasure = self.position;
        upTreasure.y = player.self.position.y + self.lossyScale.y / 2;
        self.position = upTreasure;
    }

    private Vector3 DivideVectors(Vector3 num, Vector3 den)
    {
        return new Vector3(num.x / den.x, num.y / den.y, num.z / den.z);
    }

    #region interaction
    // When player is interacting with the treasure
    public bool InteractWith(PlayerController player, GameObject interactingWith)
    {
        // Update player values
        _playerInteractingWith.Add(player);
        player.isCarrying = true;

        player.anim.SetBool("isCarrying", true);
        player.anim.SetTrigger("startCarrying");
        player.sword.SetActive(false);

        player.carrying = this;

        if (playerColliding.Count > 0)
        {
            foreach (PlayerController p in playerColliding)
            {
                if (p == player)
                {
                    playerColliding.Remove(p);
                    _isColliding = false;
                    break;
                }
            }
        }

        selfRigidbody.useGravity = false;

        // Update speed malus
        ApplySpeedMalus();

        // If the player is alone to carry it just snap the treasure as child of the player
        if (_playerInteractingWith.Count == 1)
        {
            Physics.IgnoreCollision(selfCollider, BoatManager.instance.selfCollider, true);
            UpTreasure(player);
            selfRigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
            selfRigidbody.useGravity = false;

            startPlayerPosition = player.self.position;
            startPlayerRotation = player.self.rotation;

            startSelfPosition = self.position;
            startSelfRotation = self.rotation;

            startSelfPosition = DivideVectors(Quaternion.Inverse(player.self.rotation) * (startSelfPosition - startPlayerPosition), player.self.lossyScale);
        }
        // If there is more than one player to carry it, snap treasures to the players' joint
        if (_playerInteractingWith.Count <= category.maxPlayerCarrying)
        {
            DealWithCollider(player, interactingWith);

            selfRigidbody.velocity = Vector3.zero;

            // Do not apply the velocity on the first frame
            for (int i = 0; i < _playerInteractingWith.Count; ++i)
            {
                _playerInteractingWith[i].selfRigidBody.velocity = Vector3.zero;
            }
            return true;
        }

        // If the player cannot carry the treasure due to the number of players already carrying it
        player.isCarrying = false;

        player.anim.SetBool("isCarrying", false);
        player.sword.SetActive(true);

        player.carrying = null;
        return false;
    }

    // When the player pressed the action button when he's on the treasure
    // Launch the treasure
    public void OnAction(PlayerController player)
    {
        if (_playerInteractingWith.Count == 1)
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

    public void Launch(PlayerController player)
    {
        if (isLoadingLaunch)
        {
            isLoadingLaunch = false;

            // Enable rigidbody
            selfRigidbody.isKinematic = false;
            selfRigidbody.useGravity = true;
            Physics.IgnoreCollision(selfCollider, BoatManager.instance.selfCollider, false);
            selfRigidbody.AddForce((player.self.forward + player.self.up) * launchForce, ForceMode.Impulse);
            launchForce = 0.0f;

            // Update lists values
            _playerInteractingWith.Remove(player);
            associateColliders[player].GetComponent<BoxCollider>().enabled = true;
            associateColliders.Remove(player);

            // Update player values
            player.isCarrying = false;

            player.anim.SetBool("isCarrying", false);
            player.sword.SetActive(true);

            player.carrying = null;
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

        player.carrying = null;

        // Player does not interact with the treasure anymore
        _playerInteractingWith.Remove(player);

        associateColliders[player].GetComponent<BoxCollider>().enabled = true;
        associateColliders.Remove(player);

        player.playerGraphics.forward = player.self.forward;

        // Update speed malus
        //ApplySpeedMalus();
        if (_playerInteractingWith.Count == 1)
        {
            startPlayerPosition = player.self.position;
            startPlayerRotation = player.self.rotation;

            startSelfPosition = self.position;
            startSelfRotation = self.rotation;

            startSelfPosition = DivideVectors(Quaternion.Inverse(player.self.rotation) * (startSelfPosition - startPlayerPosition), player.self.lossyScale);
        }
        if (_playerInteractingWith.Count < 1)
        {
            Physics.IgnoreCollision(selfCollider, BoatManager.instance.selfCollider, false);

            // Enable rigidbody
            selfRigidbody.useGravity = true;
            selfRigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            isGrounded = false;
        }
    }

    #endregion

    private void ApplySpeedMalus()
    {
        /*// Deal with speed according to the number of player carrying the treasure
        if (_playerInteractingWith.Count == category.maxPlayerCarrying)
            speedMalus = 0;
        else
            speedMalus = category.speedMalus / _playerInteractingWith.Count;*/
        speedMalus = category.speedMalus;
    }

    private void TreasureMovement()
    {
        if (_playerInteractingWith.Count > 0)
        {
            selfRigidbody.velocity = Vector3.zero;
            foreach(PlayerController player in _playerInteractingWith)
            {
                Vector3 applyForces = player.movement;
                applyForces.y = 0.0f;
                selfRigidbody.velocity += applyForces;
            }
        }
    }

    private void FixedUpdate()
    {
        TreasureMovement();

        // Destroy object if is completely in water
        if (_isInDeepWater)
        {
            float topTreasureY = self.position.y + self.lossyScale.y / 2;
            if (topTreasureY < NotDeepWater.instance.self.position.y)
                Destroy(gameObject);
        }
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
                if (!hit.collider.isTrigger)
                {
                    isGrounded = true;
                }
            }
            
        }
        if (_isColliding)
        {
            if (Vector3.Dot(selfRigidbody.velocity, -collisionDirection) < 1 && selfRigidbody.velocity != Vector3.zero)
            {
                _isColliding = false;
            }
            else
            {

                foreach (PlayerController player in _playerInteractingWith)
                {
                    if (_playerInteractingWith.Count > 1)
                        associateColliders[player].GetComponent<GetSnappingPosition>().SnapPlayerToPosition(player);
                }
                self.position = lastPosition;
            }
        }
        else
            lastPosition = self.position;
    }
}
