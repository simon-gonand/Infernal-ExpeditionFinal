using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class LevelSelection : MonoBehaviour
{
    public GameObject inputButtonA;

    public CinemachineVirtualCamera mainCam;
    public CinemachineVirtualCamera tableCam;

    [SerializeField]
    private UnlockedLevels levelSelection;

    private List<PlayerController> playerWhoCanInteract = new List<PlayerController>();
    private bool uiActivate = false;

    // Start is called before the first frame update
    void Start()
    {
        uiActivate = false;
    }

    public void SelectLevel(int number)
    {
        if (number == 0)
            GameManager.instance.LoadLevel("LandScape_Tuto", false);
        else
            GameManager.instance.LoadLevel("Landscape_Level0" + number, true);
        Back();
    }

    public string GetTag()
    {
        return gameObject.tag;
    }

    private void ResumeGame()
    {
        PlayerManager.instance.onLevelSelectionUI = false;
    }

    private IEnumerator WaitForCameraMovementsToMainCam()
    {
        while (Camera.main.transform.position != mainCam.transform.position)
            yield return new WaitForEndOfFrame();
        ResumeGame();
    }

    public void Back()
    {
        mainCam.Priority = 15;
        tableCam.Priority = 0;

        StartCoroutine(WaitForCameraMovementsToMainCam());

        levelSelection.gameObject.SetActive(false);
        Cursor.visible = false;
        foreach (PlayerController p in PlayerManager.instance.players)
            p.GetComponent<PlayerInput>().currentActionMap.Enable();
        uiActivate = false;
    }

    private void AwakeUI()
    {
        SaveData.instance = (SaveData)SerializationManager.Load();
        int count = 1;
        foreach (LevelProfile profile in SaveData.instance.levels)
        {
            Debug.Log("---------------------- Level_" + count + " ----------------------");
            Debug.Log(profile.highScore);
            Debug.Log(profile.starState);
            ++count;
        }
        Debug.Log("------------------- Nb Stars -------------------");
        Debug.Log(SaveData.instance.earnedStars);

        foreach (PlayerController p in PlayerManager.instance.players)
            p.GetComponent<PlayerInput>().currentActionMap.Disable();

        levelSelection.CheckLevelState();
        levelSelection.gameObject.SetActive(true);

        AudioManager.AMInstance.mapOpeningSFX.Post(gameObject);

        Cursor.visible = true;
    }

    private IEnumerator WaitForCameraMovementsToTableCam()
    {
        while (Camera.main.transform.position != tableCam.transform.position)
            yield return new WaitForEndOfFrame();

        AwakeUI();
    }

    public void ActivateLevelSelectionUi()
    {
        if (!uiActivate)
        {
            mainCam.Priority = 0;
            tableCam.Priority = 15;

            StartCoroutine(WaitForCameraMovementsToTableCam());

            uiActivate = true;

            PlayerManager.instance.onLevelSelectionUI = true;
            foreach(PlayerController player in PlayerManager.instance.players)
            {
                player.selfPlayerInput.currentActionMap.Disable();
            }

            Debug.Log("LaunchLevelUI");

            return;
        }        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inputButtonA.SetActive(true);
            PlayerController player = other.GetComponent<PlayerController>();
            playerWhoCanInteract.Add(player);
            player.levelSelectionTable = this;
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inputButtonA.SetActive(false);
            PlayerController player = other.GetComponent<PlayerController>();
            playerWhoCanInteract.Remove(player);
            player.levelSelectionTable = null;
        }
    }
}
