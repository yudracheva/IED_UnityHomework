using System;
using Interfaces;
using UnityEngine;

namespace Bonuses
{
    [RequireComponent(typeof(BoxCollider))]
    public abstract class Bonus : MonoBehaviour, IPickedupObject<Bonus>
    {
        private int amountValue;
        
        public BonusTypeId Type { get; private set; }

        public event Action<Bonus> PickedUp;

        public void Show() => gameObject.SetActive(true);

        public void Hide() => gameObject.SetActive(false);

        public void SetPosition(Vector3 position) => transform.position = position;

        public void SetValue(int value) => amountValue = value;

        protected abstract void Pickup(Collider other, int value);

        protected abstract bool IsCanBePickedUp(Collider other);

        private void OnTriggerEnter(Collider other)
        {
            if (!IsCanBePickedUp(other)) return;
            
            Pickup(other, amountValue);
            NotifyAboutPickedUp();
        }

        private void NotifyAboutPickedUp() => PickedUp?.Invoke(this);
    }
}