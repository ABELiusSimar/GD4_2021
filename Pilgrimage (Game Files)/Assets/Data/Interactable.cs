using UnityEngine;

namespace Interactable
{
    public interface IInteractable
    {
        bool IsInteractable { get; }
        void onInteract();
    }
}