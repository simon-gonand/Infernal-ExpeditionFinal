using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private GameObject pauseMenuUI;
    [SerializeField]
    private OptionMenu optionMenuUI;
    [SerializeField]
    private Button firstSelected;
    private Button lastSelected;

    private bool isPause = false;

    public static PauseMenu instance;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    public void PauseGame()
    {
        if (isPause)
        {
            Resume();
        }
        else
        {
            Pause();
        }
    }

    public void RestartLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        GameManager.instance.LoadLevel(scene.name, GameManager.instance.boatOnTargetGroup);
        foreach(PlayerController player in PlayerManager.instance.players)
        {
            player.selfPlayerInput.currentActionMap.Enable();
        }
        Resume();
    }

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        firstSelected.Select();
        Cursor.visible = true;
        Time.timeScale = 0.0f;
        isPause = true;
        AudioManager.AMInstance.menuSelectSFX.Post(gameObject);
    }

    private void Resume()
    {
        pauseMenuUI.SetActive(false);
        Cursor.visible = false;
        Time.timeScale = 1.0f;
        isPause = false;
        AudioManager.AMInstance.menuCancelSFX.Post(gameObject);
    }

    public void Option()
    {
        optionMenuUI.OpenMenu(pauseMenuUI, EventSystem.current.currentSelectedGameObject);
        pauseMenuUI.SetActive(false);

        AudioManager.AMInstance.menuSelectSFX.Post(gameObject);
    }

    public void Quit()
    {
        GameManager.instance.LoadLevel("ÎleAuxPirates", false);
        Resume();
    }

    private void Update()
    {
        if (pauseMenuUI.activeSelf)
        {
            if (EventSystem.current.currentSelectedGameObject != null)
                lastSelected = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
            else
            {
                lastSelected.Select();
            }
        }
    }
}
