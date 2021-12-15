using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenGate : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject linkedGate;
    [SerializeField]
    private Transform snapPoint;
    [SerializeField]
    private float offsetClick;

    private PlayerController interactingPlayer = null;
    private float openGateYPosition;
    private bool isOpen = false;

    public bool InteractWith(PlayerController player, GameObject interactingWith)
    {
        if (interactingPlayer != null || isOpen) return false;
        interactingPlayer = player;
        player.self.position = new Vector3(snapPoint.position.x, player.self.position.y, snapPoint.position.z);
        return true;
    }

    public void OnAction(PlayerController player)
    {
        Vector3 newPosition = linkedGate.transform.position;
        newPosition.y -= offsetClick;
        linkedGate.transform.position = newPosition;
        if (openGateYPosition + newPosition.y < 0)
        {
            interactingPlayer.isInteracting = false;
            interactingPlayer.selfRigidBody.constraints = RigidbodyConstraints.FreezeRotation;
            interactingPlayer.selfRigidBody.mass = 1;
            UninteractWith(interactingPlayer);
            isOpen = true;
        }
    }

    public void UninteractWith(PlayerController player)
    {
        player.isInteracting = false;
        interactingPlayer = null;
    }

    // Start is called before the first frame update
    void Start()
    {
        Collider fenceCollider = linkedGate.GetComponent<Collider>();
        openGateYPosition = fenceCollider.bounds.size.y;
    }
}
