using System;
using Services.Progress;

namespace Services.StatisticCounter
{
    public class StatisticCounterService : IStatisticCounterService
    {
        private readonly IPersistentProgressService _progressService;
        
        public StatisticCounterService(IPersistentProgressService progressService)
        {
            _progressService = progressService ?? throw new ArgumentNullException(nameof(progressService));
        }
            
        public void AddDeathEnemy()
        {
            _progressService.Player.NumberOfKilledEnemies.AddDeadEnemy();
        }

        public void AddWave()
        {
            _progressService.Player.NumberOfWaves.AddWave();
        }
    }
}