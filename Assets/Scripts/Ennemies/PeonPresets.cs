using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PeonPresets", menuName = "Presets/Peon", order = 1)]
public class PeonPresets : ScriptableObject
{
    [Tooltip("to have same speed than player it's approximatively \"playerSpeed / 50\"")]
    public float speed;
    public float howManyCanAttackAPlayer;
}
