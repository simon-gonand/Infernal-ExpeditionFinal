using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SeaSicknessModifier : IModifier
{
    protected override void StartBehaviour()
    {
        ModifierUiManager.instance.SpawnSickToken(durationTime);

        UniversalAdditionalCameraData additionalCameraData = Camera.main.GetComponent<UniversalAdditionalCameraData>();
        additionalCameraData.SetRenderer(1);
    }
    protected override void EndBehaviour()
    {
        UniversalAdditionalCameraData additionalCameraData = Camera.main.GetComponent<UniversalAdditionalCameraData>();
        additionalCameraData.SetRenderer(0);
    }
}
