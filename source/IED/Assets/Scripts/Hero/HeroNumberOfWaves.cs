using System;
using Services.PlayerData;
using UnityEngine;

namespace Hero
{
    public class HeroNumberOfWaves : MonoBehaviour
    {
        private NumberOfWaves _number;
        public event Action<int> Changed;

        public void Construct(NumberOfWaves number)
        {
            _number = number;
            _number.Changed += Display;
            Display();
        }

        private void OnDestroy() =>
            _number.Changed -= Display;

        public void AddWave() =>
            _number.AddWave();
        
        private void Display() =>
            Changed?.Invoke(_number.Count);
    }
}