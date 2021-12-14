using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarryPlayer : MonoBehaviour, ICarriable
{
    public PlayerController selfScript;

    [HideInInspector]public PlayerController carrier;

    private bool _isLoadingLaunch = false;
    public bool isLoadingLaunch { get { return _isLoadingLaunch; } }
    [HideInInspector]public float launchForce = 0.0f;

    public bool InteractWith(PlayerController carrier, GameObject interactingWith)
    {
        if (selfScript.isCarried || selfScript.isCarrying || selfScript.isInteracting) return false;
        carrier.isCarrying = true;
        carrier.carrying = this;
        this.carrier = carrier;
        selfScript.isCarried = true;
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
        player.selfRigidBody.velocity = Vector3.zero;
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

            if (carrier.playerMovementInput == Vector2.zero)
            {
                yield return new WaitForEndOfFrame();
                continue;
            }

            if (launchForce > selfScript.playerPreset.maxLaunchForce)
                launchForce = selfScript.playerPreset.maxLaunchForce;
            yield return new WaitForSeconds(offsetTime);
        }
    }

    public void UninteractWith(PlayerController player)
    {
        // It's dirty (UI)
        carrier.selfPlayerThrowUi.globaleConeCanvas.SetActive(false);

        player.isCarrying = false;
        player.isInteracting = false;
        player.carrying = null;
        selfScript.isCarried = false;
        selfScript.selfRigidBody.mass = 1;
        selfScript.selfRigidBody.isKinematic = false;
        selfScript.selfRigidBody.AddForce(player.self.forward * 2000.0f);
        selfScript.self.SetParent(null);

        selfScript.anim.SetBool("IsGettingCarried", false);

    }

    public void GetOnBoat(Transform entryPosition)
    {
        selfScript.isOnBoat = true;
    }

    public void GetOffBoat()
    {
        selfScript.isOnBoat = false;
    }

    private void StopLaunching()
    {
        _isLoadingLaunch = false;
        carrier.isLaunching = false;
        launchForce = 0.0f;
    }

    public void Launch(PlayerController player)
    {
        StopCoroutine(LoadingLaunchForce());
        if (isLoadingLaunch)
        {
            _isLoadingLaunch = false;

            Vector3 launchDirection = new Vector3(carrier.playerMovementInput.x, 0.0f, carrier.playerMovementInput.y);
            if (carrier.playerMovementInput == Vector2.zero)
            {
                StopLaunching();
                return;
            }

            // It's dirty (UI)
            carrier.selfPlayerThrowUi.globaleConeCanvas.SetActive(false);

            // Enable rigidbody
            selfScript.selfRigidBody.mass = 1;
            selfScript.selfRigidBody.isKinematic = false;
            selfScript.self.SetParent(player.self.parent);
            selfScript.selfRigidBody.AddForce((launchDirection + Vector3.up) * launchForce, ForceMode.Impulse);
            selfScript.isCarried = false;

            player.isCarrying = false;
            player.isInteracting = false;
            player.carrying = null;
            player.isLaunching = false;
            selfScript.hasBeenLaunched = true;

            // Is Carried animation
            selfScript.anim.SetBool("IsGettingCarried", false);

            // Update launched anim

            // Play Launch sound

            launchForce = 0.0f;
        }
    }
}
