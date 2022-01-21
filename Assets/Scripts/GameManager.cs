using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    
    public FollowPath followPath;
    public PlayerManager playerManager;
    public CinemachineTargetGroup targetGroup;

    public bool boatOnTargetGroup;

    [Header("Number Stars Needed to Unlock")]
    public List<int> neededStarsToUnlock;

    [Header("All treasures in scene")]
    public List<Treasure> treasuresInScene;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            new LevelManager();
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(transform.parent.gameObject);
        SceneManager.sceneLoaded += GetObjects;
    }

    public void LoadLevel(string sceneName, bool isBoatInScene)
    {
        string levelIdStr = sceneName.Substring(sceneName.Length - 1);
        try
        {
            LevelManager.instance.levelId = Convert.ToInt32(levelIdStr);
        }
        catch
        {
            LevelManager.instance.levelId = -1;
        }
        boatOnTargetGroup = isBoatInScene;
        if (sceneName.Equals("ÎleAuxPirates"))
        {
            PlayerManager.instance.onPirateIsland = true;
            PlayerManager.instance.onMainMenu = true;
            foreach (PlayerController p in PlayerManager.instance.players)
            {
                p.selfPlayerInput.currentActionMap.Disable();
            }
        }
        else
        {
            PlayerManager.instance.onPirateIsland = false;
            PlayerManager.instance.respawnOnBoat = true;
        }
        SceneManager.LoadScene(sceneName);
    }

    private void GetObjects(Scene scene, LoadSceneMode sceneMode)
    {
        GameObject virtualCam = GameObject.FindGameObjectWithTag("VirtualCamera");
        playerManager.cam = virtualCam.GetComponent<CinemachineVirtualCamera>();
        playerManager.camManager = virtualCam.GetComponent<CameraManager>();

        GameObject boatPath = GameObject.FindGameObjectWithTag("BoatPath");
        if (boatPath != null)
            followPath.path = boatPath.GetComponent<Path>();
        followPath.cam = virtualCam.GetComponent<CinemachineVirtualCamera>();
        followPath.InitializePath();
        targetGroup = FindObjectOfType<CinemachineTargetGroup>();

        if (PlayerManager.instance.onPirateIsland)
        {
            GameObject spawn1 = GameObject.FindGameObjectWithTag("PlayerMesh");
            GameObject spawn2 = GameObject.FindGameObjectWithTag("PlayerMesh2");
            GameObject spawn3 = GameObject.FindGameObjectWithTag("PlayerMesh3");
            GameObject spawn4 = GameObject.FindGameObjectWithTag("PlayerMesh4");

            PlayerManager.instance.player1Spawn = spawn1;
            PlayerManager.instance.player2Spawn = spawn2;
            PlayerManager.instance.player3Spawn = spawn3;
            PlayerManager.instance.player4Spawn = spawn4;
        }

        while (treasuresInScene.Count > 0)
        {
            if (treasuresInScene[0] != null)
                Destroy(treasuresInScene[0].gameObject);
            treasuresInScene.RemoveAt(0);
        }
        treasuresInScene.Clear();
        Treasure[] treasures = GameObject.FindObjectsOfType<Treasure>();
        foreach (Treasure treasure in treasures)
        {
            treasuresInScene.Add(treasure);
        }


        playerManager.OnChangeScene();
        BoatInTargetGroup();
        LevelManager.instance.StartLevel();
        GetStarsValue(PlayerManager.instance.players.Count, ScoreManager.instance.maxScore);
    }

    private void BoatInTargetGroup()
    {
        if (boatOnTargetGroup)
        {
            targetGroup.AddMember(BoatManager.instance.self, 25, 20);
        }
        else
        {
            targetGroup.RemoveMember(BoatManager.instance.self);
        }
    }


    public void GetStarsValue(int _numberOfPlayer , int _maxScore)
    {
        Debug.Log("Entering scoring setting function");

        List<Treasure> littleTreasurs = new List<Treasure>();
        List<Treasure> mediumTreasurs = new List<Treasure>();
        List<Treasure> bigTreasurs = new List<Treasure>();

        int littleValue = 0;
        int mediumValue = 0;
        int bigValue = 0;

        for (int i= 0; i < treasuresInScene.Count; i++)
        {
            Debug.Log("Processing chests draft...");

            if(treasuresInScene[i].category.maxPlayerCarrying == 4)
            {
                bigTreasurs.Add(treasuresInScene[i]);
                bigValue += treasuresInScene[i].price;
            }
            else if(treasuresInScene[i].category.maxPlayerCarrying == 2)
            {
                mediumTreasurs.Add(treasuresInScene[i]);
                mediumValue += treasuresInScene[i].price;
            }
            else if (treasuresInScene[i].category.maxPlayerCarrying == 1)
            {
                littleTreasurs.Add(treasuresInScene[i]);
                littleValue += treasuresInScene[i].price;
            }
        }
        Debug.Log("Chest Draft over ! Values :" + littleValue + " " + mediumValue + " " + bigValue);


        int soloGlobalValue = littleValue + ((mediumValue / 3) *2);
        int duoGlobalValue = littleValue + mediumValue + (bigValue/2);
        int trioGlobalValue = littleValue + mediumValue + ((bigValue / 3)*2);
        int quatuorGlobalValue = littleValue + mediumValue + bigValue;

        ScoreManager.instance.maxScore = quatuorGlobalValue;

        Debug.Log("Max scene score = " + _maxScore);

        if(_numberOfPlayer == 4)
        {
            ScoreManager.instance.scoreNeedForGold = (quatuorGlobalValue / 3) * 2;
        }
        else if (_numberOfPlayer == 3)
        {
            ScoreManager.instance.scoreNeedForGold = (trioGlobalValue / 3) * 2;
        }
        else if(_numberOfPlayer == 2)
        {
            ScoreManager.instance.scoreNeedForGold = (duoGlobalValue / 3) * 2;
        }
        else
        {
            ScoreManager.instance.scoreNeedForGold = (soloGlobalValue / 3) * 2;
        }

        ScoreManager.instance.scoreNeedForSilver = (ScoreManager.instance.scoreNeedForGold / 3) * 2;
        ScoreManager.instance.scoreNeedForBronze = (ScoreManager.instance.scoreNeedForGold / 3);


        Debug.Log("Score setting done !");
    }
}
