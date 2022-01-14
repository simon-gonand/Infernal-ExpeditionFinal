using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
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
        boatOnTargetGroup = false;
    }

    public void LoadLevel(string sceneName, bool isBoatInScene)
    {
        string levelIdStr = sceneName.Substring(sceneName.Length - 2);
        if (levelIdStr[0] == '0')
            levelIdStr = levelIdStr.Substring(1);
        try
        {
            LevelManager.instance.levelId = Convert.ToInt32(levelIdStr);
        }
        catch
        {
            LevelManager.instance.levelId = 0;
        }
        boatOnTargetGroup = isBoatInScene;
        if (sceneName.Equals(SceneManager.GetSceneAt(0).name))
            PlayerManager.instance.onPirateIsland = true;
        else
            PlayerManager.instance.onPirateIsland = false;
        SceneManager.LoadScene(sceneName);
    }

    private void GetObjects(Scene scene, LoadSceneMode sceneMode)
    {
        foreach (PlayerController player in PlayerManager.instance.players)
            player.GetComponent<PlayerInput>().currentActionMap.Enable();
        GameObject virtualCam = GameObject.FindGameObjectWithTag("VirtualCamera");
        playerManager.cam = virtualCam.GetComponent<CinemachineVirtualCamera>();
        playerManager.camManager = virtualCam.GetComponent<CameraManager>();

        GameObject boatPath = GameObject.FindGameObjectWithTag("BoatPath");
        if (boatPath != null)
            followPath.path = boatPath.GetComponent<Path>();
        followPath.cam = virtualCam.GetComponent<CinemachineVirtualCamera>();
        followPath.InitializePath();
        targetGroup = FindObjectOfType<CinemachineTargetGroup>();

        if (LevelManager.instance.levelModifiers)
        {
            treasuresInScene.Clear();
            Treasure[] treasures = GameObject.FindObjectsOfType<Treasure>();
            foreach (Treasure treasure in treasures)
            {
                treasuresInScene.Add(treasure);
            }
        }

        playerManager.OnChangeScene();
        BoatInTargetGroup();
        LevelManager.instance.StartLevel();
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
}
