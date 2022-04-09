using System.Collections.Generic;
using System.Linq;
using Bonuses;
using ConstantsValue;
using Enemies;
using Enemies.Spawn;
using Services.UI.Factory;
using StaticData.Bonuses;
using StaticData.Enemies;
using StaticData.Hero;
using StaticData.Level;
using StaticData.Loot;
using StaticData.Score;
using StaticData.Shop;
using StaticData.UI;
using UnityEngine;

namespace Services.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private Dictionary<WindowId, WindowInstantiateData> _windows;
        private Dictionary<EnemyTypeId, EnemyStaticData> _enemies;
        private Dictionary<string, LevelStaticData> _levels;
        private Dictionary<string, EnemyLoot[]> _loots;
        private Dictionary<BonusTypeId, BonusStaticData> _bonuses;
        private HeroSpawnStaticData _heroSpawnData;
        private HeroBaseStaticData _heroCharacteristics;
        private ShopStaticData _shopData;
        private ScoreStaticData _scoreData;

        public void Load()
        {
            _heroSpawnData = Resources.Load<HeroSpawnStaticData>(AssetsPath.HeroSpawnDataPath);
            _heroCharacteristics = Resources.Load<HeroBaseStaticData>(AssetsPath.HeroCharacteristicsDataPath);
            _shopData = Resources.Load<ShopStaticData>(AssetsPath.ShopDataPath);
            _scoreData = Resources.Load<ScoreStaticData>(AssetsPath.ScoreDataPath);

            _enemies = Resources
                .LoadAll<EnemyStaticData>(AssetsPath.EnemiesDataPath)
                .ToDictionary(x => x.Id, x => x);

            _levels = Resources
                .LoadAll<LevelStaticData>(AssetsPath.LevelsDataPath)
                .ToDictionary(x => x.LevelKey, x => x);

            _loots = Resources
                .LoadAll<LevelLootStaticData>(AssetsPath.LootsDataPath)
                .ToDictionary(x => x.LevelKey, x => x.Loots);

            _windows = Resources
                .Load<WindowsStaticData>(AssetsPath.WindowsDataPath)
                .InstantiateData
                .ToDictionary(x => x.ID, x => x);

            _bonuses = Resources
                .LoadAll<BonusStaticData>(AssetsPath.BonusDataPath)
                .ToDictionary(x => x.Type, x => x);
        }

        public WindowInstantiateData ForWindow(WindowId windowId) =>
            _windows.TryGetValue(windowId, out var staticData)
                ? staticData
                : new WindowInstantiateData();

        public HeroSpawnStaticData ForHero() =>
            _heroSpawnData;

        public HeroBaseStaticData ForHeroCharacteristics() =>
            _heroCharacteristics;

        public EnemyStaticData ForMonster(EnemyTypeId typeId) =>
            _enemies.TryGetValue(typeId, out var staticData)
                ? staticData
                : null;

        public LevelStaticData ForLevel(string sceneKey) =>
            _levels.TryGetValue(sceneKey, out var staticData)
                ? staticData
                : null;

        public EnemyLoot ForLoot(string levelKey, EnemyTypeId typeId)
        {
            if (!_loots.TryGetValue(levelKey, out var enemyLoots)) 
                return new EnemyLoot();
            
            for (var i = 0; i < enemyLoots.Length; i++)
            {
                if (enemyLoots[i].TypeIds.Contains(typeId))
                    return enemyLoots[i];
            }

            return new EnemyLoot();
        }

        public ShopStaticData ForShop() =>
            _shopData;

        public BonusStaticData ForBonus(BonusTypeId typeId) =>
            _bonuses.TryGetValue(typeId, out var staticData) ? staticData : null;

        public ScoreStaticData ForScore() =>
            _scoreData;
    }
}