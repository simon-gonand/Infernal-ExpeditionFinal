using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryPlayer : MonoBehaviour, ICarriable
{
    public PlayerController selfScript;

    [HideInInspector]public PlayerController carrier;

    private bool _isLoadingLaunch = false;
    public bool isLoadingLaunch { get { return _isLoadingLaunch; } }

    public bool InteractWith(PlayerController carrier, GameObject interactingWith)
    {
        if (selfScript.isCarried || selfScript.isCarrying || selfScript.isInteracting) return false;
        carrier.isCarrying = true;
        carrier.carrying = this;
        this.carrier = carrier;
        selfScript.isCarried = true;
        if (selfScript.isSwimming)
        {
            selfScript.selfRigidBody.velocity += Vector3.up;
            selfScript.UpdateSwimming();
        }
        Vector3 snapPosition = carrier.playerCarryingPoint.position;
        snapPosition.y += selfScript.self.lossyScale.y / 2;
        selfScript.self.position = snapPosition;
        selfScript.selfRigidBody.isKinematic = true;
        selfScript.self.forward = carrier.self.forward;
        selfScript.self.SetParent(carrier.self);

        // Is Carried animation
        selfScript.anim.SetTrigger("startGettingCarried");
        selfScript.anim.SetBool("IsGettingCarried", true);

        // Is Carrying animation

        // Play Carry Sound
        AudioManager.AMInstance.playerCarrySFX.Post(gameObject);

        return true;
    }

    public void OnAction(PlayerController player)
    {
        _isLoadingLaunch = true;
    }

    public void UninteractWith(PlayerController player)
    {
        // It's dirty (UI)
        carrier.selfPlayerThrowUi.globaleConeCanvas.SetActive(false);

        player.isCarrying = false;
        player.isInteracting = false;
        player.carrying = null;
        player.isLaunching = false;

        _isLoadingLaunch = false;

        selfScript.isCarried = false;
        selfScript.selfRigidBody.mass = 1;
        selfScript.selfRigidBody.isKinematic = false;
        if (selfScript.isStun)
            selfScript.selfRigidBody.AddForce(player.self.forward * 250.0f);
        else 
            selfScript.selfRigidBody.AddForce(player.self.forward * 5000.0f);
        selfScript.self.SetParent(null);

        selfScript.anim.SetBool("IsGettingCarried", false);

        carrier = null;
    }

    public void GetOnBoat(Transform entryPosition)
    {
        selfScript.isOnBoat = true;
        AudioManager.AMInstance.playersOnBoat.Add(selfScript);
    }

    public void GetOffBoat()
    {
        selfScript.isOnBoat = false;
        AudioManager.AMInstance.playersOnBoat.Remove(selfScript);
    }

    public void Launch(PlayerController player)
    {
        if (isLoadingLaunch)
        {
            _isLoadingLaunch = false;

            // It's dirty (UI)
            carrier.selfPlayerThrowUi.globaleConeCanvas.SetActive(false);

            // Enable rigidbody
            carrier.selfRigidBody.mass = 1;

            selfScript.selfRigidBody.isKinematic = false;
            selfScript.self.SetParent(null);
            Physics.IgnoreCollision(selfScript.selfCollider, carrier.selfCollider, true);
            selfScript.selfRigidBody.AddForce((carrier.playerThrowDir + Vector3.up).normalized * selfScript.playerPreset.maxLaunchForce, ForceMode.Impulse);
            selfScript.isCarried = false;

            player.isCarrying = false;
            player.isInteracting = false;
            player.isLaunching = false;
            selfScript.hasBeenLaunched = true;

            // Is Carried animation
            selfScript.anim.SetBool("IsGettingCarried", false);

            // Update launched anim

            // Play Launch Sound
            AudioManager.AMInstance.playerThrowSFX.Post(gameObject);
        }
    }

    public void StopFall()
    {
        selfScript.hasBeenLaunched = false;
        Physics.IgnoreCollision(selfScript.selfCollider, carrier.selfCollider, false);
        carrier = null;
    }

    public string GetTag()
    {
        return gameObject.tag;
    }
}
