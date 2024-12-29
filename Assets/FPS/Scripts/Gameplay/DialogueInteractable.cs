using System.Collections;
using System.Collections.Generic;
using Unity.FPS.Gameplay;
using UnityEngine;

public class DialogueInteractable : MonoBehaviour, IInteractable
{
    [Tooltip("Start dialogue")]
    [SerializeField] DialogueObject dialogueObject;

    public void Interact(Interactor interactor)
    {
        if (dialogueObject != null)
        {
            DialogueStartedEvent evt = new DialogueStartedEvent();

            evt.StartedDialogue = dialogueObject;

            EventManager.Broadcast(evt);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<Interactor>(out var interactor))
        {
            interactor.Interactable = this;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<Interactor>(out var interactor))
        {
            if (interactor.Interactable == this) interactor.Interactable = null;
        }
    }
}
