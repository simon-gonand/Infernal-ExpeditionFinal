using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ClosingTutoUI : MonoBehaviour
{
    public static ClosingTutoUI Instance;

    public bool closeTuto = false;

    public GameObject billboardUIActivate;

    public void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
    }

    public void ResumeGame(InputAction.CallbackContext context)
    {
        if (closeTuto)
        {
            Time.timeScale = 1.0f;

            closeTuto = false;

            billboardUIActivate.SetActive(false);
            foreach (PlayerController p in PlayerManager.instance.players)
            {
                p.selfPlayerInput.currentActionMap.Disable();
                p.selfPlayerInput.SwitchCurrentActionMap("Controls");
                p.selfPlayerInput.currentActionMap.Enable();
            }
        }
    }
}
