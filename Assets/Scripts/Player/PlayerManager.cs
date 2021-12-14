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

    [Header("Self Reference")]
    [SerializeField]
    private PlayerInputManager self;

    [Header("External References")]
    public CinemachineVirtualCamera cam;
    public CinemachineTargetGroup targetGroup;
    public CameraManager camManager;

    [Header("PlayerStats")]
    public float deadZoneOffsetX = 10.0f;
    public float deadZoneOffsetY = 10.0f;
    public float weight;

    private List<PlayerController> _players = new List<PlayerController>();
    public List<PlayerController> players { get { return _players; } }

    private float cameraOriginalOffset;
    Coroutine coroutine;

    private void Awake()
    {
        cameraOriginalOffset = camManager.offsetPositionMovement;
    }

    // Update material of player when one is joining to avoid them to have the same color
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        float playerSpawnOffset = 0.0f;
        switch (playerInput.playerIndex)
        {
            case 0:
                self.playerPrefab = player2;
                break;
            case 1:
                self.playerPrefab = player3;
                playerSpawnOffset = 0.5f;
                break;
            case 2:
                self.playerPrefab = player4;
                playerSpawnOffset = -0.5f;
                break;
            case 3:
                playerSpawnOffset = 1.0f;
                break;
            default:
                break;
        }

        // Update players spawn positions according to which player is spawning
        // Player is spawning on the boat
        Transform playerTransform = playerInput.gameObject.transform;
        Vector3 playerSpawnPosition = BoatManager.instance.spawnPoint.position;
        playerSpawnPosition.y += playerTransform.lossyScale.y;
        playerSpawnPosition.z += playerInput.playerIndex * playerSpawnOffset;
        playerTransform.position = playerSpawnPosition;
        targetGroup.AddMember(playerTransform, weight, 20);
        playerTransform.SetParent(BoatManager.instance.self);
        _players.Add(playerInput.gameObject.GetComponent<PlayerController>());
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
