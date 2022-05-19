using Enemies.Spawn;
using StaticData.Level;
using UnityEngine;

namespace Services.Bonuses
{
    public interface IBonusSpawner : ICleanupService
    {
        void AddPoint(SpawnPoint spawnPoint);
        void SpawnBonus(WaveBonus bonus);
        GameObject GetRandomPoint();
    }
}