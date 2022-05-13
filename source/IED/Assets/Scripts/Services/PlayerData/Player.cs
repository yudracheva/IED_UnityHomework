using StaticData.Hero;
using StaticData.Hero.Components;

namespace Services.PlayerData
{
    public class Player
    {
        public readonly PlayerCharacteristics Characteristics;
        public readonly Equipment Equipment;
        public readonly Inventory Inventory;
        public readonly PlayerMoney Monies;
        public readonly PlayerScore Score;
        public readonly NumberOfEnemiesKilled NumberOfKilledEnemies; 
        public readonly NumberOfWaves NumberOfWaves;
        
        public Player(HeroBaseStaticData heroData)
        {
            Characteristics = new PlayerCharacteristics(heroData.Characteristics);
            Equipment = new Equipment(Characteristics, heroData.EquipmentSlots);
            Inventory = new Inventory(heroData.InventorySlotCount);
            Monies = new PlayerMoney();
            Score = new PlayerScore();
            NumberOfWaves = new NumberOfWaves();
            NumberOfKilledEnemies = new NumberOfEnemiesKilled();
            StaminaStaticData = heroData.StaminaStaticData;
            AttackData = heroData.AttackData;
            ImpactsData = heroData.ImpactsData;
        }

        public HeroStaminaStaticData StaminaStaticData { get; private set; }
        public HeroAttackStaticData AttackData { get; private set; }
        public HeroImpactsStaticData ImpactsData { get; private set; }

        public void SetDefaultValue(HeroBaseStaticData heroData)
        {
            Characteristics.SetDefaultValue(heroData.Characteristics);
            Equipment.ReinitSlots(heroData.EquipmentSlots);
            Inventory.ReinitSlots(heroData.InventorySlotCount);
            Monies.RemoveMoney();
            Score.ResetScore();
            NumberOfKilledEnemies.Reset();
            NumberOfWaves.Reset();
            
            StaminaStaticData = heroData.StaminaStaticData;
            AttackData = heroData.AttackData;
            ImpactsData = heroData.ImpactsData;
        }
    }
}