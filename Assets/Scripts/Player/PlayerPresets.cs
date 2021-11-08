using UnityEngine;

[CreateAssetMenu(fileName = "PlayerPresets", menuName = "Presets/Player", order = 1)]
public class PlayerPresets : ScriptableObject
{
    public float playerGroundSpeed;
    public float playerSwimSpeed;
    public float playerInNotDeepWaterSpeed;
    [Tooltip("Define the distance from where the player can interact with interactables")]
    [Range(0, 2)] public float interactionDistance;
    public float gravity;

    [Header("Dash")]
    public float dashSpeed;
    public float dashTime;
    public float dashCooldown;


}
