using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoBillboardUi : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        foreach (PlayerController player in PlayerManager.instance.players)
        {
            player.closingTutoUI.billboardUIActivate = gameObject;
        }
    }
}
