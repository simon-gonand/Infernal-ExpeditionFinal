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
        return true;
    }

    public void OnAction(PlayerController player)
    {
        // No action implementation
    }

    public void SelectLevel(int number)
    {
        switch (number)
        {
            case 1:
                //GameManager.instance.LoadLevel("Level_0" + number, true);
                break;
            case 2:
                //GameManager.instance.LoadLevel("Level_0" + number, true);
                break;
            case 3:
                //GameManager.instance.LoadLevel("Level_0" + number, true);
                break;
            case 4:
                GameManager.instance.LoadLevel("Level_0" + number, true);
                break;
            case 5:
                //GameManager.instance.LoadLevel("Level_0" + number, true);
                break;
            case 6:
                //GameManager.instance.LoadLevel("Level_0" + number, true);
                break;
            case 7:
                //GameManager.instance.LoadLevel("Level_0" + number, true);
                break;
            case 8:
                //GameManager.instance.LoadLevel("Level_0" + number, true);
                break;
            case 9:
                //GameManager.instance.LoadLevel("Level_0" + number, true);
                break;
            case 10:
                //GameManager.instance.LoadLevel("Level_0" + number, true);
                break;
            default:
                break;
        }

        UninteractWith(playerInteracting);
        playerInteracting.isInteracting = false;
    }

    public string GetTag()
    {
        return gameObject.tag;
    }

    public void UninteractWith(PlayerController player)
    {
        playerInteracting = null;
        levelSelection.SetActive(false);
    }
}
