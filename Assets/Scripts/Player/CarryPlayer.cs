using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryPlayer : MonoBehaviour, ICarriable
{
    public PlayerController selfScript;

    private bool _isLoadingLaunch = false;
    public bool isLoadingLaunch { get { return _isLoadingLaunch; } }
    private float launchForce = 0.0f;

    public bool InteractWith(PlayerController carrier, GameObject interactingWith)
    {
        if (selfScript.isCarried || selfScript.isCarrying || selfScript.isInteracting) return false;
        carrier.isCarrying = true;
        carrier.carrying = this;
        selfScript.isCarried = true;
        Vector3 snapPosition = carrier.playerCarryingPoint.position;
        snapPosition.y += selfScript.self.lossyScale.y / 2;
        selfScript.self.position = snapPosition;
        selfScript.selfRigidBody.isKinematic = true;
        selfScript.self.forward = carrier.self.forward;
        selfScript.self.SetParent(carrier.self);

        // Is Carried animation
        // Is Carrying animation

        // Carry player sound

        return true;
    }

    public void OnAction(PlayerController player)
    {
        _isLoadingLaunch = true;
        StartCoroutine(LoadingLaunchForce());
    }

    IEnumerator LoadingLaunchForce()
    {
        // Increase every 0.1 seconds
        float offsetTime = 0.1f;
        // Calculate how many the launch force will increase every 0.1 seconds
        float offsetLaunch = selfScript.playerPreset.maxLaunchForce * offsetTime / selfScript.playerPreset.fullChargeTime;
        while (isLoadingLaunch && launchForce != selfScript.playerPreset.maxLaunchForce)
        {
            launchForce += offsetLaunch;
            if (launchForce > selfScript.playerPreset.maxLaunchForce)
                launchForce = selfScript.playerPreset.maxLaunchForce;
            Debug.Log(launchForce);
            yield return new WaitForSeconds(offsetTime);
        }
    }

    public void UninteractWith(PlayerController player)
    {
        player.isCarrying = false;
        player.carrying = null;
        selfScript.isCarried = false;
        selfScript.selfRigidBody.mass = 1;
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
        if (isLoadingLaunch)
        {
            _isLoadingLaunch = false;

            // Enable rigidbody
            selfScript.selfRigidBody.mass = 1;
            selfScript.selfRigidBody.isKinematic = false;
            selfScript.self.SetParent(player.self.parent);
            selfScript.selfRigidBody.AddForce((player.self.forward + player.self.up) * launchForce, ForceMode.Impulse);
            selfScript.isCarried = false;

            player.isCarrying = false;
            player.carrying = null;
            selfScript.hasBeenLaunched = true;

            // Update launched anim

            // Play Launch sound

            launchForce = 0.0f;
        }
    }
}
