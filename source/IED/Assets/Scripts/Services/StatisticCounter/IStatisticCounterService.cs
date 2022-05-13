namespace Services.StatisticCounter
{
    public interface IStatisticCounterService : IService
    {
        void AddDeathEnemy();
        
        void AddWave();
    }
}