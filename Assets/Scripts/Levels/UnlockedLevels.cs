using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class UnlockedLevels : MonoBehaviour
{
    [SerializeField]
    private List<Button> levelButtons;

    [SerializeField]
    private Color lockedColor;

    public void CheckLevelState()
    {
        levelButtons[0].Select();
        for (int i = 0; i < levelButtons.Count; ++i)
        {
            if (SaveData.instance.earnedStars < GameManager.instance.neededStarsToUnlock[i])
            {
                levelButtons[i].interactable = false;
                //levelButtons[i].GetComponentInChildren<TextMeshProUGUI>().color = lockedColor;
            }
        }
    }   
}