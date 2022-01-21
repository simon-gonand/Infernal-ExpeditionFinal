using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoDashModifier : IModifier
{
    protected override void StartBehaviour()
    {
        ModifierUiManager.instance.SpawnNoDashToken(durationTime);

        foreach (PlayerController player in PlayerManager.instance.players)
        {
            player.canDash = false;
        }
    }

    protected override void EndBehaviour()
    {
        foreach (PlayerController player in PlayerManager.instance.players)
        {
            player.canDash = true;
        }
    }

}
