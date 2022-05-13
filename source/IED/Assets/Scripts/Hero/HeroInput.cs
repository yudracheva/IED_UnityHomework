using System;
using Services.Input;
using UnityEngine;

namespace Hero
{
    public class HeroInput : MonoBehaviour
    {
        [SerializeField] private HeroStateMachine stateMachine;

        private IInputService _inputService;
        private Camera _mainCamera;
        private readonly RaycastHit[] _hits = new RaycastHit[1];
        private bool _isDisabled;

        public void Construct(IInputService inputService)
        {
            _inputService = inputService;
            inputService.Enable();
        }

        private void Start()
        {
            _mainCamera = Camera.main;
        }

        private void OnDestroy()
        {
            _inputService.Disable();
        }

        private void Update()
        {
            if (_isDisabled)
                return;
            
            if (_inputService.IsAttackButtonDown())
                stateMachine.SetAttackState();

            if (_inputService.IsRollButtonDown())
                stateMachine.SetRollState();

            stateMachine.SetIsBlocking(_inputService.IsBlockButtonPressed());
            stateMachine.SetMoveAxis(_inputService.Axis);
            stateMachine.SetRotate(Rotation());
        }

        private float Rotation()
        {
            var ray = _mainCamera.ScreenPointToRay(_inputService.ClickPosition);
            if (Physics.RaycastNonAlloc(ray, _hits) > 0)
                return Angle(_hits[0].point);

            return 0;
        }

        private float Angle(Vector3 mouseClick)
        {
            var differenceDirection =
                new Vector2(mouseClick.x - transform.position.x, mouseClick.z - transform.position.z).normalized;
            var forward = new Vector2(transform.forward.x, transform.forward.z);
            return Vector2.SignedAngle(differenceDirection, forward);
        }

        public void Disable()
        {
            _isDisabled = true;
            stateMachine.SetMoveAxis(Vector2.zero);
            stateMachine.SetRotate(0f);
        }
    }
}