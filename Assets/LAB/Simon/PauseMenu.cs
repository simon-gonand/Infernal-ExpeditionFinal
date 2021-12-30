using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    public void RestartLevel()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        Resume();
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

    public void Option()
    {
        // TODO
    }

    public void Quit()
    {
        GameManager.instance.LoadLevel("BalanceZone", false);
    }
}
