using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Cinemachine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera mainMenuCam;
    [SerializeField]
    private Button firstSelected;

    private void Awake()
    {
        firstSelected.Select();
    }

    public void Play()
    {
        PlayerManager.instance.CheckInputs();
        mainMenuCam.Priority = 0;
        foreach (PlayerController player in PlayerManager.instance.players)
            player.GetComponent<PlayerInput>().currentActionMap.Enable();
        Cursor.visible = false;
        gameObject.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    private void Update()
    {
        PlayerManager.instance.CheckInputs();
    }
}
