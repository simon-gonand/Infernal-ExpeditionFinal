using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TreasuresCategoryPresets", menuName = "Presets/TreasuresCategory", order = 1)]
public class TreasuresCategory : ScriptableObject
{
    public float maxLaunchForce;
    public float multiplyUpAngle;
    [Tooltip("In how many time the player can launch the treasure with full force")]
    public float fullChargeTime;
    public float speedMalus;
    public int maxPlayerCarrying;
}
