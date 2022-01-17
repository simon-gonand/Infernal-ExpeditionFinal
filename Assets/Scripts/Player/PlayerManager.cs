using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerManager : MonoBehaviour
{
    [Header("Player Prefabs")]
    [SerializeField]
    private GameObject player2;
    [SerializeField]
    private GameObject player3;
    [SerializeField]
    private GameObject player4;

    [Header("Pirate's Island player spawns")]
    public GameObject firstPlayer;
    public GameObject player1Spawn;
    public GameObject player2Spawn;
    public GameObject player3Spawn;
    public GameObject player4Spawn;

    [Header("Self Reference")]
    public PlayerInputManager self;

    [Header("External References")]
    [HideInInspector] public CinemachineVirtualCamera cam;
    [HideInInspector] public CameraManager camManager;

    [Header("PlayerStats")]
    public float deadZoneOffsetX = 10.0f;
    public float deadZoneOffsetY = 10.0f;
    public float weight;

    [Space]
    public bool onPirateIsland = true;
    public bool onMainMenu = true;

    private List<PlayerController> _players = new List<PlayerController>();
    public List<PlayerController> players { get { return _players; } }

    private float cameraOriginalOffset;
    Coroutine coroutine;

    private bool _respawnOnBoat;
    public bool respawnOnBoat { set { _respawnOnBoat = value; } get { return _respawnOnBoat; } }
    [HideInInspector] public Vector3 respawnPoint;

    public static PlayerManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            firstPlayer.SetActive(true);
            firstPlayer.transform.SetParent(BoatManager.instance.transform.parent);
        }
        else Destroy(gameObject);
    }

    private void Start()
    {
        cameraOriginalOffset = camManager.offsetPositionMovement;
        _respawnOnBoat = true;
    }

    // Update material of player when one is joining to avoid them to have the same color
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        // Update players spawn positions according to which player is spawning
        // Player is spawning on the boat
        Transform playerTransform = playerInput.gameObject.transform;
        Vector3 playerSpawnPosition = SetPlayerPosition(playerInput.playerIndex);
        if (onPirateIsland && playerInput.playerIndex == 0)
            playerSpawnPosition = playerTransform.position;
        playerTransform.position = playerSpawnPosition;
        if (!onPirateIsland && respawnOnBoat)
        {
            playerTransform.SetParent(BoatManager.instance.self);
        }
        else if (onMainMenu)
        {
            playerInput.currentActionMap.Disable();
        }
        if (onPirateIsland)
        {
            playerTransform.SetParent(BoatManager.instance.self.parent);
        }
        if (GameManager.instance != null)
            GameManager.instance.targetGroup.AddMember(playerTransform, weight, 20);
        PlayerController player = playerInput.gameObject.GetComponent<PlayerController>();
        player.id = playerInput.playerIndex;
        _players.Add(player);

        AudioManager.AMInstance.playerRespawnSFX.Post(gameObject);
    }

    private Vector3 SetPlayerPosition(int id)
    {
        Vector3 playerSpawnPosition = BoatManager.instance.spawnPoint1.position;
        if (onPirateIsland)
            playerSpawnPosition = player1Spawn.transform.position;
        switch (id)
        {
            case 0:
                self.playerPrefab = player2;
                break;
            case 1:
                if (onPirateIsland)
                {
                    playerSpawnPosition = player2Spawn.transform.position;
                    Destroy(player2Spawn);
                }
                else
                    playerSpawnPosition = BoatManager.instance.spawnPoint2.position;
                self.playerPrefab = player3;
                break;
            case 2:
                if (onPirateIsland)
                {
                    playerSpawnPosition = player3Spawn.transform.position;
                    Destroy(player3Spawn);
                }
                else
                    playerSpawnPosition = BoatManager.instance.spawnPoint3.position;
                self.playerPrefab = player4;
                break;
            case 3:
                if (onPirateIsland)
                {
                    playerSpawnPosition = player4Spawn.transform.position;
                    Destroy(player4Spawn);
                }
                else
                    playerSpawnPosition = BoatManager.instance.spawnPoint4.position;
                break;
            default:
                break;
        }
        if (_respawnOnBoat || onPirateIsland)
            return playerSpawnPosition;
        else
            return respawnPoint;
    }

    private void SetZoomSpeed(PlayerController player)
    {
        Vector3 distance = player.self.position - BoatManager.instance.self.position;
        float time = cam.GetMaxDampTime() / distance.magnitude;
        camManager.offsetPositionMovement = cameraOriginalOffset;
        camManager.CalculateOffsetRotationMovement();
        if (coroutine != null)
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(IncreaseZoomSpeed(time));
    }

    IEnumerator IncreaseZoomSpeed(float time)
    {
        camManager.offsetPositionMovement *= 5;
        camManager.CalculateOffsetRotationMovement();
        yield return new WaitForSeconds(time);
        camManager.offsetPositionMovement = cameraOriginalOffset;
        camManager.CalculateOffsetRotationMovement();
    }

    public void OnChangeScene()
    {
        for (int i = 0; i < players.Count; ++i)
        {
            PlayerController player = players[i];
            player.ResetPlayer();
            player.self.position = SetPlayerPosition(i);
            GameManager.instance.targetGroup.AddMember(player.self, weight, 20);
        }
    }

    public void CheckInputs()
    {
        if (_players.Count == 1)
        {
            if (Gamepad.current != null && Gamepad.current.aButton.wasPressedThisFrame)
                _players[0].GetComponent<PlayerInput>().SwitchCurrentControlScheme(Gamepad.current);

            if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
                _players[0].GetComponent<PlayerInput>().SwitchCurrentControlScheme(Keyboard.current, Mouse.current);
        }
    }

    private void Update()
    {
        if (_players.Count == 0) return;
        bool playerIsOutCam = false;
        foreach (PlayerController player in _players)
        {
            Vector2 posScreen = Camera.main.WorldToScreenPoint(player.self.position);
            if (posScreen.y > Camera.main.pixelHeight || posScreen.x > Camera.main.pixelWidth || posScreen.y < 0 || posScreen.x < 0)
            {
                camManager.isUnzooming = true;
                if (camManager.isUnzoomMax)
                {
                    if (posScreen.y < -deadZoneOffsetY || posScreen.x < -deadZoneOffsetX ||
                        posScreen.x > Camera.main.pixelWidth + deadZoneOffsetX || posScreen.y > Camera.main.pixelHeight + deadZoneOffsetY)
                    {
                        SetZoomSpeed(player);
                        player.Die();
                    }
                }
                camManager.offsetPositionMovement = cameraOriginalOffset;
                camManager.CalculateOffsetRotationMovement();
                if (coroutine != null)
                {
                    StopCoroutine(coroutine);
                    coroutine = null;
                }
                playerIsOutCam = true;
            }
        }
        if (!playerIsOutCam)
            camManager.isUnzooming = false;
    }
}