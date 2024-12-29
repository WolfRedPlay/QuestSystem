using UnityEngine;
using Unity.FPS.Gameplay;

public class Interactor : MonoBehaviour
{
    PlayerInputHandler _playerInput;
    public IInteractable Interactable { get; set; }

    private void Start()
    {
        _playerInput = GetComponent<PlayerInputHandler>();
    }

    private void Update()
    {
        if (_playerInput.GetInteractInputDown())
        {
            if (Interactable != null) Interactable.Interact(this);
        }
    }
}
