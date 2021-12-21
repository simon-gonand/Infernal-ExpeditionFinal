using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICarriable : IInteractable
{
    public void GetOnBoat(Transform entryPosition);
    public void GetOffBoat();
    public void Launch(PlayerController player);
}
