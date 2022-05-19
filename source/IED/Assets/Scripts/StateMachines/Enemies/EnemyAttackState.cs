using Animations;
using Enemies.Entity;
using StaticData.Enemies;
using UnityEngine;

namespace StateMachines.Enemies
{
    public class EnemyAttackState : EnemyBaseMachineState
    {
        private readonly float attackCooldown;
        private readonly EnemyStateMachine enemy;
        private readonly EnemyAttack enemyAttack;
        private float damageCoeff;

        private float lastAttackTime;

        public EnemyAttackState(StateMachine stateMachine, string animationName, BattleAnimator animator,
            EnemyStateMachine enemy, EnemyAttack enemyAttack, EnemyAttackStaticData attackData, float damageCoeff) :
            base(stateMachine, animationName, animator)
        {
            this.enemy = enemy;
            this.enemyAttack = enemyAttack;
            this.damageCoeff = damageCoeff;
            attackCooldown = attackData.AttackCooldown;
            UpdateAttackTime();
            this.animator.Attacked += Attack;
        }

        public void Cleanup()
        {
            animator.Attacked -= Attack;
        }

        public void UpdateDamageCoeff(float coeff)
        {
            damageCoeff = coeff;
        }

        public override bool IsCanBeInterrupted()
        {
            return true;
        }

        public override void Enter()
        {
            base.Enter();
            UpdateAttackTime();
        }

        public override void TriggerAnimation()
        {
            base.TriggerAnimation();
            ChangeState(enemy.IdleState);
        }

        public bool IsCanAttack()
        {
            return Time.time >= lastAttackTime + attackCooldown;
        }

        private void Attack()
        {
            enemyAttack.Attack(damageCoeff);
        }

        private void UpdateAttackTime()
        {
            lastAttackTime = Time.time;
        }
    }
}