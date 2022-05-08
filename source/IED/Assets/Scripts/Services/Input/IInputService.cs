using UnityEngine;

namespace Services.Input
{
    public interface IInputService : IService
    {
        Vector2 Axis { get; }
        Vector2 ClickPosition { get; }
        void Enable();
        void Disable();

        bool IsAttackButtonDown();
        bool IsRollButtonDown();
        bool IsBlockButtonPressed();
    }
}