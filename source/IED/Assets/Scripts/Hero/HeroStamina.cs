using System;
using System.Collections;
using Systems.Healths;
using Services.PlayerData;
using StaticData.Hero.Components;
using UnityEngine;

namespace Hero
{
    public class HeroStamina : MonoBehaviour, IStamina
    {
        private float _currentStamina;
        private Coroutine _recoveryCoroutine;

        private HeroStaminaStaticData _staminaData;
        private PlayerCharacteristics _characteristics;

        public event Action<float, float> Changed;

        public void Construct(HeroStaminaStaticData staminaData, PlayerCharacteristics characteristics)
        {
            this._staminaData = staminaData;
            this._characteristics = characteristics;
            SetDefaultValue();
        }

        public bool IsCanAttack() =>
            _currentStamina - _staminaData.AttackCost >= 0;

        public bool IsCanRoll() =>
            _currentStamina - _staminaData.RollCost >= 0;

        public void WasteToAttack()
        {
            _currentStamina -= _staminaData.AttackCost;
            if (_recoveryCoroutine == null)
                _recoveryCoroutine = StartCoroutine(RecoveryValue());
            Display();
        }

        public void WasteToRoll()
        {
            _currentStamina -= _staminaData.RollCost;
            if (_recoveryCoroutine == null)
                _recoveryCoroutine = StartCoroutine(RecoveryValue());
            Display();
        }

        private void SetDefaultValue() =>
            _currentStamina = _characteristics.Stamina();

        private void Display() =>
            Changed?.Invoke(_currentStamina, _characteristics.Stamina());

        private IEnumerator RecoveryValue()
        {
            while (_currentStamina < _characteristics.Stamina())
            {
                yield return new WaitForSeconds(_staminaData.RecoveryRate);
                _currentStamina += _staminaData.RecoveryCount;
                Display();
            }

            _recoveryCoroutine = null;
        }
    }
}