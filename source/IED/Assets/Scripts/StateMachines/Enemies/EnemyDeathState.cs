using Animations;
using Enemies.Entity;

namespace StateMachines.Enemies
{
    public class EnemyDeathState : EnemyBaseMachineState
    {
        private readonly EnemyDeath enemyDeath;

        public EnemyDeathState(StateMachine stateMachine, string animationName, BattleAnimator animator,
            EnemyDeath enemyDeath) : base(stateMachine, animationName, animator)
        {
            this.enemyDeath = enemyDeath;
        }

        public override bool IsCanBeInterrupted()
        {
            return false;
        }

        public override void Enter()
        {
            base.Enter();
            enemyDeath.NotifyAboutDead();
        }
    }
}