using Systems.Healths;
using Animations;
using Hero;
using StateMachines.Enemies;
using StaticData.Enemies;
using UnityEngine;
using Utilities;

namespace Enemies.Entity
{
    public class EnemyStateMachine : BaseEntityStateMachine
    {
        [SerializeField] protected BattleAnimator battleAnimator;
        [SerializeField] private EntitySearcher entitySearcher;
        [SerializeField] private EnemyMove move;
        [SerializeField] private EnemyRotate rotate;
        [SerializeField] private EnemyAttack attack;
        [SerializeField] private EnemyDeath death;

        private EnemiesMoveStaticData _moveData;
        private EnemyAttackStaticData _attackData;
        private float _damageCoeff;

        private IHealth _health;

        public EnemyAttackState AttackState { get; private set; }
        public EnemyDeathState DeathState { get; private set; }
        public EnemyHurtState ImpactState { get; private set; }
        public EnemyIdleState IdleState { get; private set; }
        public EnemyRunState RunState { get; private set; }
        public EnemySearchState SearchState { get; private set; }
        public EnemyWalkState WalkState { get; private set; }

        public void Construct(
            EnemiesMoveStaticData moveData,
            EnemyAttackStaticData attackData,
            float damageCoeff,
            IHealth health)
        {
            _moveData = moveData;
            _attackData = attackData;
            _damageCoeff = damageCoeff;
            _health = health;
            _health.Dead += Dead;
            attack.Construct(_attackData);
            Initialize();
        }

        protected override void Subscribe()
        {
            base.Subscribe();
            battleAnimator.Triggered += AnimationTriggered;
            death.Revived += Revive;
        }

        protected override void Cleanup()
        {
            base.Cleanup();
            battleAnimator.Triggered -= AnimationTriggered;
            _health.Dead -= Dead;
            death.Revived -= Revive;
            AttackState.Cleanup();
        }

        protected override void CreateStates()
        {
            AttackState = new EnemyAttackState(stateMachine, "IsSimpleAttack", battleAnimator, this, attack, _attackData,
                _damageCoeff);
            DeathState = new EnemyDeathState(stateMachine, "IsDead", battleAnimator, death);
            ImpactState = new EnemyHurtState(stateMachine, "IsImpact", battleAnimator, this);
            IdleState = new EnemyIdleState(stateMachine, "IsIdle", battleAnimator, move, _moveData, this, rotate);
            RunState = new EnemyRunState(stateMachine, "IsRun", battleAnimator, move, _moveData, this);
            SearchState = new EnemySearchState(stateMachine, "IsIdle", battleAnimator, entitySearcher, move, this);
            WalkState = new EnemyWalkState(stateMachine, "IsWalk", battleAnimator, move, _moveData, this);
        }

        protected override void SetDefaultState() =>
            stateMachine.Initialize(SearchState);

        public void Impact()
        {
            if (stateMachine.State.IsCanBeInterrupted())
                stateMachine.ChangeState(ImpactState);
        }

        public void UpdateDamageCoeff(float coeff)
        {
            _damageCoeff = coeff;
            AttackState.UpdateDamageCoeff(_damageCoeff);
        }

        private void Dead() =>
            stateMachine.ChangeState(DeathState);

        private void Revive() =>
            stateMachine.ChangeState(SearchState);
    }
}