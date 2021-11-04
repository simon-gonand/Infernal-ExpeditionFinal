using UnityEngine;

[CreateAssetMenu(fileName = "PlayerPresets", menuName = "Presets/Player", order = 1)]
public class PlayerPresets : ScriptableObject
{
    public float playerSpeed;
    [Tooltip("Define the distance from where the player can interact with interactables")]
    [Range(0, 2)] public float interactionDistance;
    public float dashSpeed;
    public float dashTime;
    public float dashCooldown;
}
