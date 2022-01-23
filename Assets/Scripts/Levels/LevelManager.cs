using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelManager
{
    public static LevelManager instance;

    public int levelId = -1;
    public bool levelModifiers;

    public LevelManager()
    {
        if (instance == null) instance = this;
        else return;
        levelModifiers = true;
    }

    public void StartLevel()
    {
        AudioManager.AMInstance.gameplayNavigationSWITCH.Post(AudioManager.AMInstance.gameObject);

        if (SceneManager.GetActiveScene().buildIndex <= 1)
        {
            AudioManager.AMInstance.lobbiTheme.Post(AudioManager.AMInstance.gameObject);
        }
        else if(SceneManager.GetActiveScene().buildIndex == 5)
        {
            AudioManager.AMInstance.run03Theme.Post(AudioManager.AMInstance.gameObject);
        }
        else
        {
            AudioManager.AMInstance.runTheme.Post(AudioManager.AMInstance.gameObject);
        }


        if (levelId == -1)
            Cursor.visible = true;
        else
        {
            Cursor.visible = false;
        }
            
        if (levelId == 7)
        {
            foreach (PlayerController player in PlayerManager.instance.players)
            {
                player.selfPlayerInput.currentActionMap.Disable();
            }
        }
    }

    public void EndLevel()
    {
        AudioManager.AMInstance.mapCompletedSWITCH.Post(AudioManager.AMInstance.gameObject);

        EndLevelUI.instance.InitializeUI();
        Cursor.visible = true;


        foreach (PlayerController player in PlayerManager.instance.players)
            player.GetComponent<PlayerInput>().currentActionMap.Disable();
        if (levelId == -1) return;
        if (SaveData.instance.levels[levelId].highScore < ScoreManager.instance.actualScore)
        {
            SaveData.instance.levels[levelId].highScore = ScoreManager.instance.actualScore;
            SaveData.instance.levels[levelId].starState = ScoreManager.instance.actualStar;
            SaveData.instance.CountStars();
        }
        SerializationManager.Save(SaveData.instance);
    }
}
