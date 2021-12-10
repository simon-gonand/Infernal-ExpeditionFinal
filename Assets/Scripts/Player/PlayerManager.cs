using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerManager : MonoBehaviour
{
    [Header("Player Materials")]
    [SerializeField]
    private GameObject player2;
    [SerializeField]
    private GameObject player3;
    [SerializeField]
    private GameObject player4;

    [SerializeField]
    private PlayerInputManager inputMan;

    [Header("External References")]
    public CinemachineTargetGroup targetGroup;
    public CameraManager camManager;

    [Header("PlayerStats")]
    public float deadZoneOffsetX = 10.0f;
    public float deadZoneOffsetY = 10.0f;

    private List<PlayerController> _players = new List<PlayerController>();
    public List<PlayerController> players { get { return _players; } }

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    // Update material of player when one is joining to avoid them to have the same color
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        float playerSpawnOffset = 0.0f;
        switch (playerInput.playerIndex)
        {
            case 0:
                inputMan.playerPrefab = player2;
                break;
            case 1:
                inputMan.playerPrefab = player3;

                playerSpawnOffset = 0.5f;
                break;
            case 2:
                inputMan.playerPrefab = player4;
                playerSpawnOffset = -0.5f;
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
        targetGroup.AddMember(playerTransform, 1, 20);
        playerTransform.SetParent(BoatManager.instance.self);
        _players.Add(playerInput.gameObject.GetComponent<PlayerController>());
    }

    private void Update()
    {
        if (_players.Count == 0) return;
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
                        player.Die();
                }
                return;
            }
        }
        camManager.isUnzooming = false;
    }
}
