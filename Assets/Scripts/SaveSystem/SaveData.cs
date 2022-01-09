using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public List<LevelProfile> levels = new List<LevelProfile>(10);

    public int earnedStars = 0;
    
    private static SaveData _instance;
    public static SaveData instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new SaveData();
            }
            return _instance;
        }

        set
        {
            _instance = value;
        }
    }

    private SaveData()
    {
        for (int i = 0; i < 10; ++i)
        {
            levels.Add(new LevelProfile());
        }
    }

    public void CountStars()
    {
        earnedStars = 0;
        foreach(LevelProfile level in levels)
        {
            switch (level.starState)
            {
                case ScoreManager.differentStarState.Bronze:
                    earnedStars += 1;
                    break;
                case ScoreManager.differentStarState.Silver:
                    earnedStars += 2;
                    break;
                case ScoreManager.differentStarState.Gold:
                    earnedStars += 3;
                    break;
                default:
                    break;
            }
        }
    }

    public void ResetSave()
    {
        _instance = new SaveData();
    }
}
