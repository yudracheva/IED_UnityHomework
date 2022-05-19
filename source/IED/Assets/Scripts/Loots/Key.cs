using Hero;
using StaticData.Bonuses;
using StaticData.Loot.Items;
using UnityEngine;

namespace Loots
{
    [RequireComponent(typeof(BoxCollider))]
    public class Key : MonoBehaviour
    {
        private MandatoryInventoryItemStaticData _bonusData;
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out HeroInventory heroInventory))
                Pickup(heroInventory);
        }
        
        private void Pickup(HeroInventory heroInventory)
        {
            var key = ScriptableObject.CreateInstance<ItemStaticData>();
            key.ID = System.Guid.NewGuid().ToString();
            key.Type = LootType.Key;
            key.Name = "Key";
            key.Icon = _bonusData.Icon;

            heroInventory.AddItem(key);
            
            this.gameObject.SetActive(false);
        }

        public void Construct(MandatoryInventoryItemStaticData bonusData)
        {
            _bonusData = bonusData;
        }
    }
}