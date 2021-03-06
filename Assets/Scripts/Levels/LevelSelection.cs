using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Cinemachine;

public class LevelSelection : MonoBehaviour
{
    public GameObject inputButtonA;

    private Coroutine coroutine;

    [Header("CAMERA")]
    public CinemachineVirtualCamera mainCam;
    public CinemachineVirtualCamera tableCam;

    [Header("SCORE TOKEN")]
    public GameObject tokenGroup;
    public GameObject bronzeToken;
    public GameObject silverToken;
    public GameObject goldToken;

    [Space(5)]
    public GameObject cursor;

    [Space(10)]
    [SerializeField]
    private UnlockedLevels levelSelection;
    [SerializeField]
    private Outline outline;

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
        LevelManager.instance.levelModifiers = false;
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
        AudioManager.AMInstance.lobbyThemeToClassicSWITCH.Post(AudioManager.AMInstance.gameObject);
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

        tokenGroup.SetActive(false);
    }

    private IEnumerator LerpTransition(float offset)
    {
        RectTransform cursorTransform = cursor.GetComponent<RectTransform>();
        tokenGroup.SetActive(false);

        Vector3 startPos = cursorTransform.localPosition;
        float t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime*2;
            cursorTransform.localPosition = Vector3.Lerp(startPos, new Vector3(startPos.x + offset, startPos.y, startPos.z),t);
            yield return null;
        }

        tokenGroup.SetActive(true);
        if (offset < 0.0f)
        {
            ++currentLevelSelectedIndex;
        }
        else if (offset > 0.0f)
            --currentLevelSelectedIndex;
        Debug.Log(currentLevelSelectedIndex);
        CheckStarsStates();

        AudioUpdateForBiome();

        coroutine = null;
        yield return null;
    }

    public void ChangeSelection(Vector2 value)
    {

        currentSelectedObject = EventSystem.current.currentSelectedGameObject;
        if (value.x < 0.0f)
        {
            if (currentLevelSelectedIndex == 0)
            {
                return;
            }

            if (coroutine == null)
            {
                // Move left
                coroutine = StartCoroutine(LerpTransition(+1288));
                AudioManager.AMInstance.menuNavigationSFX.Post(gameObject);
                
            }

        }
        else if (value.x > 0.0f)
        {
            if (currentLevelSelectedIndex == 5)
            {
                return;
            }

            if (coroutine == null)
            {
                // Move right
                coroutine = StartCoroutine(LerpTransition(-1288));
                AudioManager.AMInstance.menuNavigationSFX.Post(gameObject);
                
            }
        }
    }

    // Remove this function when we got all the levels
    private int DebugGetLevelIndex()
    {
        switch (currentLevelSelectedIndex)
        {
            case 0:
                return 0;
            case 1:
                return 1;
            case 2:
                return 4;
            case 3:
                return 5;
            case 4:
                return 7;
            case 5:
                return 8;
            default:
                return 0;
        }
    }

    public void SelectModifierLevel()
    {
        LevelManager.instance.levelModifiers = true;
        int number = DebugGetLevelIndex();
        if (number == 0)
            GameManager.instance.LoadLevel("LandScape_Tuto", false);
        else
            GameManager.instance.LoadLevel("Landscape_Level0" + number, true);
        Back();
    }

    private void CheckStarsStates()
    {
        LevelProfile level = SaveData.instance.levels[DebugGetLevelIndex()];
        switch (level.starState)
        {
            case ScoreManager.differentStarState.NoStar:
                bronzeToken.SetActive(false);
                silverToken.SetActive(false);
                goldToken.SetActive(false);
                break;
            case ScoreManager.differentStarState.Bronze:
                bronzeToken.SetActive(true);
                silverToken.SetActive(false);
                goldToken.SetActive(false);
                break;
            case ScoreManager.differentStarState.Silver:
                bronzeToken.SetActive(true);
                silverToken.SetActive(true);
                goldToken.SetActive(false);

                break;
            case ScoreManager.differentStarState.Gold:
                bronzeToken.SetActive(true);
                silverToken.SetActive(true);
                goldToken.SetActive(true);
                break;
            default:
                break;
        }
    }

    private void AwakeUI()
    {
        SaveData.instance = (SaveData)SerializationManager.Load();

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

        tokenGroup.SetActive(true);
        CheckStarsStates();
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
            outline.enabled = true;
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
            outline.enabled = false;
        }
    }



    void AudioUpdateForBiome()
    {


        if (currentLevelSelectedIndex <= 1)
        {
            AudioManager.AMInstance.lobbyThemeToClassicSWITCH.Post(AudioManager.AMInstance.gameObject);
        }
        else if (currentLevelSelectedIndex <= 3)
        {
            AudioManager.AMInstance.lobbyThemeToCitySWITCH.Post(AudioManager.AMInstance.gameObject);
        }
        else if (currentLevelSelectedIndex <= 4)
        {
            AudioManager.AMInstance.lobbyThemeToBaySWITCH.Post(AudioManager.AMInstance.gameObject);
        }
        else
        {
            AudioManager.AMInstance.lobbyThemeToClassicSWITCH.Post(AudioManager.AMInstance.gameObject);
        }

    }
}
