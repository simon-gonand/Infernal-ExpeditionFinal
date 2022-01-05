using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
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

    public List<LevelProfile> levels = new List<LevelProfile>(10);
}
