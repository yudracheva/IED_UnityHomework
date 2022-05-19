﻿using Bonuses;
using Enemies.Spawn;
using Loots;
using Services.UI.Factory;
using StaticData.Audio;
using StaticData.Bonuses;
using StaticData.Enemies;
using StaticData.Hero;
using StaticData.Level;
using StaticData.Loot;
using StaticData.Score;
using StaticData.Shop;
using StaticData.UI;

namespace Services.StaticData
{
    public interface IStaticDataService : IService
    {
        void Load();
        WindowInstantiateData ForWindow(WindowId id);
        HeroSpawnStaticData ForHero();
        HeroBaseStaticData ForHeroCharacteristics();
        EnemyStaticData ForMonster(EnemyTypeId typeId);
        LevelStaticData ForLevel(string sceneKey);
        EnemyLoot ForLoot(string levelKey, EnemyTypeId typeId);
        ShopStaticData ForShop();
        BonusStaticData ForBonus(BonusTypeId typeId);
        ScoreStaticData ForScore();
        MandatoryInventoryItemStaticData ForMandatoryItem(LootType key);
        DoorStaticData GetDoor();
        LevelAudio AudioForLevel(string sceneKey);
    }
}