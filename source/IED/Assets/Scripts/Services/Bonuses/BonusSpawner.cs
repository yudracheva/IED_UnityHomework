﻿using System.Collections.Generic;
using Enemies.Spawn;
using Services.Random;
using StaticData.Level;
using UnityEngine;

namespace Services.Bonuses
{
    public class BonusSpawner : IBonusSpawner
    {
        private readonly IBonusFactory bonusFactory;
        private readonly IRandomService randomService;
        private readonly List<SpawnPoint> spawnPoints = new(20);

        public BonusSpawner(IBonusFactory bonusFactory, IRandomService randomService)
        {
            this.bonusFactory = bonusFactory;
            this.randomService = randomService;
        }

        public void Cleanup()
        {
            spawnPoints.Clear();
            bonusFactory.Cleanup();
        }

        public void AddPoint(SpawnPoint spawnPoint)
        {
            spawnPoints.Add(spawnPoint);
        }

        public void SpawnBonus(WaveBonus waveBonus)
        {
            for (var i = 0; i < waveBonus.Count; i++)
                bonusFactory.SpawnBonus(waveBonus.TypeId, waveBonus.Value, RandomSpawnPoint().transform);
        }

        public GameObject GetRandomPoint()
        {
            return spawnPoints[randomService.Next(spawnPoints.Count)].gameObject;
        }

        private SpawnPoint RandomSpawnPoint()
        {
            return spawnPoints[randomService.Next(spawnPoints.Count)];
        }
    }
}