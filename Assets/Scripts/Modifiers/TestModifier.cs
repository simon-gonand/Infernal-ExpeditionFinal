using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TestModifier : IModifier
{
    protected override void StartBehaviour()
    {
        Debug.Log("Start modifier");
    }
}
