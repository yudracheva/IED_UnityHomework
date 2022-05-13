using System;

namespace Services.PlayerData
{
    public class NumberOfEnemiesKilled
    {
        public int Count { get; private set; }

        public event Action Changed;

        public void AddDeadEnemy()
        {
            Count += 1;
            NotifyAboutChange();
        }
        
        public void Reset()
        {
            Count = 0;
        }
        
        private void NotifyAboutChange()
        {
            Changed?.Invoke();
        }
    }
}