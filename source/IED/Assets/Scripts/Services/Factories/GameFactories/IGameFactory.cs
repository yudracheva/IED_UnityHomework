﻿using System.Collections.Generic;
using Enemies.Spawn;
using StaticData.Level;
using UnityEngine;

namespace Services.Factories.GameFactories
{
    public interface IGameFactory : IService
    {
        GameObject CreateHero(bool needToReset = true);
        GameObject CreateHud(GameObject gameObject);
        void CreateEnemySpawnPoints(List<SpawnPointStaticData> spawnPoints, SpawnPoint pointPrefab);
        void CreateBonusSpawnPoints(List<SpawnPointStaticData> spawnPoints, SpawnPoint pointPrefab);
    }
}