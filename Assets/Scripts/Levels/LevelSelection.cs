using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;

public class LevelSelection : MonoBehaviour
{
    public GameObject inputButtonA;

    public CinemachineVirtualCamera mainCam;
    public CinemachineVirtualCamera tableCam;

    [SerializeField]
    private UnlockedLevels levelSelection;

    private GameObject currentSelectedObject;
    private int currentLevelSelectedIndex;

    private List<PlayerController> playerWhoCanInteract = new List<PlayerController>();
    private bool _uiActivate = false;
    public bool uiActivate { get { return _uiActivate; } }

    // Start is called before the first frame update
    void Start()
    {
        _uiActivate = false;
        currentLevelSelectedIndex = 0;
    }

    private void Update()
    {
        if (_uiActivate)
        {
            inputButtonA.SetActive(false);
        }

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
        PlayerManager.instance.onLevelSelectionUI = false;
        foreach (PlayerController p in PlayerManager.instance.players)
        {
            p.selfPlayerInput.currentActionMap.Disable();
            p.selfPlayerInput.SwitchCurrentActionMap("Controls");
            p.selfPlayerInput.currentActionMap.Enable();
        }
        _uiActivate = false;
    }

    public void ChangeSelection(Vector2 value)
    {
        currentSelectedObject = EventSystem.current.currentSelectedGameObject;
        if (value.x < 0.0f)
        {
            // Move left
            --currentLevelSelectedIndex;
        }
        else if (value.x > 0.0f)
        {
            // Move right
            ++currentLevelSelectedIndex;
        }

        LevelProfile level = SaveData.instance.levels[currentLevelSelectedIndex];
        switch (level.starState)
        {
            case ScoreManager.differentStarState.NoStar:
                // Nique
                break;
            case ScoreManager.differentStarState.Bronze:
                // Display bronze
                break;
            case ScoreManager.differentStarState.Silver:
                // Display bronze et silver
                break;
            case ScoreManager.differentStarState.Gold:
                // Display bronze, silver et gold
                break;
            default:
                break;
        }
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
        {
            p.selfPlayerInput.currentActionMap.Disable();
            p.selfPlayerInput.SwitchCurrentActionMap("ControlsUI");
            p.selfPlayerInput.currentActionMap.Enable();
        }

        levelSelection.CheckLevelState();
        levelSelection.gameObject.SetActive(true);

        if (currentSelectedObject != null)
            EventSystem.current.SetSelectedGameObject(currentSelectedObject);

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
        if (!_uiActivate)
        {
            mainCam.Priority = 0;
            tableCam.Priority = 15;

            StartCoroutine(WaitForCameraMovementsToTableCam());

            _uiActivate = true;

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
