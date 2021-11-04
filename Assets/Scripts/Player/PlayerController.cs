using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public Transform self;
    public Rigidbody selfRigidBody;
    public Transform carryingSnapPoint;
    public FixedJoint fixedJoint;
    [SerializeField]
    private PlayerPresets playerPreset;

    public Transform boatTransform;


    private Vector2 playerMovementInput = Vector2.zero;

    private IInteractable interactingWith;
    private float nextDash;

    #region boolean
    private bool _isInteracting = false;
    public bool isInteracting { get { return _isInteracting; } set { _isInteracting = value; } }

    private bool _isCarrying = false;
    public bool isCarrying { get { return _isCarrying; } set { _isCarrying = value; } }
    private TreasuresCategory _treasureCarried;
    public TreasuresCategory treasureCarried { set { _treasureCarried = value; } }

    private bool _isOnBoat = true;
    public bool isOnBoat { get { return _isOnBoat; } set { _isOnBoat = value; } }

    private bool _isGrounded = false;
    public bool isGrounded { set { _isGrounded = value; } }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
    }

    #region InputsManagement
    // When the player moves
    public void OnMove(InputAction.CallbackContext context)
    {
        // If the player is interacting with something he can't move
        if (_isInteracting && interactingWith != null)
            interactingWith.OnMove(context.ReadValue<Vector2>());
            
        else
            playerMovementInput = context.ReadValue<Vector2>();
    }

    // When the player pressed the action button
    public void OnAction(InputAction.CallbackContext context)
    {
        // If the player is interacting with something he can't attack
        if ((_isInteracting || _isCarrying) && context.performed)
            interactingWith.OnAction();
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
            RaycastHit hit;
            int layerMask = 1 << LayerMask.NameToLayer("Interactable");
            if (Physics.Raycast(startRayPos, self.forward, out hit, playerPreset.interactionDistance, layerMask))
            {
                // Stop player's movements
                playerMovementInput = Vector2.zero;

                // Set with which interactable the player is interacting with
                interactingWith = hit.collider.gameObject.GetComponent<IInteractable>();
                interactingWith.InteractWith(this);
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
        if (_isCarrying && _treasureCarried != null)
            currentSpeed -= _treasureCarried.speedMalus;

        // Apply movements
        Vector3 move = new Vector3(playerMovementInput.x, 0.0f, playerMovementInput.y);
        self.Translate(move * Time.deltaTime * currentSpeed, Space.World);

        // Set the rotation of the player according to his movements
        if (move != Vector3.zero)
            self.forward = move;
    }

    private void UpdatePlayerOnBoat()
    {
        if (_isOnBoat && !_isGrounded)
        {
            // Raycast the ground if player is on the boat
            Vector3 rayPos = self.position;
            rayPos.y -= self.lossyScale.y;
            RaycastHit hit;
            if (Physics.Raycast(rayPos, -Vector3.up, out hit, 0.05f))
            {
                if (hit.collider.CompareTag("Boat"))
                {
                    // Set the zone as parent of the player to move with it
                    self.SetParent(hit.transform);

                    self.position = new Vector3(self.position.x, self.position.y - 0.05f, self.position.z);

                    // Update the constraints of the rigid body to avoid gravity
                    selfRigidBody.constraints = RigidbodyConstraints.FreezeRotation |
                        RigidbodyConstraints.FreezePositionY;

                    isGrounded = true;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        PlayerMovement();
        UpdatePlayerOnBoat();
    }
}
