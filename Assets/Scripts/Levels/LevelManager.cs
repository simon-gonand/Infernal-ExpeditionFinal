using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class LevelManager
{
    public static LevelManager instance;

    public int levelId = 0;
    public bool levelModifiers;

    public LevelManager()
    {
        if (instance == null) instance = this;
        else return;
        levelModifiers = true;
    }

    public void StartLevel()
    {
        //Cursor.visible = false;
    }

    public void EndLevel()
    {
        EndLevelUI.instance.InitializeUI();
        Cursor.visible = true;
        foreach (PlayerController player in PlayerManager.instance.players)
            player.GetComponent<PlayerInput>().currentActionMap.Disable();
        if (levelId == 0) return;
        if (SaveData.instance.levels[levelId - 1].highScore < ScoreManager.instance.actualScore)
        {
            SaveData.instance.levels[levelId - 1].highScore = ScoreManager.instance.actualScore;
            SaveData.instance.levels[levelId - 1].starState = ScoreManager.instance.actualStar;
            SaveData.instance.CountStars();
        }
        SerializationManager.Save(SaveData.instance);
    }
}
