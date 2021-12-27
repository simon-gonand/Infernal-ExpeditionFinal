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

    public bool boatOnTargetGroup = false;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(transform.parent.gameObject);
        SceneManager.sceneLoaded += GetObjects;
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
        followPath.path.InitializePath();
        followPath.InitializePath();
        targetGroup = FindObjectOfType<CinemachineTargetGroup>();

        playerManager.OnChangeScene();
        BoatInTargetGroup();
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
