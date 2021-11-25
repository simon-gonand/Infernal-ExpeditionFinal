using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerManager : MonoBehaviour
{
    [Header("Player Materials")]
    [SerializeField]
    private Material player1Material;
    [SerializeField]
    private Material player2Material;
    [SerializeField]
    private Material player3Material;
    [SerializeField]
    private Material player4Material;

    private List<PlayerController> _players = new List<PlayerController>();
    public List<PlayerController> players { get { return _players; } }

    public CinemachineTargetGroup targetGroup;
    private int count;

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
                playerInput.gameObject.GetComponentInChildren<MeshRenderer>().material = player1Material;
                break;
            case 1:
                playerInput.gameObject.GetComponentInChildren<MeshRenderer>().material = player2Material;
                playerSpawnOffset = 0.5f;
                break;
            case 2:
                playerInput.gameObject.GetComponentInChildren<MeshRenderer>().material = player3Material;
                playerSpawnOffset = -0.5f;
                break;
            case 3:
                playerInput.gameObject.GetComponentInChildren<MeshRenderer>().material = player4Material;
                playerSpawnOffset = 1.0f;
                break;
            default:
                break;
        }

        // Update players spawn positions according to which player is spawning
        // Player is spawning on the boat
        Vector3 playerSpawnPosition = BoatManager.instance.self.position;
        playerSpawnPosition.y += 3.0f;
        playerSpawnPosition.z += playerInput.playerIndex * playerSpawnOffset;
        Transform playerTransform = playerInput.gameObject.transform;
        playerTransform.position = playerSpawnPosition;
        playerTransform.SetParent(BoatManager.instance.self);
        _players.Add(playerInput.gameObject.GetComponent<PlayerController>());


        targetGroup.m_Targets[count+1].target = playerInput.gameObject.transform;
        count++;
    }
}
