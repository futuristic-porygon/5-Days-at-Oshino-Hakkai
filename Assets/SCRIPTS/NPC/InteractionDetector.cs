using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{

    private IInteractable interactableInRange = null;
    [SerializeField] private GameObject interactIcon;
   
    void Start()
    {
        interactIcon.SetActive(false);
    }

    public void onInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            interactableInRange?.Interact();
            if (!interactableInRange.canInteract())
            {
                interactIcon.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable.canInteract())
        {
            interactableInRange = interactable;
            interactIcon.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable== interactableInRange)
        {
            interactableInRange = null;
            interactIcon.SetActive(false);
        }
    }
    //holi
}
