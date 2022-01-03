using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField]
    private CinemachineVirtualCamera mainMenuCam;

    public void Play()
    {
        mainMenuCam.Priority = 0;
        foreach (PlayerController player in PlayerManager.instance.players)
            player.GetComponent<PlayerInput>().currentActionMap.Enable();
        gameObject.SetActive(false);
    }

    public void Quit()
    {
        
    }
}
