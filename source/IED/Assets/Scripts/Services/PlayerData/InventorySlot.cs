using StaticData.Loot.Items;

namespace Services.PlayerData
{
    public class InventorySlot
    {
        public int Count;
        public int Index;
        public ItemStaticData Item;

        public InventorySlot(int index)
        {
            Index = index;
        }

        public void ClearSlot()
        {
            Item = null;
            Count = 0;
        }

        public void RemoveItem(int count)
        {
            Count -= count;
            if (Count == 0)
                ClearSlot();
        }

        public void PutItem(ItemStaticData item)
        {
            if (Item == null)
                Item = item;

            Count++;
        }
    }
}