using UnityEngine;

namespace StaticData.Level
{
    [CreateAssetMenu(fileName = "DoorStaticData", menuName = "Static Data/Levels/Create Door Data", order = 55)]
    public class DoorStaticData : ScriptableObject
    {
        public GameObject DoorPrefab;
        public Vector3 SpawnPoint;
        public Quaternion SpawnRotation;
    }
}