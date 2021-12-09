using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PiqueSousPresets", menuName = "Presets/Pique Sous", order = 1)]
public class PiqueSousPreset : ScriptableObject
{
    public float speed;
    public float cooldown;
    public float attachOffset;
}
