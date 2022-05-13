using Services.PlayerData;
using StaticData.Loot.Items;
using UnityEngine;

namespace Hero
{
    public class HeroInventory : MonoBehaviour
    {
        private Inventory _inventory;

        public void Construct(Inventory inventory)
        {
            this._inventory = inventory;
        }

        public bool IsCanAddItem(ItemStaticData item) =>
            _inventory.IsCanAddItem(item);

        public void AddItem(ItemStaticData item) =>
            _inventory.AddItem(item);
    }
}