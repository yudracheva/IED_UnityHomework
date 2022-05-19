using Loots;
using UnityEngine;

namespace StaticData.Bonuses
{
    [CreateAssetMenu(fileName = "MandatoryInventoryItemStaticData", menuName = "Static Data/Bonus/Create Mandatory Inventory Item", order = 56)]
    public class MandatoryInventoryItemStaticData : ScriptableObject
    {
        public LootType Type;
        public GameObject Prefab;
        public Sprite Icon;
    }
}