using Services.PlayerData;
using StaticData.Hero;

namespace Services.Progress
{
    public class PersistentProgressService : IPersistentProgressService
    {
        private readonly HeroBaseStaticData baseStaticData;

        public PersistentProgressService(HeroBaseStaticData baseStaticData)
        {
            Player = new Player(baseStaticData);
            this.baseStaticData = baseStaticData;
        }

        public Player Player { get; }

        public void SetPlayerToDefault()
        {
            Player.SetDefaultValue(baseStaticData);
        }
    }
}