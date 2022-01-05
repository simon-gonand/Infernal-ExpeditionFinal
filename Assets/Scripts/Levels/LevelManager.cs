using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager
{
    public static LevelManager instance;

    public int levelId = 0;

    public LevelManager()
    {
        if (instance == null) instance = this;
        else return;
    }

    public void StartLevel()
    {
        
    }

    public void EndLevel()
    {
        EndLevelUI.instance.InitializeUI();
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
