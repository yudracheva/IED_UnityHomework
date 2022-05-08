using Systems.Healths;
using Animations;
using ConstantsValue;
using Services.PlayerData;
using Services.UserSetting;
using StateMachines.Player;
using StaticData.Hero.Components;
using UnityEngine;

namespace Hero
{
    public class HeroStateMachine : BaseEntityStateMachine
    {
        [SerializeField] private BattleAnimator battleAnimator;
        [SerializeField] private HeroMove move;
        [SerializeField] private HeroRotate rotate;
        [SerializeField] private HeroAttack attack;
        [SerializeField] private HeroStamina stamina;

        private HeroAttackStaticData attackData;
        private HeroImpactsStaticData impactsData;

        public PlayerAttackState AttackState { get; private set; }
        public PlayerHurtState ImpactState { get; private set; }
        public PlayerIdleShieldState IdleShieldState { get; private set; }
        public PlayerIdleState IdleState { get; private set; }
        public PlayerRollState RollState { get; private set; }
        public PlayerShieldImpactState ShieldImpactState { get; private set; }
        public PlayerMoveState MoveState { get; private set; }
        public PlayerShieldMoveState ShieldMoveState { get; private set; }
        public PlayerDeathState DeathState { get; private set; }

        public bool IsBlockingPressed { get; private set; }
        public bool IsBlockingUp => stateMachine.State == IdleShieldState;
        public bool IsRolling => stateMachine.State == RollState;

        public Vector2 MoveAxis { get; private set; }
        public float RotateAngle { get; private set; }

        public void Construct(
            HeroAttackStaticData heroAttackStaticData, 
            HeroImpactsStaticData heroImpactsStaticData,
            PlayerCharacteristics playerCharacteristics,
            IUserSettingService userSettingService)
        {
            attackData = heroAttackStaticData;
            impactsData = heroImpactsStaticData;
            attack.Construct(heroAttackStaticData, playerCharacteristics);
            GetComponentInChildren<AudioSource>().volume = userSettingService.GetUserSettings().ActionsVolume; 
            Initialize();
        }

        protected override void Subscribe()
        {
            base.Subscribe();
            battleAnimator.Triggered += AnimationTriggered;
        }

        protected override void Cleanup()
        {
            base.Cleanup();
            battleAnimator.Triggered -= AnimationTriggered;
            AttackState.Cleanup();
        }
        
        protected override void CreateStates()
        {
            AttackState = new PlayerAttackState(
                stateMachine: stateMachine, 
                animationName: AnimationStateConstants.IsSimpleAttack, 
                animator: battleAnimator, 
                hero: this, 
                heroAttack: attack,
                attackData: attackData, 
                heroStamina: stamina);
            
            ImpactState = new PlayerHurtState(
                stateMachine: stateMachine, 
                animationName: AnimationStateConstants.IsImpact, 
                animator: battleAnimator, 
                hero: this, 
                cooldown: impactsData.ImpactCooldown);
            
            IdleShieldState = new PlayerIdleShieldState(
                stateMachine: stateMachine, 
                animationName: AnimationStateConstants.IsBlocking, 
                floatValueName: "MouseRotation", 
                animator: battleAnimator,
                hero: this, 
                heroRotate: rotate);
            
            IdleState = new PlayerIdleState(
                stateMachine: stateMachine,
                animationName: AnimationStateConstants.IsIdle,
                floatValueName: "MouseRotation",
                animator: battleAnimator,
                hero: this,
                heroRotate: rotate);
            
            RollState = new PlayerRollState(
                stateMachine: stateMachine,
                animationName: AnimationStateConstants.IsRoll,
                animator: battleAnimator,
                hero: this,
                heroMove: move,
                heroStamina: stamina);
            
            ShieldImpactState = new PlayerShieldImpactState(
                stateMachine: stateMachine,
                animationName: AnimationStateConstants.IsShieldImpact,
                animator: battleAnimator,
                hero: this,
                cooldown: impactsData.ShieldImpactCooldown);
            
            MoveState = new PlayerMoveState(
                stateMachine: stateMachine,
                animationName: AnimationStateConstants.IsIdle,
                floatValueName: "MoveX",
                animator: battleAnimator,
                hero: this,
                heroMove: move,
                heroRotate: rotate);
            
            ShieldMoveState = new PlayerShieldMoveState(
                stateMachine: stateMachine,
                animationName: AnimationStateConstants.IsBlocking,
                floatValueName: "MoveY",
                animator: battleAnimator,
                hero: this,
                heroMove: move,
                heroRotate: rotate);
            
            DeathState = new PlayerDeathState(
                stateMachine: stateMachine,
                animationName: AnimationStateConstants.IsDead,
                animator: battleAnimator,
                hero: this);
        }

        protected override void SetDefaultState() =>
            stateMachine.Initialize(IdleState);
        
        public void SetAttackState()
        {
            if (stateMachine.State.IsCanBeInterapted() && AttackState.IsCanAttack())
                stateMachine.ChangeState(AttackState);
        }

        public void SetMoveAxis(Vector2 moveDirection) =>
            MoveAxis = moveDirection;

        public void SetIsBlocking(bool isBlocking) =>
            IsBlockingPressed = isBlocking;

        public void SetRollState()
        {
            if (stateMachine.State.IsCanBeInterapted() && RollState.IsCanRoll())
                stateMachine.ChangeState(RollState);
        }

        public void ImpactInShield() =>
            stateMachine.ChangeState(ShieldImpactState);

        public void Impact()
        {
            if (ImpactState.IsKnockbackCooldown() && stateMachine.State.IsCanBeInterapted())
                stateMachine.ChangeState(ImpactState);
        }

        public void SetRotate(float rotateAngle) =>
            RotateAngle = rotateAngle;

        public void Dead() =>
            stateMachine.ChangeState(DeathState);
    }
}