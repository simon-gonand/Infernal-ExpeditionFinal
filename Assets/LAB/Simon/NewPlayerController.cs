using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NewPlayerController : MonoBehaviour
{
    public Transform self;
    public CharacterController controller;
    public Animator selfAnimator;
    public Transform playerGraphics;
    public CapsuleCollider collider;

    private Vector2 movementInputs;
    private Vector3 _movements;

    public bool isGrounded = false;

    #region Interaction

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInputs = context.ReadValue<Vector2>();
    } 

    #endregion

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateMovement();
        CheckIsGrounded();
        UpdateAnimation();
    }

    private void UpdateMovement()
    {
        _movements = new Vector3(movementInputs.x, 0.0f, movementInputs.y);
        if (!isGrounded)
            _movements.y = -7.5f;
        controller.Move(_movements * 5.0f * Time.deltaTime);
        if (!Mathf.Approximately(movementInputs.x, 0.0f) && !Mathf.Approximately(movementInputs.y, 0.0f))
        {
            _movements.y = 0.0f;
            self.forward = _movements;
            playerGraphics.forward = self.forward;
        }
    }

    private void CheckIsGrounded()
    {
        Vector3 startPos = self.position;
        startPos.y = self.position.y - collider.height / 2;
        isGrounded = Physics.CheckSphere(startPos, 1.0f, 1 << -1, QueryTriggerInteraction.Ignore);
    }

    private void UpdateAnimation()
    {
        if (movementInputs.x != 0 && movementInputs.y != 0)
        {
            selfAnimator.SetBool("isMoving", true);

            if (Mathf.Abs(movementInputs.x) > Mathf.Abs(movementInputs.y))
            {
                selfAnimator.SetFloat("playerSpeed", Mathf.Abs(movementInputs.x));
            }
            else
            {
                selfAnimator.SetFloat("playerSpeed", Mathf.Abs(movementInputs.y));
            }
        }
        else
            selfAnimator.SetBool("isMoving", false);
    }
}
