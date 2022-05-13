using System;
using Services.PlayerData;
using UnityEngine;

namespace Hero
{
    public class HeroMoney : MonoBehaviour
    {
        private PlayerMoney _money;
        public event Action<int> Changed;

        public void Construct(PlayerMoney money)
        {
            _money = money;
            _money.Changed += Display;
            Display();
        }

        private void OnDestroy() =>
            _money.Changed -= Display;

        public void AddMoney(int addedMoney) =>
            _money.AddMoney(addedMoney);

        public void ReduceMoney(int decedMoney) =>
            _money.ReduceMoney(decedMoney);

        private void Display() =>
            Changed?.Invoke(_money.Count);
    }
}