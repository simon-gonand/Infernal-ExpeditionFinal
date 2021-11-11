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
    public FixedJoint fixedJoint;
    [SerializeField]
    private PlayerPresets playerPreset;

    [Header("Children References")]
    public Transform playerGraphics;
    [SerializeField]
    private Transform attackPoint;

    public Animator anim;

    private Vector2 playerMovementInput = Vector2.zero;

    private IInteractable interactingWith;

    private float nextDash;
    private float nextAttack;

    private Treasure _transportedTreasure;
    public Treasure transportedTreasure { get { return _transportedTreasure; } set { _transportedTreasure = value; } }

    private Vector3 _movement;
    public Vector3 movement { get { return _movement; } set { _movement = value; } }

    #region booleans
    // Is the player interacting with something
    private bool _isInteracting = false;
    public bool isInteracting { get { return _isInteracting; } set { _isInteracting = value; } }

    // Is the player carrying a treasure
    private bool _isCarrying = false;
    public bool isCarrying { get { return _isCarrying; } set { _isCarrying = value; } }

    // Is the player on the boat
    private bool _isOnBoat = true;
    public bool isOnBoat { get { return _isOnBoat; } set { _isOnBoat = value; } }

    private bool _isSwimming = false;
    public bool isSwimming { set { _isSwimming = value; } }

    private bool _isInWater = false;
    public bool isInWater { set { _isInWater = value; } }

    private bool isStun = false;
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
        if (!_isSwimming)
        {
            // If the player is interacting with something he can't attack
            if ((_isInteracting || _isCarrying))
            {
                if (context.started)
                {
                    interactingWith.OnAction(this);
                }
                else if (context.canceled)
                {
                    if (_transportedTreasure != null)
                    {
                        _transportedTreasure.LaunchObject(this);
                    }
                }
            }
            else
            {
                if (context.performed && Time.time > nextAttack && !isStun)
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
        if (context.performed && Time.time > nextDash && !_isInteracting && !_isSwimming)
        {
            selfRigidBody.AddForce(self.forward * playerPreset.dashSpeed, ForceMode.Impulse);
            
            nextDash = Time.time + playerPreset.dashCooldown;
            StartCoroutine(DashTimer());
        }
    }

    IEnumerator DashTimer()
    {
        anim.SetBool("isDashing", true);
        yield return new WaitForSeconds(playerPreset.dashTime);
        selfRigidBody.velocity = Vector3.zero;
        anim.SetBool("isDashing", false);
    }

    // When the player pressed the interaction button
    public void OnInteraction(InputAction.CallbackContext context)
    {
        if (!_isSwimming)
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
                            interactingWith = hit.collider.gameObject.GetComponentInParent<IInteractable>();
                            if (!interactingWith.InteractWith(this, hit.collider.gameObject))
                                interactingWith = null;
                            break;
                        }
                    }
                }
            }
            // Else put the treasure down or uninteract with the interactable
            else if ((_isInteracting || _isCarrying) && context.performed)
            {
                interactingWith.UninteractWith(this);
            }
        }
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion

    private void Attack()
    {
        // Play attack animation
        Collider[] hit = Physics.OverlapSphere(attackPoint.position, playerPreset.attackRange);
        foreach(Collider hitted in hit)
        {
            // For sound design
            if (hitted.CompareTag("Enemy"))
            {
                Debug.Log("Enemy has been attacked");
                // Play enemy attacked sound
                // Damage enemy (enemy die animation)
                Destroy(hitted.gameObject);
            }
            if (hitted.CompareTag("Player"))
            {
                PlayerController attacked = hitted.GetComponent<PlayerController>();
                if (attacked != this && !attacked.isStun)
                {
                    StartCoroutine(attacked.StunWait());
                    attacked.movement = Vector3.zero;
                }
            }
            // etc...
        }
    }

    public IEnumerator StunWait()
    {
        isStun = true;
        // Update stun bool in animation for animation ?
        if (isCarrying)
        {
            _transportedTreasure.UninteractWith(this);
        }
        Debug.Log("Player is stun");
        yield return new WaitForSeconds(playerPreset.stunTime);
        Debug.Log("Player is not stun anymore");
        isStun = false;
        // Update stun bool in animation for animation ?
    }

    // Update movements of the player
    private void PlayerMovement()
    {
        float currentSpeed = playerPreset.playerGroundSpeed;
        // If player is swimming reduce speed
        if (_isSwimming && _isInWater)
            currentSpeed = playerPreset.playerSwimSpeed;
        // If player is in not deep water reduce speed
        else if (_isInWater)
            currentSpeed = playerPreset.playerInNotDeepWaterSpeed;
            
        // Apply speed malus if the player is carrying an heavy treasure
        if (_isCarrying)
            currentSpeed -= _transportedTreasure.speedMalus;

        // Apply movements
        Vector3 calculatePlayerInput = playerMovementInput * currentSpeed * Time.deltaTime;
        _movement = new Vector3(calculatePlayerInput.x, selfRigidBody.velocity.y,
            calculatePlayerInput.y);

        if (_isCarrying && _transportedTreasure.playerInteractingWith.Count > 1)
        {
            if ((_transportedTreasure.selfRigidbody.velocity.x < 0.1f || _transportedTreasure.selfRigidbody.velocity.x > 0.1f) ||
                (_transportedTreasure.selfRigidbody.velocity.z < 0.1f || _transportedTreasure.selfRigidbody.velocity.z > 0.1f))
            {
                _transportedTreasure.UpdatePlayerMovement(this, playerGraphics);
                _transportedTreasure.UpdatePlayerRotation(this, playerGraphics);
            }
        }
        else
        {
            if (_isCarrying)
                _transportedTreasure.UpdatePlayerRotation(this, playerGraphics);
            selfRigidBody.velocity = _movement;
        }

        // If velocity on Y is equal to 0.0 then it means that the player is swimming
        // if not then it means he must deal with gravity
        if (selfRigidBody.velocity.y > 0.5f && _isSwimming)
        {
            selfRigidBody.useGravity = true;
            Vector3 resetRotation = playerGraphics.eulerAngles;
            resetRotation.x = 0.0f;
            playerGraphics.eulerAngles = resetRotation;
            _isSwimming = false;
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
            

        // Set the rotation of the player according to his movements
        if (_movement.x != 0 || _movement.z != 0)
        {
            _movement.y = 0.0f;
            self.forward = _movement;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!isStun)
            PlayerMovement();
        InfoAnim();
    }

    void InfoAnim()
    {
        if (!isStun)
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
