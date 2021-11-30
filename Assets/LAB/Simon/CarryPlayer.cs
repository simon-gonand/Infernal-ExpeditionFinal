using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryPlayer : MonoBehaviour, IInteractable
{
    public PlayerController selfScript;

    public bool InteractWith(PlayerController carrier, GameObject interactingWith)
    {
        if (selfScript.isCarried || selfScript.isCarrying || selfScript.isInteracting) return false;
        carrier.isCarrying = true;
        Vector3 snapPosition = carrier.playerCarryingPoint.position;
        snapPosition.y += selfScript.self.lossyScale.y / 2;
        selfScript.self.position = snapPosition;
        selfScript.selfRigidBody.isKinematic = true;
        selfScript.self.forward = carrier.self.forward;
        selfScript.self.SetParent(carrier.self);
        // Carry player sound
        return true;
    }

    public void OnAction(PlayerController player)
    {
        throw new System.NotImplementedException();
    }

    public void UninteractWith(PlayerController player)
    {
        throw new System.NotImplementedException();
    }

    public void GetOnBoat()
    {
        selfScript.isOnBoat = true;
    }

    public void GetOffBoat()
    {
        selfScript.isOnBoat = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
