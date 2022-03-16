using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Cinemachine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera mainMenuCam;
    [SerializeField]
    private Button firstSelected;
    [SerializeField]
    private OptionMenu optionMenu;

    private Button lastSelected;

    private void Awake()
    {
        firstSelected.Select();
        firstSelected.GetComponent<MainMenuButton>().SetSelectAtStart();

        foreach(PlayerController player in PlayerManager.instance.players)
        {
            player.selfPlayerInput.currentActionMap.Disable();
            player.selfPlayerInput.SwitchCurrentActionMap("ControlsUI");
            player.selfPlayerInput.currentActionMap.Enable();
        }
    }

    public void Play()
    {
        PlayerManager.instance.CheckInputs();
        PlayerManager.instance.onMainMenu = false;
        mainMenuCam.Priority = 0;
        foreach (PlayerController player in PlayerManager.instance.players)
        {
            player.selfPlayerInput.currentActionMap.Disable();
            if (GameManager.instance.debugMode)
                player.selfPlayerInput.SwitchCurrentActionMap("DebugControls");
            else
                player.selfPlayerInput.SwitchCurrentActionMap("Controls");

            player.selfPlayerInput.currentActionMap.Enable();
        }
        Cursor.visible = false;
        gameObject.SetActive(false);
    }

    public void Option()
    {
        optionMenu.OpenMenu(gameObject, EventSystem.current.currentSelectedGameObject);
        gameObject.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void Update()
    {
        PlayerManager.instance.CheckInputs();
        if (EventSystem.current.currentSelectedGameObject != null)
            lastSelected = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        else
        {
            lastSelected.Select();
        }
    }
}
