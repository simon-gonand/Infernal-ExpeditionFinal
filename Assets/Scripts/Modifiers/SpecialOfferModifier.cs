using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialOfferModifier : IModifier
{
    [SerializeField]
    private Treasure activeOn;

    [SerializeField]
    private int newPriceUp;
    [SerializeField]
    private int newPriceDown;

    private List<int> oldValues = new List<int>();
    protected override void StartBehaviour()
    {
        ModifierUiManager.instance.SpawnExpensiveToken(durationTime);

        oldValues.Clear();
        foreach(Treasure treasure in GameManager.instance.treasuresInScene)
        {
            oldValues.Add(treasure.price);
            if (treasure.mesh.sharedMesh == activeOn.mesh.sharedMesh)
            {
                treasure.price = newPriceUp;
            }
            else
            {
                treasure.price = newPriceDown;
            }
        }
    }

    protected override void EndBehaviour()
    {
        for(int i = 0; i < oldValues.Count; ++i)
        {
            GameManager.instance.treasuresInScene[i].price = oldValues[i];
        }
    }
}
