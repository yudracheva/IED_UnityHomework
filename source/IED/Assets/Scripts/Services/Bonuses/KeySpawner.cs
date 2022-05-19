using System;
using Loots;
using Services.Assets;
using Services.Random;
using Services.StaticData;
using UnityEngine;

namespace Services.Bonuses
{
    public class KeySpawner : IKeySpawner
    {
        private readonly IRandomService _randomService;
        private readonly IStaticDataService _staticData;
        private readonly IAssetProvider _assets;

        private bool _wasSpawned;
        
        public KeySpawner(IRandomService randomService, IStaticDataService staticData, IAssetProvider assets)
        {
            _randomService = randomService ?? throw new ArgumentNullException(nameof(randomService));
            _staticData = staticData ?? throw new ArgumentNullException(nameof(staticData));
            _assets = assets ?? throw new ArgumentNullException(nameof(assets));
        }

        public void Cleanup()
        {
            _wasSpawned = false;
        }
        
        public void SpawnKey(GameObject parent)
        {
            if (_wasSpawned)
                return;
            
            if (_randomService.NextFloat() < 0.9f)
            {
                _wasSpawned = true;
                var bonusData = _staticData.ForMandatoryItem(LootType.Key);
                var keyObj = _assets.Instantiate(bonusData.Prefab, parent.transform.position, Quaternion.identity, parent.transform);
                keyObj.GetComponent<Key>().Construct(bonusData);
            }
        }
    }
}