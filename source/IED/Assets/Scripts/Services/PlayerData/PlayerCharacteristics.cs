using System;
using System.Collections.Generic;
using Loots;

namespace Services.PlayerData
{
    public class PlayerCharacteristics
    {
        private readonly Dictionary<CharacteristicType, Characteristic> characteristics = new(20);

        public PlayerCharacteristics(Characteristic[] characteristics)
        {
            SetDefaultValue(characteristics);
        }

        public event Action Changed;

        public void SetDefaultValue(Characteristic[] heroCharacteristics)
        {
            characteristics.Clear();
            for (var i = 0; i < heroCharacteristics.Length; i++)
                characteristics.Add(heroCharacteristics[i].Type, heroCharacteristics[i]);
        }

        public int Stamina()
        {
            return characteristics[CharacteristicType.Stamina].Value;
        }

        public int Damage()
        {
            return characteristics[CharacteristicType.Strength].Value;
        }

        public int Health()
        {
            return characteristics[CharacteristicType.Health].Value;
        }

        public void IncCharacteristic(CharacteristicType type, int value)
        {
            if (characteristics.ContainsKey(type))
            {
                characteristics[type].ChangeValue(value);
                NotifyAboutChange();
            }
        }

        public void ReduceCharacteristic(CharacteristicType type, int value)
        {
            if (characteristics.ContainsKey(type))
            {
                characteristics[type].ChangeValue(-value);
                NotifyAboutChange();
            }
        }

        private void NotifyAboutChange()
        {
            Changed?.Invoke();
        }
    }
}