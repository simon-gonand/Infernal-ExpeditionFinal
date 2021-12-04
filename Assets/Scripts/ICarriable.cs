using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICarriable : IInteractable
{
    public bool isLoadingLaunch { get; }

    public void GetOnBoat(Transform entryPosition);
    public void GetOffBoat();
    public void Launch(PlayerController player);
}
