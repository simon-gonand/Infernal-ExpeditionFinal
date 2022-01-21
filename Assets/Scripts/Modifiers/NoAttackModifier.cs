using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoAttackModifier : IModifier
{
    protected override void StartBehaviour()
    {
        ModifierUiManager.instance.SpawnNoAttackToken(durationTime);

        foreach(PlayerController player in PlayerManager.instance.players)
        {
            player.canAttack = false;
        }
    }

    protected override void EndBehaviour()
    {
        foreach (PlayerController player in PlayerManager.instance.players)
        {
            player.canAttack = true; ;
        }
    }

}
