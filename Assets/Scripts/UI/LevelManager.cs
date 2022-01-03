using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager
{
    public static LevelManager instance;

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
        Debug.Log("fini");
    }
}
