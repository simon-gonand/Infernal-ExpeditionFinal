using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;

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

    private void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0.0f;
        isPause = true;
    }

    private void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1.0f;
        isPause = false;
    }
}
