using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Transform self;
    public Rigidbody selfRigidBody;
    public FixedJoint fixedJoint;
    [SerializeField]
    private PlayerPresets playerPreset;


    private Vector2 playerMovementInput = Vector2.zero;

    private IInteractable interactingWith;

    private float nextDash;

    private Treasure _transportedTreasure;
    public Treasure transportedTreasure { set { _transportedTreasure = value; } }

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
    #endregion

    // Start is called before the first frame update
    void Start()
    {
    }

    #region InputsManagement
    // When the player moves
    public void OnMove(InputAction.CallbackContext context)
    {
        playerMovementInput = context.ReadValue<Vector2>();
    }

    // When the player pressed the action button
    public void OnAction(InputAction.CallbackContext context)
    {
        // If the player is interacting with something he can't attack
        if ((_isInteracting || _isCarrying) && context.started)
            interactingWith.OnAction(this);
        if (context.canceled)
            Debug.Log("canceled");
        // else attack on action pressed
    }

    // When the player pressed the dash button
    public void OnDash(InputAction.CallbackContext context)
    {
        if (context.performed && Time.time > nextDash && !_isInteracting)
        {
            selfRigidBody.AddForce(self.forward * playerPreset.dashSpeed, ForceMode.Impulse);
            
            nextDash = Time.time + playerPreset.dashCooldown;
            StartCoroutine(DashTimer());
        }
    }

    IEnumerator DashTimer()
    {
        yield return new WaitForSeconds(playerPreset.dashTime);
        selfRigidBody.velocity = Vector3.zero;
    }

    // When the player pressed the interaction button
    public void OnInteraction(InputAction.CallbackContext context)
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
            interactingWith = null;
        }
    }

    public void OnReload(InputAction.CallbackContext context)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion

    // Update movements of the player
    private void PlayerMovement()
    {
        float currentSpeed = playerPreset.playerSpeed;
        // Apply speed malus if the player is carrying an heavy treasure
        if (_isCarrying)
            currentSpeed -= _transportedTreasure.speedMalus;

        // Apply movements
        Vector3 move = new Vector3(playerMovementInput.x, 0.0f, playerMovementInput.y);
        selfRigidBody.velocity = move * Time.fixedDeltaTime * currentSpeed;

        // Set the rotation of the player according to his movements
        if (move != Vector3.zero)
            self.forward = move;
    }

    private void ApplyGravity()
    {
        // Raycast the ground if player is on the boat
        Vector3 rayPos = self.position;
        rayPos.y -= self.lossyScale.y;
        if (!Physics.Raycast(rayPos, -Vector3.up, 0.05f))
        {
            Vector3 newVelocity = selfRigidBody.velocity;
            newVelocity.y = -playerPreset.gravity;
            selfRigidBody.velocity = newVelocity;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        PlayerMovement();
        ApplyGravity();
    }
}
