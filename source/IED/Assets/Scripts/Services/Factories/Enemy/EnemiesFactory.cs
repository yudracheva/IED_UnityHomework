using System;
using System.Collections.Generic;
using Enemies.Entity;
using Enemies.Spawn;
using Services.Assets;
using Services.StaticData;
using Systems.Healths;
using UI.Displaying;
using UnityEngine;

namespace Services.Factories.Enemy
{
    public class EnemiesFactory : IEnemiesFactory
    {
        private readonly IAssetProvider assets;

        private readonly List<SlainedEnemy> enemiesPool;
        private readonly IStaticDataService staticData;

        public EnemiesFactory(IAssetProvider assets, IStaticDataService staticData)
        {
            this.assets = assets;
            this.staticData = staticData;
            enemiesPool = new List<SlainedEnemy>(10);
        }

        public void Cleanup()
        {
            for (var i = 0; i < enemiesPool.Count; i++)
            {
                try
                {
                    enemiesPool[i].Enemy.GetComponent<EnemyDeath>().Happened -= OnMonsterSlained; // enemy can be destroyed
                }
                catch { }
            }

            enemiesPool.Clear();
        }

        public GameObject SpawnMonster(EnemyTypeId typeId, Transform parent, float damageCoeff = 1f, float hpCoeff = 1f)
        {
            if (IsContainsInPool(typeId))
                return RecreateMonster(typeId, parent, damageCoeff, hpCoeff);

            return CreateMonster(typeId, parent, damageCoeff, hpCoeff);
        }

        private GameObject RecreateMonster(
            EnemyTypeId typeId, 
            Transform parent,
            float damageCoeff = 1f,
            float hpCoeff = 1f)
        {
            var enemyData = staticData.ForMonster(typeId);
            var slainedEnemy = PooledMonster(typeId);

            var monster = slainedEnemy.Enemy;

            var health = monster.GetComponentInChildren<IHealth>();
            health.SetHp(enemyData.Hp * hpCoeff, enemyData.Hp * hpCoeff);
            var death = monster.GetComponent<EnemyDeath>();
            death.Revive();
            death.Happened += OnMonsterSlained;
            RemoveFromPool(slainedEnemy);
            monster.transform.position = parent.position;

            monster.GetComponent<EnemyStateMachine>().UpdateDamageCoeff(damageCoeff);
            return monster;
        }

        private GameObject CreateMonster(
            EnemyTypeId typeId, 
            Transform parent, 
            float damageCoeff = 1f,
            float hpCoeff = 1f)
        {
            var enemyData = staticData.ForMonster(typeId);
            var monster = assets.Instantiate(enemyData.Prefab, parent.position, Quaternion.identity, parent);

            var health = monster.GetComponentInChildren<IHealth>();
            health.SetHp(enemyData.Hp * hpCoeff, enemyData.Hp * hpCoeff);

            monster.GetComponentInChildren<HPDisplayer>().Construct(health);
            monster.GetComponent<EnemyStateMachine>()
                .Construct(enemyData.MoveData, enemyData.AttackData, damageCoeff, health);
            var death = monster.GetComponent<EnemyDeath>();
            death.Construct(enemyData.Id);
            death.Happened += OnMonsterSlained;

            return monster;
        }

        private void OnMonsterSlained(EnemyTypeId typeId, GameObject enemy)
        {
            enemiesPool.Add(new SlainedEnemy(typeId, enemy));
            enemy.GetComponent<EnemyDeath>().Happened -= OnMonsterSlained;
        }

        private bool IsContainsInPool(EnemyTypeId typeId)
        {
            for (var i = 0; i < enemiesPool.Count; i++)
                if (enemiesPool[i].Id == typeId)
                    return true;

            return false;
        }

        private SlainedEnemy PooledMonster(EnemyTypeId typeId)
        {
            for (var i = 0; i < enemiesPool.Count; i++)
                if (enemiesPool[i].Id == typeId)
                    return enemiesPool[i];

            return new SlainedEnemy();
        }

        private void RemoveFromPool(SlainedEnemy slainedEnemy)
        {
            enemiesPool.Remove(slainedEnemy);
        }
    }
}