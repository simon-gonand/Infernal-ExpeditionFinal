using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectLevels : MonoBehaviour, IInteractable
{
    [SerializeField]
    private GameObject levelSelection;

    private PlayerController playerInteracting = null;

    public bool InteractWith(PlayerController player, GameObject interactingWith)
    {
        if (playerInteracting != null) return false;
        playerInteracting = player;
        levelSelection.SetActive(true);
        Cursor.visible = true;
        return true;
    }

    public void OnAction(PlayerController player)
    {
        // No action implementation
    }

    public void SelectLevel(int number)
    {
        GameManager.instance.LoadLevel("Level_0" + number, true);

        Back();
    }

    public string GetTag()
    {
        return gameObject.tag;
    }

    public void Back()
    {
        playerInteracting.selfRigidBody.constraints = RigidbodyConstraints.FreezeRotation;
        playerInteracting.selfRigidBody.mass = 1;
        playerInteracting.isInteracting = false;
        UninteractWith(playerInteracting);
    }

    public void UninteractWith(PlayerController player)
    {
        playerInteracting = null;
        levelSelection.SetActive(false);
        Cursor.visible = false;
    }
}
