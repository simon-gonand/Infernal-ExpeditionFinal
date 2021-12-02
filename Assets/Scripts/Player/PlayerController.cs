using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Self References")]
    public Transform self;
    public Rigidbody selfRigidBody;
    public PlayerPresets playerPreset;

    [Header("Children References")]
    public Transform playerGraphics;
    [SerializeField]
    private Transform attackPoint;
    public Transform playerCarryingPoint;


    [Header ("Anim info")]
    public Animator anim;
    public GameObject sword;
    public GameObject stunFx;

    private Vector2 playerMovementInput = Vector2.zero;

    private IInteractable _interactingWith;
    public IInteractable interactingWith { get { return _interactingWith; } }

    private float nextDash;
    private float dashTimer;
    private Vector3 originalDashPos;
    private Vector3 targetDashPos;

    private float nextAttack;

    private ICarriable _carrying;
    public ICarriable carrying { get { return _carrying; } set { _carrying = value; } }

    private Vector3 _movement;
    public Vector3 movement { get { return _movement; } set { _movement = value; } }

    private Vector3 collisionDirection;

    [System.NonSerialized]
    public List<EnemiesAI> isAttackedBy = new List<EnemiesAI>();

    #region booleans
    // Is the player interacting with something
    private bool _isInteracting = false;
    public bool isInteracting { get { return _isInteracting; } set { _isInteracting = value; } }

    // Is the player carrying a treasure
    private bool _isCarrying = false;
    public bool isCarrying { get { return _isCarrying; } set { _isCarrying = value; } }

    private bool _isCarried = false;
    public bool isCarried { get { return _isCarried; } set { _isCarried = value; } }

    private bool _hasBeenLaunched = false;
    public bool hasBeenLaunched { get { return _hasBeenLaunched; } set { _hasBeenLaunched = value; } }

    // Is the player on the boat
    private bool _isOnBoat = true;
    public bool isOnBoat { get { return _isOnBoat; } set { _isOnBoat = value; } }

    private bool _isSwimming = false;
    public bool isSwimming { set { _isSwimming = value; } get { return _isSwimming; } }

    private bool _isInWater = false;
    public bool isInWater { set { _isInWater = value; } }

    private bool _isStun = false;
    public bool isStun { get { return _isStun; } }

    private bool isDashing = false;
    #endregion

    #region Collision

    private void OnCollisionEnter(Collision collision)
    {
        if (isDashing)
        {
            StopDash();
        }
        if (_isCarrying && collision.collider.GetComponent<IInteractable>() != _interactingWith)
        {
            collisionDirection = collision.GetContact(0).normal;
            if (collision.collider.GetType() == typeof(TerrainCollider)) {
                if (!Physics.Raycast(self.position, collisionDirection, 0.1f))
                    return;
            }
            Treasure treasure = _interactingWith as Treasure;
            if (treasure != null)
                treasure.isColliding = true;
        }
        if (_hasBeenLaunched)
        {
            if (collision.collider.gameObject.transform.position.y < self.position.y)
                _hasBeenLaunched = false;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (_isCarrying && collision.collider.GetComponent<IInteractable>() != _interactingWith)
        {
            if (collision.collider.GetType() == typeof(TerrainCollider))
            {
                if (!Physics.Raycast(self.position, collisionDirection, 0.1f))
                    return;
            }
            Treasure treasure = _interactingWith as Treasure;
            if (treasure != null)
                treasure.isColliding = false;
        }
    }

    private void CheckIfDashCollide()
    {
        if (Physics.Raycast(self.position, self.forward * self.localScale.z, 1.0f))
            StopDash();
    }

    #endregion

    #region InputsManagement
    // When the player moves
    public void OnMove(InputAction.CallbackContext context)
    {
        playerMovementInput = context.ReadValue<Vector2>();
    }

    // When the player pressed the action button
    public void OnAction(InputAction.CallbackContext context)
    {
        if (!_isSwimming && !_isCarried)
        {
            // If the player is interacting with something he can't attack
            if ((_isInteracting || _isCarrying))
            {
                if (context.started)
                {
                    _interactingWith.OnAction(this);
                }
                else if (context.canceled)
                {
                    if (_carrying != null)
                    {
                        _carrying.Launch(this);
                        selfRigidBody.mass = 1;
                    }
                }
            }
            else
            {
                if (context.performed && Time.time > nextAttack && !_isStun)
                {
                    Attack();
                    nextAttack = Time.time + playerPreset.attackCooldown;
                }
            }
        }
    }

    // When the player pressed the dash button
    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && Time.time > nextDash && !_isInteracting && !_isSwimming && !isDashing && !_isCarried)
        {
            isDashing = true;
            Vector3 currentVelocity = selfRigidBody.velocity;
            currentVelocity += self.forward * playerPreset.dashSpeed * Time.deltaTime * 0.1f;
            originalDashPos = self.position;
            targetDashPos = self.position + currentVelocity;
        }
    }

    // When the player pressed the interaction button
    public void OnInteraction(InputAction.CallbackContext context)
    {
        if (!_isSwimming && !_isCarried)
        {
            // If the player is not interacting with anything or carrying a treasure
            if (!_isInteracting && !_isCarrying && context.performed)
            {
                // Define from where the raycast will start
                Vector3 startRayPos = self.position;
                startRayPos.y -= self.lossyScale.y / 2;

                // If the raycast is encountering an interactable
                int layerMask = 1 << LayerMask.NameToLayer("Interactable");
                List<Vector3> raycastsStartPos = new List<Vector3>();

                // Set three different Raycasts (one at the bottom, one at the center and one at the top)
                raycastsStartPos.Add(startRayPos);
                startRayPos.y += self.lossyScale.y;
                raycastsStartPos.Add(startRayPos);
                raycastsStartPos.Add(self.position);

                for (int i = 0; i < raycastsStartPos.Count; ++i)
                {
                    RaycastHit hit;
                    if (Physics.Raycast(raycastsStartPos[i], self.forward, out hit, playerPreset.interactionDistance, layerMask))
                    {
                        if (hit.collider.isTrigger && hit.collider.enabled)
                        {
                            // Stop player's movements
                            playerMovementInput = Vector2.zero;
                            // Set with which interactable the player is interacting with
                            _interactingWith = hit.collider.gameObject.GetComponentInParent<IInteractable>();
                            if (!_interactingWith.InteractWith(this, hit.collider.gameObject))
                                _interactingWith = null;
                            else
                                selfRigidBody.mass = 1000;
                            break;
                        }
                    }
                }
            }
            // Else put the treasure down or uninteract with the interactable
            else if ((_isInteracting || _isCarrying) && context.performed)
            {
                _interactingWith.UninteractWith(this);
                selfRigidBody.mass = 1;
            }
        }
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(0);
    }
    #endregion

    private void Attack()
    {
        // Play attack animation
        anim.SetTrigger("attack");

        Collider[] hit = Physics.OverlapSphere(attackPoint.position, playerPreset.attackRange);
        foreach(Collider hitted in hit)
        {
            // For sound design
            if (hitted.CompareTag("Enemy"))
            {
                Debug.Log("Enemy has been attacked");
                // Play enemy attacked sound
                EnemiesAI enemy = hitted.GetComponent<EnemiesAI>();
                enemy.Die(this);
                return;
            }
            if (hitted.CompareTag("Player"))
            {
                PlayerController attacked = hitted.GetComponent<PlayerController>();
                if (attacked != this && !attacked._isStun)
                {
                    attacked.StunPlayer();
                }
            }
            // etc...
        }
    }

    public void StunPlayer()
    {
        StartCoroutine(StunWait());
        movement = Vector3.zero;
    }

    private IEnumerator StunWait()
    {
        _isStun = true;

        anim.SetBool("isStun", true);
        anim.SetTrigger("startStun");
        stunFx.SetActive(true);

        // Update stun bool in animation for animation ?
        if (isCarrying)
        {
            _carrying.UninteractWith(this);
        }
        Debug.Log("Player is stun");
        yield return new WaitForSeconds(playerPreset.stunTime);
        Debug.Log("Player is not stun anymore");

        _isStun = false;
        anim.SetBool("isStun", false);
        stunFx.SetActive(false);
        // Update stun bool in animation ?
    }

    public void UpdateSwimming()
    {
        if (selfRigidBody.velocity.y > 0.5f && _isSwimming)
        {
            selfRigidBody.useGravity = true;

            Vector3 resetRotation = playerGraphics.eulerAngles;
            resetRotation.x = 0.0f;
            playerGraphics.eulerAngles = resetRotation;

            _isSwimming = false;

            anim.SetBool("isSwiming", false);
            sword.SetActive(true);
        }
        else if (_isSwimming)
        {
            
            // There is no gravity so the player should not move on the y-axis
            _movement.y = 0.0f;
            selfRigidBody.velocity = _movement;
            // The player must stay at the top of the water
            Vector3 upPlayer = self.position;
            upPlayer.y = NotDeepWater.instance.self.position.y;
            self.position = upPlayer;
            
        }
    }

    // Update movements of the player
    private void PlayerMovement()
    {
        float currentSpeed = playerPreset.playerGroundSpeed;
        // If player is swimming reduce speed
        if (_isSwimming && _isInWater)
        {
            currentSpeed = playerPreset.playerSwimSpeed;

            anim.SetBool("isSwiming", true);
            sword.SetActive(false);
        }
        // If player is in not deep water reduce speed
        else if (_isInWater)
            currentSpeed = playerPreset.playerInNotDeepWaterSpeed;

        // Apply speed malus if the player is carrying an heavy treasure
        Treasure transportedTreasure = _carrying as Treasure;
        if (_isCarrying && transportedTreasure != null)
            currentSpeed -= transportedTreasure.speedMalus;

        // Apply movements
        Vector3 calculatePlayerInput = playerMovementInput * currentSpeed * Time.deltaTime;
        _movement = new Vector3(calculatePlayerInput.x, selfRigidBody.velocity.y,
            calculatePlayerInput.y);
        selfRigidBody.velocity = _movement;
        if (_isCarrying && transportedTreasure != null)
        {
            transportedTreasure.UpdatePlayerMovement(this);
            if (transportedTreasure.playerInteractingWith.Count > 1)
            {
                if ((transportedTreasure.selfRigidbody.velocity.x < 0.1f || transportedTreasure.selfRigidbody.velocity.x > 0.1f) ||
                    (transportedTreasure.selfRigidbody.velocity.z < 0.1f || transportedTreasure.selfRigidbody.velocity.z > 0.1f))
                {
                    transportedTreasure.UpdatePlayerRotation(this, playerGraphics);
                }
            }
            else
                transportedTreasure.UpdatePlayerRotation(this, self);
        }

        // Player can't go up
        if (selfRigidBody.velocity.y > 0)
            selfRigidBody.velocity = new Vector3(selfRigidBody.velocity.x, 0.0f, selfRigidBody.velocity.z);

        // If velocity on Y is equal to 0.0 then it means that the player is swimming
        // if not then it means he must deal with gravity
        UpdateSwimming();

        // Set the rotation of the player according to his movements
        if (_movement.x != 0 || _movement.z != 0)
        {
            _movement.y = 0.0f;
            self.forward = _movement;
        }
    }

    private void Dash()
    {
        anim.SetBool("isDashing", true);
        //selfRigidBody.velocity += self.forward * playerPreset.dashSpeed;

        float normalizedTimer = dashTimer / playerPreset.dashTime;
        
        Vector3 newPos = Vector3.Lerp(originalDashPos, targetDashPos, normalizedTimer);
        self.position = newPos;

        dashTimer += Time.deltaTime;

        if (dashTimer > playerPreset.dashTime)
        {
            StopDash();
        }
    }

    private void StopDash()
    {
        selfRigidBody.velocity = Vector3.zero;
        anim.SetBool("isDashing", false);
        nextDash = Time.time + playerPreset.dashCooldown;
        isDashing = false;
        dashTimer = 0.0f;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!_isStun && !_isCarried && !_hasBeenLaunched)
        {
            if (isDashing)
            {
                Dash();
                CheckIfDashCollide();
            }
            else
                PlayerMovement(); 
            InfoAnim();
        }
    }

    void InfoAnim()
    {
        
        if (!_isStun && !_isCarried)
        {
            if (playerMovementInput.x != 0 || playerMovementInput.y != 0)
            {
                anim.SetBool("isMoving", true);

                if (Mathf.Abs(playerMovementInput.x) > Mathf.Abs(playerMovementInput.y))
                {
                    anim.SetFloat("playerSpeed", Mathf.Abs(playerMovementInput.x));
                }
                else
                {
                    anim.SetFloat("playerSpeed", Mathf.Abs(playerMovementInput.y));
                }
            }
            else
            {
                anim.SetBool("isMoving", false);
            }
        }
    }
}
