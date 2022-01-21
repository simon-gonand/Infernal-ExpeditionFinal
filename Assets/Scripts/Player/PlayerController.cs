using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Self References")]
    public Transform self;
    public Rigidbody selfRigidBody;
    public Collider selfCollider;
    public CarryPlayer selfCarryPlayer;
    public PlayerPresets playerPreset;
    public PlayerThrowUI selfPlayerThrowUi;
    public GameObject sweatParticleSysteme;
    public PlayerInput selfPlayerInput;
    public ClosingTutoUI closingTutoUI;

    [Header("Children References")]
    public Transform playerGraphics;
    public SkinnedMeshRenderer selfRenderer;
    [SerializeField]
    private Transform attackPoint;
    public Transform playerCarryingPoint;
    public Outline outline;
    public GameObject playerUICircle;

    [Header ("Anim info")]
    public Animator anim;
    public GameObject sword;
    public GameObject stunFx;

    [Header ("Debug")]
    public bool drawIteractLine;

    [Header("Audio")]
    public bool canPlaySound = false;

    private int _id;
    public int id { set { _id = value; } }

    public LayerMask mask;
    private IInteractable _interactingWith;
    public IInteractable interactingWith { get { return _interactingWith; } }

    private LevelSelection _levelSelectionTable;
    public LevelSelection levelSelectionTable { set { _levelSelectionTable = value; } }

    private float nextDash;
    private float dashTimer;
    private Vector3 originalDashPos;
    private Vector3 targetDashPos;

    private float nextAttack;

    private ICarriable _carrying;
    public ICarriable carrying { get { return _carrying; } set { _carrying = value; } }

    private Vector2 _playerMovementInput = Vector2.zero;
    public Vector2 playerMovementInput { get { return _playerMovementInput; } }
    private Vector3 _movement;
    public Vector3 movement { get { return _movement; } }

    private Vector3 collisionDirection;
    private GameObject treasureInFront;

    private float playerY;

    [System.NonSerialized]
    public List<EnemiesAI> isAttackedBy = new List<EnemiesAI>();

    [HideInInspector]public Vector3 playerThrowDir;


    #region booleans
    // Is the player interacting with something
    private bool _isInteracting = false;
    public bool isInteracting { get { return _isInteracting; } set { _isInteracting = value; } }

    // Is the player carrying a treasure
    private bool _isCarrying = false;
    public bool isCarrying { get { return _isCarrying; } set { _isCarrying = value; } }

    // Is the player carried by another player
    private bool _isCarried = false;
    public bool isCarried { get { return _isCarried; } set { _isCarried = value; } }

    // If the player has been launched by another player
    private bool _hasBeenLaunched = false;
    public bool hasBeenLaunched { get { return _hasBeenLaunched; } set { _hasBeenLaunched = value; } }

    // If the player has press the launch button but didn't release yet
    private bool _isLaunching = false;
    public bool isLaunching { get { return _isLaunching; } set { _isLaunching = value; } }

    // Is the player on the boat
    private bool _isOnBoat = false;
    public bool isOnBoat { get { return _isOnBoat; } set { _isOnBoat = value; } }

    private bool _isSwimming = false;
    public bool isSwimming { set { _isSwimming = value; } get { return _isSwimming; } }

    private bool _isInWater = false;
    public bool isInWater { set { _isInWater = value; } }

    private bool _isStun = false;
    public bool isStun { get { return _isStun; } }
    
    private bool _isDead = false;
    public bool isDead { get { return _isDead; } }

    #region ModifierBooleans
    // Is the No Attack modifier has been triggered
    private bool _canAttack = true;
    public bool canAttack { set { _canAttack = value; } }

    // Is the No Dash modifier has been triggered
    private bool _canDash = true;
    public bool canDash { set { _canDash = value; } }
    #endregion

    private bool isDashing = false;
    private bool isGrounded = false;
    public bool isColliding = false;
    #endregion

    #region Reset
    public void ResetPlayer()
    {
        isAttackedBy.Clear();
        if (_carrying != null)
        {
            Treasure treasure = _carrying as Treasure;
            if (treasure != null)
                Destroy(treasure.gameObject);
            _carrying = null;
        }
        if (_interactingWith != null)
            _interactingWith = null;

        ResetStates();
        ResetAnimStates();

        selfRenderer.enabled = true;
        selfPlayerThrowUi.gameObject.SetActive(true);

        selfRigidBody.velocity += Vector3.up;
        UpdateSwimming();
        selfRigidBody.velocity -= Vector3.up;
        selfRigidBody.mass = 1;
    }

    private void ResetStates()
    {
        _isInteracting = false;
        _isCarrying = false;
        _isCarried = false;
        _hasBeenLaunched = false;
        _isLaunching = false;
        _isOnBoat = false;
        _isSwimming = false;
        _isInWater = false;
        _isStun = false;
        isDashing = false;
        _isDead = false;
        isGrounded = false;

        sword.SetActive(true);
    }

    private void ResetAnimStates()
    {
        anim.SetBool("isMoving", false);
        anim.SetBool("isDashing", false);
        anim.SetBool("isSwiming", false);
        anim.SetBool("isCarrying", false);
        anim.SetBool("isStun", false);
        anim.SetBool("isGettingCarried", false);

        anim.Play("Breathing Idle");
    }
    #endregion

    #region Collision

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Treasures") && isGrounded)
        {
            if (_isCarrying)
            {
                Treasure treasure = carrying as Treasure;
                if (treasure != null && treasure.selfCollider == collision.collider)
                {
                    return;
                }
                else
                {
                    isColliding = true;
                    selfRigidBody.velocity = Vector3.zero;
                    collisionDirection = collision.GetContact(0).normal;
                    if (treasure != null)
                    {
                        treasure.isColliding = true;
                        treasure.collisionDirection = collisionDirection;
                    }
                }
            }
            if(!_isCarrying)
            {
                isColliding = true;
                selfRigidBody.velocity = Vector3.zero;
                collisionDirection = collision.GetContact(0).normal;
            }
            selfRigidBody.constraints = RigidbodyConstraints.FreezeRotation | RigidbodyConstraints.FreezePositionY;
        }
        if (isDashing)
        {
            collisionDirection = collision.GetContact(0).normal;
            if (Physics.Raycast(self.position, -collisionDirection, 1.0f, mask))
                StopDash();
        }
        if (_isCarrying && collision.collider.GetComponent<ICarriable>() != _carrying &&
            collision.collider.gameObject.layer != LayerMask.NameToLayer("Floor"))
        {
            collisionDirection = collision.GetContact(0).normal;
            if (!Physics.Raycast(self.position, -collisionDirection, 1.0f, mask))
                return;
            Treasure treasure = _carrying as Treasure;
            if (treasure != null)
            {
                treasure.isColliding = true;
                treasure.collisionDirection = -collisionDirection;
            }
        }
        if (_hasBeenLaunched)
        {
            if (collision.collider.gameObject.transform.position.y < self.position.y)
            {
                selfCarryPlayer.StopFall();
            }
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Treasures"))
        {
            if (_isCarrying)
            {
                Treasure t = _carrying as Treasure;
                if (t != null && t.selfCollider == collision.collider)
                {
                    return;
                }
                else
                    isColliding = false;
            }
            else
            {
                isColliding = false;
                selfRigidBody.constraints = RigidbodyConstraints.FreezeRotation;
            }
        }
        if (_isCarrying && collision.collider.GetComponent<IInteractable>() != _interactingWith)
        {
            if (!Physics.Raycast(self.position, -collisionDirection, 0.1f, mask))
                return;
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
        _playerMovementInput = context.ReadValue<Vector2>();
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
                    _isLaunching = true;
                    _interactingWith.OnAction(this);
                }
                else if (context.canceled)
                {
                    if (_carrying != null)
                    {
                        _carrying.Launch(this);
                    }
                }
            }
            else
            {
                if (context.performed && Time.time > nextAttack && !_isStun && _canAttack)
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
        if (context.performed && Time.time > nextDash && !_isInteracting && !_isSwimming && !isDashing && !_isCarried && isGrounded && _canDash)
        {
            isDashing = true;
            Vector3 currentVelocity = selfRigidBody.velocity;
            currentVelocity += self.forward * playerPreset.dashSpeed * Time.deltaTime * 0.1f;
            currentVelocity.y = 0.0f;
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
            if (!_isInteracting && !_isCarrying && context.started)
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
                            _playerMovementInput = Vector2.zero;
                            // Set with which interactable the player is interacting with
                            _interactingWith = hit.collider.gameObject.GetComponentInParent<IInteractable>();
                            _isInteracting = true;
                            if (!_interactingWith.InteractWith(this, hit.collider.gameObject))
                            {
                                _interactingWith = null;
                                _isInteracting = false;
                            }
                            else
                            {
                                selfRigidBody.mass = 1000;
                                selfRigidBody.constraints = RigidbodyConstraints.FreezeRotation;
                                isColliding = false;
                            }
                            break;
                        }
                    }
                }
            }
            // Else put the treasure down or uninteract with the interactable
            else if ((_isInteracting || _isCarrying) && context.canceled && !interactingWith.GetTag().Equals("LevelSelection"))
            {
                _interactingWith.UninteractWith(this);
                selfRigidBody.constraints = RigidbodyConstraints.FreezeRotation;
                selfRigidBody.mass = 1;
            }
        }
    }

    #region UI
    public void OnValidationUI(InputAction.CallbackContext context)
    {
        if (_levelSelectionTable != null && context.performed)
            _levelSelectionTable.ActivateLevelSelectionUi();
    }

    public void OnCancelUI(InputAction.CallbackContext context)
    {
        Debug.Log(_levelSelectionTable.uiActivate);
        if (_levelSelectionTable != null && _levelSelectionTable.uiActivate && context.performed)
            _levelSelectionTable.Back();
    }

    public void OnChangeSelectionUI(InputAction.CallbackContext context)
    {
        if (_levelSelectionTable != null)
        {
            _levelSelectionTable.ChangeSelection(context.ReadValue<Vector2>());
        }
    }

    #endregion

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
            PauseMenu.instance.PauseGame();
    }

    public void OnRestart(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ResetPlayer();
            Die();
        }
    }
    #endregion

    private void Attack()
    {
        // Play attack animation
        anim.SetTrigger("attack");
        AudioManager.AMInstance.playerAttackSFX.Post(gameObject);

        Collider[] hit = Physics.OverlapSphere(attackPoint.position, playerPreset.attackRange);
        foreach(Collider hitted in hit)
        {
            // For sound design
            if (hitted.CompareTag("Enemy"))
            {
                Debug.Log("Enemy has been attacked");
                // Play sword impact sound
                EnemiesAI enemy = hitted.GetComponent<EnemiesAI>();
                enemy.Die();
                return;
            }
            if (hitted.CompareTag("Player"))
            {
                // Play sword impact sound
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
        _movement = Vector3.zero;
    }

    private IEnumerator StunWait()
    {
        _isStun = true;

        anim.SetBool("isStun", true);
        anim.SetTrigger("startStun");
        stunFx.SetActive(true);

        //Play Stunt Sound
        AudioManager.AMInstance.playerStuntSFX.Post(gameObject);

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

        // Stop sound stun
    }

    public void UpdateSwimming()
    {
        if (selfRigidBody.velocity.y > 0.3f)
        {
            selfRigidBody.useGravity = true;

            Vector3 resetRotation = playerGraphics.eulerAngles;
            resetRotation.x = 0.0f;
            playerGraphics.eulerAngles = resetRotation;

            _isSwimming = false;

            anim.SetBool("isSwiming", false);
            sword.SetActive(true);
        }
        else
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

            // Play swim sound

            anim.SetBool("isSwiming", true);
            sword.SetActive(false);
        }
        // If player is in not deep water reduce speed
        else if (_isInWater)
        {
            // Play walk in water
            currentSpeed = playerPreset.playerInNotDeepWaterSpeed;
        }
        else
        {
            // Play Footsteps sound
        }

        // Apply speed malus if the player is carrying an heavy treasure
        Treasure transportedTreasure = _carrying as Treasure;
        if (_isCarrying && transportedTreasure != null)
        {
            currentSpeed -= transportedTreasure.speedMalus;
            if (currentSpeed < 50.0f)
                currentSpeed = 50.0f;
        }

        // Apply movements
        Vector3 calculatePlayerInput = playerMovementInput * currentSpeed * Time.deltaTime;
        _movement = new Vector3(calculatePlayerInput.x, selfRigidBody.velocity.y,
            calculatePlayerInput.y);
        selfRigidBody.velocity = _movement;
        
        // Set the rotation of the player according to his movements
        if (_movement.x != 0 || _movement.z != 0)
        {
            _movement.y = 0.0f;
            self.forward = _movement;
            playerGraphics.forward = self.forward;
        }
        
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

        // If velocity on Y is equal to 0.0 then it means that the player is swimming
        // if not then it means he must deal with gravity
        if (_isSwimming)
            UpdateSwimming();
        
        // Player can't go up
        else if (selfRigidBody.velocity.y > 0)
            selfRigidBody.velocity = new Vector3(selfRigidBody.velocity.x, 0.0f, selfRigidBody.velocity.z);
    }

    private void Dash()
    {
        anim.SetBool("isDashing", true);

        // Dash sound
        AudioManager.AMInstance.playerDashSFX.Post(gameObject);

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

    public void Die()
    {
        if (_isDead == false)
        {
            _isDead = true;
            // Play death out of bounds sound
            if (isInteracting)
            {
                interactingWith.UninteractWith(this);
            }

            StartCoroutine(Respawn());
            RespawnUiManager.instance.SpawnPicto(_id);
        }
    }

    private IEnumerator Respawn()
    {
        Vector3 respawnPosition = PlayerManager.instance.SetPlayerPosition(_id, false).position;
        respawnPosition.y += self.lossyScale.y;
        if (isSwimming)
        {
            selfRigidBody.velocity += Vector3.up;
            UpdateSwimming();
        }
        selfRigidBody.velocity = Vector3.zero;
        self.position = respawnPosition;

        selfRenderer.enabled = false;
        sword.SetActive(false);
        selfPlayerThrowUi.gameObject.SetActive(false);

        yield return new WaitForSeconds(playerPreset.respawnCooldown);

        _isDead = false;
        selfRenderer.enabled = true;
        sword.SetActive(true);
        selfPlayerThrowUi.gameObject.SetActive(true);

        // Play respawn sound
        AudioManager.AMInstance.playerRespawnSFX.Post(gameObject);
    }

    public bool CheckMovementWhenColliding()
    {
        Vector3 movement = new Vector3(_playerMovementInput.x, 0.0f, _playerMovementInput.y);
        if (Vector3.Dot(movement, -collisionDirection) <= 0.1 && _playerMovementInput != Vector2.zero)
            return true;
        return false;
    }

    void FixedUpdate()
    {
        if (!isColliding && !_isStun && !_isCarried && !_hasBeenLaunched && !_isDead && 
            ((_isInteracting && _carrying != null) ? true : !_isInteracting))
        {
            if (isDashing)
            {
                Dash();
                CheckIfDashCollide();
            }
            else
                PlayerMovement();
        }
        else if (isColliding)
        {
            if (CheckMovementWhenColliding())
                PlayerMovement();
        }
    }

    void Update()
    {
        InfoAnim();
        TreasureDetectionForOutline();
        PlayerJoystickDetection();

        CheckIsGrounded();
        if (isGrounded)
            playerY = self.position.y;
        CheckFallingWhenCarrying();
        CheckIsUnderMap();
    }
    

    private void TreasureDetectionForOutline()
    {
        RaycastHit hit;

        Vector3 offSet = new Vector3(0,-0.5f,0);

        // Draw raycast foward the player
        if (Physics.Raycast(transform.position + offSet, transform.forward, out hit, playerPreset.interactionDistance))
        {
            // Draw ray for debug
            if (drawIteractLine == true)
            {
                Debug.DrawRay(transform.position + offSet, transform.forward * playerPreset.interactionDistance, Color.green);
            }

            // Check if raycast hit a Treasures
            if (hit.collider.gameObject.transform.parent.CompareTag("Treasures"))
            {
                // If it's the same treasure detected, cancel everything
                if (treasureInFront == hit.collider.gameObject.transform.parent.gameObject)
                {
                    treasureInFront.GetComponent<Treasure>().selfAura.SetActive(false);
                    return;
                }
                else
                {
                    // Check if there is an old treasure in front
                    if (treasureInFront != null)
                    {
                        // Unselecte the old treasure
                        treasureInFront.GetComponent<Treasure>().SelecteTreasure(false);
                        if (!isCarrying || (isCarrying && treasureInFront.GetComponent<Treasure>() != (Treasure)carrying))
                            treasureInFront.GetComponent<Treasure>().selfAura.SetActive(true);
                    }

                    // Save and selected the new treasure
                    treasureInFront = hit.collider.gameObject.transform.parent.gameObject;
                    treasureInFront.GetComponent<Treasure>().SelecteTreasure(true);
                    treasureInFront.GetComponent<Treasure>().selfAura.SetActive(false);
                }
            }
        }
        // If there is no more treasure detected
        else if (treasureInFront != null)
        {
            // Reset and unselect the old treasure
            treasureInFront.GetComponent<Treasure>().SelecteTreasure(false);
            if (!isCarrying || (isCarrying && treasureInFront.GetComponent<Treasure>() != (Treasure)carrying))
                treasureInFront.GetComponent<Treasure>().selfAura.SetActive(true);
            treasureInFront = null;
        }
        else
        {
            // Draw ray for debug
            if (drawIteractLine == true)
            {
                Debug.DrawRay(transform.position + offSet, transform.forward * playerPreset.interactionDistance, Color.red);
            }
        }
    }
    void InfoAnim()
    {
        
        if (!_isStun && !_isCarried && !_isDead && ((_isInteracting && _carrying != null) ? !_isLaunching : !_isInteracting))
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
    private void PlayerJoystickDetection()
    {
        if (playerMovementInput != Vector2.zero)
            playerThrowDir = new Vector3(playerMovementInput.x, 0, playerMovementInput.y).normalized;
    }
    
    public void SweatActivator(bool activate)
    {
        if (activate)
        {
            sweatParticleSysteme.SetActive(true);
        }
        else
        {
            sweatParticleSysteme.SetActive(false);
        }
    }


    private void CheckFallingWhenCarrying()
    {
        if (_carrying != null)
        {
            Treasure treasure = _carrying as Treasure;
            if (treasure != null && treasure.self.position.y - self.position.y > self.lossyScale.y && !isGrounded)
            {
                treasure.UninteractWith(this);
                selfRigidBody.mass = 1;
            }
        }
    }

    private void CheckIsGrounded()
    {
        Vector3 startPos = self.position;
        startPos.y -= selfCollider.bounds.size.y / 4;
        Debug.DrawRay(startPos, Vector3.down * 0.5f);

        if (Physics.Raycast(startPos, Vector3.down, 0.5f))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
            if (!isCarried)
            {
                selfRigidBody.constraints = RigidbodyConstraints.FreezeRotation;
            }
            if (canPlaySound == false)
            {
                canPlaySound = true;
            }
        }

        //Sound
        if (canPlaySound && isGrounded)
        {
            AudioManager.AMInstance.playerGroundImpactSFX.Post(gameObject);
            canPlaySound = false;
        }
    }

    private void CheckIsUnderMap()
    {
        if (self.position.y < -1.0f)
        {
           Vector3 upPlayer = new Vector3(self.position.x, playerY, self.position.z);
           self.position = upPlayer;
        }            
    }
}