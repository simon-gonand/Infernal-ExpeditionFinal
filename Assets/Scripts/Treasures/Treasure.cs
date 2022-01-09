using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour, ICarriable
{
    [Header("Treasure price")]
    public int price;

    public Transform self;
    public Rigidbody selfRigidbody;
    public Collider selfCollider;
    public TreasuresCategory category;
    [System.NonSerialized]
    public float speedMalus = 0.0f;

    private List<PlayerController> _playerInteractingWith = new List<PlayerController>();
    public List<PlayerController> playerInteractingWith { get { return _playerInteractingWith; } }

    private List<PlayerController> playerColliding = new List<PlayerController>();
    private List<PlayerController> playerCollisionIgnored = new List<PlayerController>();

    private Dictionary<PlayerController, GameObject> associateColliders = new Dictionary<PlayerController, GameObject>();
    private bool isGrounded = false;

    [HideInInspector]public Vector3 playerThrowDir;
    private Vector3 globalDir;

    private int numOfSelected;
    public Outline outlineScript;

    private Vector3 lastPosition;

    private Vector3 startPlayerPosition;
    private Quaternion startPlayerRotation;
    private Matrix4x4 playerMatrix;
    private Vector3 startSelfPosition;
    private Quaternion startSelfRotation;

    private bool _isColliding = false;
    public bool isColliding { set { _isColliding = value; } }
    private Vector3 _collisionDirection;
    public Vector3 collisionDirection { set { _collisionDirection = value; } }
    private Rigidbody collidingWith;
    private bool isMovingWhenColliding;

    private bool _isInDeepWater = false;
    public bool isInDeepWater { set { _isInDeepWater = value; } get { return _isInDeepWater; } }

    private bool _isCarriedByPiqueSous = false;
    public bool isCarriedByPiqueSous { get { return _isCarriedByPiqueSous; } }

    #region CollisionCallbacks
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Floor") && _playerInteractingWith.Count > 0) return;
        if (collision.collider.CompareTag("Boat")) return;
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
        if (_playerInteractingWith.Count > 0)

        {

            Debug.Log(collision.collider.name);

            _collisionDirection = collision.GetContact(0).normal;

            _isColliding = true;

            collidingWith = collision.collider.GetComponent<Rigidbody>();

        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collidingWith != null && collidingWith.velocity != Vector3.zero && isMovingWhenColliding)
        {
            _collisionDirection = -_collisionDirection;
            isMovingWhenColliding = false;
        }
        else if (!isMovingWhenColliding)
        {
            _collisionDirection = -_collisionDirection;
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
        outlineScript.enabled = false;
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
    }

    // Remove boat parent from treasure and all players that are carrying it
    public void GetOffBoat()
    {
        self.SetParent(null);
    }

    // Disable collider on the side where the player is interacting with the treasure
    private void DealWithCollider(PlayerController player, GameObject interactingWith)
    {
        interactingWith.GetComponent<BoxCollider>().enabled = false;

        // Snap the player to the center of the side of the treasure
        interactingWith.GetComponent<GetSnappingPosition>().SnapPlayerToPosition(player);
        associateColliders.Add(player, interactingWith);
    }

    public void UpTreasure(Transform interact)
    {
        // Snap treasure to the player
        Vector3 upTreasure = self.position;
        upTreasure.y = interact.position.y + self.lossyScale.y / 2;
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
        if (_isCarriedByPiqueSous) return false;
        // Update player values
        _playerInteractingWith.Add(player);
        player.isCarrying = true;

        player.anim.SetBool("isCarrying", true);
        player.anim.SetTrigger("startCarrying");
        player.sword.SetActive(false);

        // Play Carry Sound
        AudioManager.AMInstance.playerCarrySFX.Post(gameObject);

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
            UpTreasure(player.self);
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
                _playerInteractingWith[i].isLaunching = false;
            }

            StopLaunching();
            return true;
        }

        _playerInteractingWith.Remove(player);

        // If the player cannot carry the treasure due to the number of players already carrying it
        player.isCarrying = false;
        player.carrying = null;

        player.anim.SetBool("isCarrying", false);
        player.sword.SetActive(true);

        return false;
    }

    // When the player pressed the action button when he's on the treasure
    // Launch the treasure
    public void OnAction(PlayerController player)
    {
        player.isLaunching = true;
    }

    public void Launch(PlayerController player)
    {
        int nbPlayers = _playerInteractingWith.Count;
        while (_playerInteractingWith.Count > 0)
        {
            PlayerController p = _playerInteractingWith[0];

            // Update lists values
            _playerInteractingWith.Remove(p);
            associateColliders[p].GetComponent<BoxCollider>().enabled = true;
            associateColliders.Remove(p);

            // Update player values
            p.isInteracting = false;
            p.isCarrying = false;
            p.carrying = null;
            p.isLaunching = false;
            p.selfRigidBody.mass = 1;

            // Update Anim
            p.anim.SetBool("isCarrying", false);
            p.sword.SetActive(true);

            Physics.IgnoreCollision(selfCollider, p.selfCollider, true);
            playerCollisionIgnored.Add(p);
        }

        // Enable rigidbody
        selfRigidbody.isKinematic = false;
        selfRigidbody.useGravity = true;
        Physics.IgnoreCollision(selfCollider, BoatManager.instance.selfCollider, false);
        selfRigidbody.AddForce((playerThrowDir.normalized + (Vector3.up * category.multiplyUpAngle)).normalized * category.forceNbPlayer[nbPlayers - 1], 
            ForceMode.Impulse);
        selfRigidbody.constraints = RigidbodyConstraints.FreezeRotation;

        if (self.parent != null)
            self.SetParent(null);

        // Play Launch Sound
        AudioManager.AMInstance.playerThrowSFX.Post(gameObject);

        isGrounded = false;
    }

    private void StopLaunching()
    {
        foreach(PlayerController player in _playerInteractingWith)
        {
            player.isLaunching = false;
        }
    }

    // When the player is not interacting with the treasure anymore
    public void UninteractWith(PlayerController player)
    {
        StopLaunching();

        // Update player values
        player.isCarrying = false;
        player.isInteracting = false;

        player.anim.SetBool("isCarrying", false);
        player.sword.SetActive(true);

        player.carrying = null;

        // Player does not interact with the treasure anymore
        _playerInteractingWith.Remove(player);

        associateColliders[player].GetComponent<BoxCollider>().enabled = true;
        associateColliders.Remove(player);

        player.playerGraphics.forward = player.self.forward;

        // Update speed malus
        ApplySpeedMalus();
        if (_playerInteractingWith.Count == 1)
        {
            player = _playerInteractingWith[0];
            associateColliders[player].GetComponent<GetSnappingPosition>().SnapPlayerToPosition(player);

            player.self.forward = associateColliders[player].transform.forward;

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

    public void InteractWithPiqueSous(PiqueSousAI piqueSous)
    {
        _isCarriedByPiqueSous = true;

        selfRigidbody.isKinematic = true;

        self.forward = piqueSous.self.forward;
        self.position = piqueSous.treasureAttach.position + piqueSous.self.forward * (piqueSous.self.localScale.z + piqueSous.preset.attachOffset);

        UpTreasure(piqueSous.self);
        self.SetParent(piqueSous.self);
    }

    public void UnInteractWithPiqueSous(PiqueSousAI piqueSous)
    {
        _isCarriedByPiqueSous = false;
        selfRigidbody.isKinematic = false;

        self.SetParent(null);
    }
    #endregion

    private void ApplySpeedMalus()
    {
        // Deal with speed according to the number of player carrying the treasure
        if (_playerInteractingWith.Count == category.maxPlayerCarrying)
            speedMalus = 0;
        else
            speedMalus = category.speedMalus / (_playerInteractingWith.Count * _playerInteractingWith.Count);
    }

    private void TreasureMovement()
    {
        Vector3 direction = Vector3.zero;
        if (_playerInteractingWith.Count > 0)
        {
            selfRigidbody.velocity = Vector3.zero;
            foreach(PlayerController player in _playerInteractingWith)
            {
                if (player.isLaunching) continue;
                Vector3 applyForces = player.movement / _playerInteractingWith.Count;
                applyForces.y = 0.0f;
                direction += applyForces;
            }
            selfRigidbody.velocity = direction;
        }
    }

    public void SelecteTreasure(bool select)
    {
        if (select == true)
        {
            numOfSelected += 1;
        }
        else
        {
            numOfSelected -= 1;
        }

        if (numOfSelected < 1)
        {
            outlineScript.enabled = false;
        }
        else
        {
            outlineScript.enabled = true;
        }
    }

    private void PlayerJoystickDetection()

    {
        foreach (PlayerController player in _playerInteractingWith)

        {
            Vector3 dir = new Vector3(player.playerMovementInput.x, 0.0f, player.playerMovementInput.y);
            globalDir += dir;
        }

        if (globalDir != Vector3.zero)
            playerThrowDir = globalDir;

        globalDir = Vector3.zero;
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
            if (Physics.Raycast(raycastStartPos, -Vector3.up, out hit, 0.5f))
            {
                if (!hit.collider.isTrigger && _playerInteractingWith.Count == 0)
                {
                    selfRigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
                    isGrounded = true;
                }
                while(playerCollisionIgnored.Count > 0)
                {
                    Physics.IgnoreCollision(selfCollider, playerCollisionIgnored[0].selfCollider, false);
                    playerCollisionIgnored.RemoveAt(0);
                }
            }
            
        }
        if (_isColliding && !isCarriedByPiqueSous)
        {
            if (Vector3.Dot(selfRigidbody.velocity, -_collisionDirection) < 0 && selfRigidbody.velocity != Vector3.zero)
            {
                _isColliding = false;
            }
            else
            {

                foreach (PlayerController player in _playerInteractingWith)
                {
                    if (_playerInteractingWith.Count > 1)
                    {
                        associateColliders[player].GetComponent<GetSnappingPosition>().SnapPlayerToPosition(player);
                    }
                }
                self.position = lastPosition;
            }
        }
        else
            lastPosition = self.position;

        PlayerJoystickDetection();
    }

    public string GetTag()
    {
        return gameObject.tag;
    }
}
