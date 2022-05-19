using System;
using System.Collections;
using Enemies.Entity;
using Enemies.Spawn;
using Services.Bonuses;
using Services.StatisticCounter;
using StaticData.Level;
using UnityEngine;

namespace Services.Waves
{
    public class WaveServices : IWaveServices
    {
        private readonly IBonusSpawner _bonusSpawner;
        private readonly ICoroutineRunner _coroutineRunner;
        private readonly IEnemySpawner _enemiesSpawner;
        private readonly IStatisticCounterService _statisticCounterService;
        private readonly IKeySpawner _keySpawner;
        
        private int _currentEnemiesCount;
        private int _currentWaveIndex;
        private LevelWaveStaticData _waves;

        public WaveServices(
            IEnemySpawner spawner, 
            ICoroutineRunner coroutineRunner, 
            IBonusSpawner bonusSpawner, 
            IStatisticCounterService statisticCounterService,
            IKeySpawner keySpawner)
        {
            _statisticCounterService = statisticCounterService ?? throw new ArgumentNullException(nameof(statisticCounterService));
            _enemiesSpawner = spawner ?? throw new ArgumentNullException(nameof(spawner));
            _coroutineRunner = coroutineRunner ?? throw new ArgumentNullException(nameof(coroutineRunner));
            _bonusSpawner = bonusSpawner ?? throw new ArgumentNullException(nameof(bonusSpawner));
            _keySpawner = keySpawner ?? throw new ArgumentNullException(nameof(keySpawner));
            
            _enemiesSpawner.Spawned += OnEnemySpawned;
        }

        public void Cleanup()
        {
            _enemiesSpawner.Spawned -= OnEnemySpawned;
        }

        public void Start()
        {
            _currentWaveIndex = 0;
            _coroutineRunner.StartCoroutine(StartWave(_waves.FirstWaveDelay));
        }

        public void SetLevelWaves(LevelWaveStaticData wavesData)
        {
            _waves = wavesData;
        }

        private void OnEnemySpawned(GameObject enemy)
        {
            enemy.GetComponent<EnemyDeath>().Happened += OnEnemyDead;
        }

        private void OnEnemyDead(EnemyTypeId enemyTypeId, GameObject enemy)
        {
            _statisticCounterService.AddDeathEnemy();
            enemy.GetComponent<EnemyDeath>().Happened -= OnEnemyDead;
            _currentEnemiesCount--;
            if (_currentEnemiesCount <= 0)
            {
                _statisticCounterService.AddWave();
                CompleteWave();   
            }
        }

        private void CompleteWave()
        {
            _coroutineRunner.StartCoroutine(StartWave(_waves.Waves[_currentWaveIndex].WaveWaitTime));
            SpawnBonuses();
            IncWaveIndex();
        }

        private IEnumerator StartWave(float delay)
        {
            yield return new WaitForSeconds(delay);

            _enemiesSpawner.Spawn(_waves.Waves[_currentWaveIndex].Enemies);
            _currentEnemiesCount = 0;
            for (var i = 0; i < _waves.Waves[_currentWaveIndex].Enemies.Length; i++)
                _currentEnemiesCount += _waves.Waves[_currentWaveIndex].Enemies[i].Count;
        }

        private void SpawnBonuses()
        {
            var bonuses = _waves.Waves[_currentWaveIndex].Bonuses;
            foreach (var t in bonuses)
                _bonusSpawner.SpawnBonus(t);
            
            _keySpawner.SpawnKey(_bonusSpawner.GetRandomPoint());
        }

        private void IncWaveIndex()
        {
            _currentWaveIndex++;
            _currentWaveIndex = Mathf.Clamp(_currentWaveIndex, 0, _waves.Waves.Length);
        }
    }
}