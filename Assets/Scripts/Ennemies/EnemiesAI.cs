using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface EnemiesAI
{
    public void ResetCurrentFollowedPlayer();
    public void Die(PlayerController player);
}
