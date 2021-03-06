using UnityEngine;

// Interface that will implement all interactable object (boat equiments, treasures...)
public interface IInteractable
{
    // When the player is interacting with the interactable
    public bool InteractWith(PlayerController player, GameObject interactingWith);
    // When the player is not interacting with the interactable anymore
    public void UninteractWith(PlayerController player);

    // When the player pressed the action button when he's on the interactable
    public void OnAction(PlayerController player);

    public string GetTag();
}
