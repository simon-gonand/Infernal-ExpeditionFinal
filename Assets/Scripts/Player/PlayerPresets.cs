using UnityEngine;

[CreateAssetMenu(fileName = "PlayerPresets", menuName = "Presets/Player", order = 1)]
public class PlayerPresets : ScriptableObject
{
    [Tooltip("Define the distance from where the player can interact with interactables")]
    [Range(0, 2)] public float interactionDistance;

    [Header("Speed")]
    public float playerGroundSpeed;
    public float playerSwimSpeed;
    public float playerInNotDeepWaterSpeed;
    public float climbingOnBoatSpeed;

    [Header("Dash")]
    public float dashSpeed;
    public float dashTime;
    public float dashCooldown;

    [Header("Attack")]
    public float attackRange;
    public float attackCooldown;
    public float stunTime;

    [Header("Carry other player")]
    public float maxLaunchForce;
    [Tooltip("In how many time the player can launch the treasure with full force")]
    public float fullChargeTime;
}
