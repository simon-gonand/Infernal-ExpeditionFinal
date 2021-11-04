using UnityEngine;

// Interface that will implement all interactable object (boat equiments, treasures...)
public interface IInteractable
{
    // When the player is interacting with the interactable
    public void InteractWith(PlayerController player);
    // When the player is not interacting with the interactable anymore
    public void UninteractWith(PlayerController player);

    // When the player pressed the action button when he's on the interactable
    public void OnAction();
    // When the player is moving when he's on the interactable
    public void OnMove(Vector2 movements);
}
