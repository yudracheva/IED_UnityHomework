using System;
using Services.PlayerData;
using UnityEngine;

namespace Hero
{
    public class HeroNumberOfKilledEnemies : MonoBehaviour
    {
        private NumberOfEnemiesKilled _number;
        public event Action<int> Changed;

        public void Construct(NumberOfEnemiesKilled number)
        {
            _number = number;
            _number.Changed += Display;
            Display();
        }

        private void OnDestroy() =>
            _number.Changed -= Display;

        public void AddDeadEnemy() =>
            _number.AddDeadEnemy();

        private void Display() =>
            Changed?.Invoke(_number.Count);
    }
}