using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryPlayer : MonoBehaviour, ICarriable
{
    public PlayerController selfScript;

    public bool InteractWith(PlayerController carrier, GameObject interactingWith)
    {
        if (selfScript.isCarried || selfScript.isCarrying || selfScript.isInteracting) return false;
        carrier.isCarrying = true;
        carrier.carrying = this;
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
        Debug.Log("Loading launch");
    }

    public void UninteractWith(PlayerController player)
    {
        player.isCarrying = false;
        player.carrying = null;
        selfScript.isCarried = false;
        selfScript.selfRigidBody.isKinematic = false;
        selfScript.selfRigidBody.AddForce(player.self.forward * 2000.0f);
        selfScript.self.SetParent(null);
    }

    public void GetOnBoat(Transform entryPosition)
    {
        selfScript.isOnBoat = true;
    }

    public void GetOffBoat()
    {
        selfScript.isOnBoat = false;
    }

    public void Launch(PlayerController player)
    {
        Debug.Log("Launch");
    }
}
