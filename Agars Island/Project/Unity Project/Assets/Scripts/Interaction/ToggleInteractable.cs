using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleInteractable : MonoBehaviour
{
    private Interactable interactable;

    public bool interactableInParent;
    // Start is called before the first frame update
    void Start()
    {
        if (interactableInParent)
            interactable = GetComponentInParent<Interactable>();
        else
            interactable = GetComponent<Interactable>();
    }

    public void ToggleInteract()
    {
        interactable.canInteract = !interactable.canInteract;
    }
}
