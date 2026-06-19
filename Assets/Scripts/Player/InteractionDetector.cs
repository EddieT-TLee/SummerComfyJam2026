using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
    private IInteractable interactableObject; // Object that is within range for player can interact with
    public GameObject interactionIcon; // Icon above players head to let if be known they can interact
    
    
    void Start()
    {
        interactionIcon.SetActive(false);
    }

    public void OnInteract(InputAction.CallbackContext context){
        if (context.started) // One shot of action being called
        {
            interactableObject?.Interact();
        }
    }
    
    void OnTriggerEnter2D(Collider2D collision)
    {
        // Lets the player know that there is an object it can interact with in range and lights up their icon
        if (collision.TryGetComponent(out IInteractable interactable) && interactable.CanInteract())
        {
            interactableObject = interactable;
            interactionIcon.SetActive(true);
        }
    }
    
    void OnTriggerExit2D(Collider2D collision)
    {
        // Turns off that Icon and makes sure the player can no longer interact with the object in that range.
        if (collision.TryGetComponent(out IInteractable interactable) && interactable.CanInteract())
        {
            interactableObject = null;
            interactionIcon.SetActive(false);
        }
    }
}
