using UnityEngine;

namespace Services.Bonuses
{
    public interface IKeySpawner : IService
    {
        void SpawnKey(GameObject parent);
    }
}