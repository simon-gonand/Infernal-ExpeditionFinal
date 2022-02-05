using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Treasure : MonoBehaviour, ICarriable
{
    [Header("Treasure price")]
    public int price;

    public Transform self;
    public Rigidbody selfRigidbody;
    public BoxCollider selfColliderX;
    public BoxCollider selfColliderZ;
    public TreasuresCategory category;
    public MeshFilter mesh;
    public GameObject selfAura;
    [HideInInspector]
    public float speedMalus = 0.0f;

    private List<PlayerController> _playerInteractingWith = new List<PlayerController>();
    public List<PlayerController> playerInteractingWith { get { return _playerInteractingWith; } }

    private List<PlayerController> playerCollisionIgnored = new List<PlayerController>();

    private Dictionary<PlayerController, GameObject> associateColliders = new Dictionary<PlayerController, GameObject>();
    private bool isGrounded = false;

    [HideInInspector]public Vector3 playerThrowDir;
    private Vector3 globalDir;

    private int numOfSelected;
    public Outline outlineScript;

    private bool _isInDeepWater = false;
    public bool isInDeepWater { set { _isInDeepWater = value; } get { return _isInDeepWater; } }

    private bool _isCarriedByPiqueSous = false;
    public bool isCarriedByPiqueSous { get { return _isCarriedByPiqueSous; } }

    private Vector3 spawnPos;

    private void Start()
    {
        outlineScript.enabled = false;
        Physics.IgnoreCollision(selfColliderZ, selfColliderX, true);
        spawnPos = self.position;
    }

    public void UpdatePlayerRotation(PlayerController player, Transform playerTransform)
    {
        if (associateColliders[player] != null)
                playerTransform.forward = associateColliders[player].transform.forward;
    }

    public void UpdatePlayerMovement(PlayerController player)
    {
        if (_playerInteractingWith.Count > 1)
        {    
            if (associateColliders[player] != null)
            {
                Vector3 newPlayerPos = associateColliders[player].transform.position;
                newPlayerPos.y = player.self.position.y;
                player.self.position = newPlayerPos;
            }
        }
    }

    // Get the treasure and all the players that are carrying it on boat
    public void GetOnBoat(Transform entryPosition)
    {
        Vector3 treasureOnBoat = entryPosition.position;
        treasureOnBoat.y += self.lossyScale.y / 2;
        foreach (PlayerController player in playerInteractingWith)
        {
            player.self.position = entryPosition.position;
        }
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

    private void AdjustCollider(Vector3 localSnapPosition, PlayerController player, bool add)
    {
        if (_playerInteractingWith.Count == 1)
        {
            player.AdjustSoloColliderSize(localSnapPosition, this, add);
            return;
        }
        if (localSnapPosition.x > 0)
        {
            float offset = (localSnapPosition.x + player.selfCollider.bounds.size.z) / 2;
            Vector3 newSize = selfColliderX.size;
            Vector3 newCenter = selfColliderX.center;
            if (add)
            {
                newSize.x += offset;
                newCenter.x += offset / 2;
            }
            else
            {
                newSize.x -= offset;
                newCenter.x -= offset / 2;
            }
            selfColliderX.size = newSize;
            selfColliderX.center = newCenter;
        }
        else if (localSnapPosition.x < 0)
        {
            float offset = (localSnapPosition.x - player.selfCollider.bounds.size.z) / 2;
            Vector3 newSize = selfColliderX.size;
            Vector3 newCenter = selfColliderX.center;
            if (add)
            {
                newSize.x -= offset;
                newCenter.x += offset / 2;
            }
            else
            {
                newSize.x += offset;
                newCenter.x -= offset / 2;
            }
            selfColliderX.size = newSize;
            selfColliderX.center = newCenter;
        }
        else if (localSnapPosition.z > 0)
        {
            float offset = (localSnapPosition.z + player.selfCollider.bounds.size.z) / 2;
            Vector3 newSize = selfColliderZ.size;
            Vector3 newCenter = selfColliderZ.center;
            if (add)
            {
                newSize.z += offset;
                newCenter.z += offset / 2;
            }
            else
            {
                newSize.z -= offset;
                newCenter.z -= offset / 2;
            }
            selfColliderZ.size = newSize;
            selfColliderZ.center = newCenter;
        }
        else if (localSnapPosition.z < 0)
        {
            float offset = (localSnapPosition.z - player.selfCollider.bounds.size.z) / 2;
            Vector3 newSize = selfColliderZ.size;
            Vector3 newCenter = selfColliderZ.center;
            if (add)
            {
                newSize.z -= offset;
                newCenter.z += offset / 2;
            }
            else
            {
                newSize.z += offset;
                newCenter.z -= offset / 2;
            }
            selfColliderZ.size = newSize;
            selfColliderZ.center = newCenter;
        }
    }

    public void UpTreasure(Transform interact)
    {
        // Snap treasure to the player
        Vector3 upTreasure = self.position;
        upTreasure.y = interact.position.y + self.lossyScale.y / 2;
        self.position = upTreasure;
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

        selfAura.SetActive(false);

        player.carrying = this;

        selfRigidbody.useGravity = false;

        // Update speed malus
        ApplySpeedMalus();

        
        // If there is more than one player to carry it, snap treasures to the players' joint
        if (_playerInteractingWith.Count <= category.maxPlayerCarrying)
        {
            // Play Carry Sound
            AudioManager.AMInstance.playerCarrySFX.Post(gameObject);

            Physics.IgnoreCollision(selfColliderX, player.selfCollider, true);
            Physics.IgnoreCollision(selfColliderZ, player.selfCollider, true);

            DealWithCollider(player, interactingWith);
            AdjustCollider(interactingWith.transform.localPosition, player, true);

            // If the player is alone to carry it just snap the treasure as child of the player
            if (_playerInteractingWith.Count == 1)
            {
                Physics.IgnoreCollision(selfColliderX, BoatManager.instance.selfCollider, true);
                Physics.IgnoreCollision(selfColliderZ, BoatManager.instance.selfCollider, true);

                UpTreasure(player.self);
                selfRigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePosition;
                selfRigidbody.useGravity = false;

                player.selfRigidBody.velocity = Vector3.zero;
                player.soloCarrierCollider.enabled = true;
                selfColliderX.enabled = false;
                selfColliderZ.enabled = false;

                self.SetParent(player.self);

            }
            else if (_playerInteractingWith.Count == 2)
            {
                _playerInteractingWith[0].soloCarrierCollider.enabled = false;
                selfColliderX.enabled = true;
                selfColliderZ.enabled = true;

                selfRigidbody.isKinematic = false;
                selfRigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;

                self.SetParent(null);
            }

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
        player.SweatActivator(false);
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
        if (_playerInteractingWith.Count == 1)
        {
            _playerInteractingWith[0].soloCarrierCollider.enabled = false;
            selfColliderX.enabled = true;
            selfColliderZ.enabled = true;
            selfRigidbody.isKinematic = false;
        }
        while (_playerInteractingWith.Count > 0)
        {
            PlayerController p = _playerInteractingWith[0];

            // Update lists values
            AdjustCollider(associateColliders[p].transform.localPosition, p, false);
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
            p.SweatActivator(false);
            p.sword.SetActive(true);

            Physics.IgnoreCollision(selfColliderX, p.selfCollider, true);
            Physics.IgnoreCollision(selfColliderZ, p.selfCollider, true);
            playerCollisionIgnored.Add(p);
        }

        selfAura.SetActive(true);
        
        if (self.parent != null)
            self.SetParent(null);
        // Enable rigidbody
        selfRigidbody.useGravity = true;
        isGrounded = false;
        Physics.IgnoreCollision(selfColliderX, BoatManager.instance.selfCollider, false);
        Physics.IgnoreCollision(selfColliderZ, BoatManager.instance.selfCollider, false);
        selfRigidbody.velocity = Vector3.zero;
        selfRigidbody.constraints = RigidbodyConstraints.FreezeRotation;
        selfRigidbody.AddForce((playerThrowDir.normalized + (Vector3.up * category.multiplyUpAngle)).normalized * category.forceNbPlayer[nbPlayers - 1], 
            ForceMode.Impulse);

        // Play Launch Sound
        AudioManager.AMInstance.playerThrowSFX.Post(gameObject);

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
        player.SweatActivator(false);
        player.sword.SetActive(true);

        player.carrying = null;

        AdjustCollider(associateColliders[player].transform.localPosition, player, false);
        // Player does not interact with the treasure anymore
        _playerInteractingWith.Remove(player);

        
        associateColliders[player].GetComponent<BoxCollider>().enabled = true;
        associateColliders.Remove(player);

        player.playerGraphics.forward = player.self.forward;

        Physics.IgnoreCollision(selfColliderX, player.selfCollider, false);
        Physics.IgnoreCollision(selfColliderZ, player.selfCollider, false);

        // Update speed malus
        ApplySpeedMalus();
        if (_playerInteractingWith.Count == 1)
        {
            player = _playerInteractingWith[0];
            associateColliders[player].GetComponent<GetSnappingPosition>().SnapPlayerToPosition(player);

            player.self.forward = associateColliders[player].transform.forward;

            self.SetParent(player.self);

            _playerInteractingWith[0].soloCarrierCollider.enabled = true;
            selfColliderX.enabled = false;
            selfColliderZ.enabled = false;
        }
        if (_playerInteractingWith.Count < 1)
        {
            Physics.IgnoreCollision(selfColliderX, BoatManager.instance.selfCollider, false);
            Physics.IgnoreCollision(selfColliderZ, BoatManager.instance.selfCollider, false);

            player.soloCarrierCollider.enabled = false;
            selfColliderX.enabled = true;
            selfColliderZ.enabled = true;

            // Enable rigidbody
            selfRigidbody.useGravity = true;
            selfRigidbody.isKinematic = false;
            selfRigidbody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
            isGrounded = false;

            self.SetParent(null);
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

    public Vector3 GetTreasuresVelocity()
    {
        Vector3 direction = Vector3.zero;
        foreach (PlayerController player in _playerInteractingWith)
        {
            Vector3 applyForces = player.movement / _playerInteractingWith.Count;
            applyForces.y = 0.0f;
            direction += applyForces;
        }
        return direction;
    }

    private void TreasureMovement()
    {
        if (_playerInteractingWith.Count > 0)
            selfRigidbody.velocity = Vector3.zero;
        if (_playerInteractingWith.Count > 1)
        {
            selfRigidbody.velocity = GetTreasuresVelocity();
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
            Vector3 dir = new Vector3(player.playerMovementInput.x, 0.0f, player.playerMovementInput.y);            globalDir += dir;
        }        if (globalDir != Vector3.zero)
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
            {
                if (LevelManager.instance.levelId == 0)
                {
                    self.position = spawnPos;
                }
                else
                {
                    Destroy(gameObject);
                }
            }
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
                    selfRigidbody.isKinematic = true;
                }
                while(playerCollisionIgnored.Count > 0)
                {
                    Physics.IgnoreCollision(selfColliderX, playerCollisionIgnored[0].selfCollider, false);
                    Physics.IgnoreCollision(selfColliderZ, playerCollisionIgnored[0].selfCollider, false);
                    playerCollisionIgnored.RemoveAt(0);
                }
            }
            
        }

        PlayerJoystickDetection();
        UpdateWeightNeed();
    }

    public string GetTag()
    {
        return gameObject.tag;
    }

    public void UpdateWeightNeed()
    {
        if (_playerInteractingWith.Count > 0)
        {
            if (_playerInteractingWith.Count < category.maxPlayerCarrying)
            {
                foreach (PlayerController player in _playerInteractingWith)
                {
                    player.SweatActivator(true);
                }
            }
            else
            {
                foreach (PlayerController player in _playerInteractingWith)
                {
                    player.SweatActivator(false);
                }
            }
        }
    }
}