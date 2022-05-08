using Input;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Services.Input
{
    public class InputService : IInputService
    {
        private readonly HeroControls input;
        private Mouse currentMouse;

        public InputService(HeroControls inputMap)
        {
            input = inputMap;
        }

        public void Enable()
        {
            input.Enable();
            currentMouse = Mouse.current;
        }

        public void Disable()
        {
            input.Disable();
        }

        public Vector2 Axis =>
            SimpleInputAxis();

        public Vector2 ClickPosition =>
            MousePosition();

        public bool IsAttackButtonDown()
        {
            return input.Player.Attack.triggered;
        }

        public bool IsRollButtonDown()
        {
            return input.Player.Roll.triggered;
        }

        public bool IsBlockButtonPressed()
        {
            return input.Player.Block.IsPressed();
        }

        private Vector2 SimpleInputAxis()
        {
            return input.Player.Move.ReadValue<Vector2>();
        }

        private Vector2 MousePosition()
        {
            return input.Player.Mouse.ReadValue<Vector2>();
        }
    }
}