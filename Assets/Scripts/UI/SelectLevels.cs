using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectLevels : MonoBehaviour, IInteractable
{
    [SerializeField]
    private UnlockedLevels levelSelection;

    private PlayerController playerInteracting = null;

    public bool InteractWith(PlayerController player, GameObject interactingWith)
    {
        if (playerInteracting != null) return false;
        
        SaveData.instance = (SaveData)SerializationManager.Load();
        int count = 1;
        foreach (LevelProfile profile in SaveData.instance.levels)
        {
            Debug.Log("---------------------- Level_" + count + " ----------------------");
            Debug.Log(profile.highScore);
            Debug.Log(profile.starState);
            ++count;
        }
        Debug.Log("------------------- Nb Stars -------------------");
        Debug.Log(SaveData.instance.earnedStars);

        playerInteracting = player;
        levelSelection.CheckLevelState();
        levelSelection.gameObject.SetActive(true);
        Cursor.visible = true;
        return true;
    }

    public void OnAction(PlayerController player)
    {
        // No action implementation
    }

    public void SelectLevel(int number)
    {
        if (number == 10)
            GameManager.instance.LoadLevel("Level_" + number, true);
        else
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
        levelSelection.gameObject.SetActive(false);
        Cursor.visible = false;
    }
}
