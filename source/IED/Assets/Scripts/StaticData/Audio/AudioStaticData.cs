using UnityEngine;

namespace StaticData.Audio
{
    [CreateAssetMenu(fileName = "AudioStaticData", menuName = "Static Data/Audio/Create Audio Data", order = 55)]
    public class AudioStaticData : ScriptableObject
    {
        public LevelAudio[] Audios;
    }
}
