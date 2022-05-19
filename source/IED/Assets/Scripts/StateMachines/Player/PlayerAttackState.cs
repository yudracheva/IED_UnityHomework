using Animations;
using Hero;
using StaticData.Hero.Components;
using UnityEngine;

namespace StateMachines.Player
{
    public class PlayerAttackState : PlayerBaseMachineState
    {
        private readonly float attackCooldown;
        private readonly HeroAttack heroAttack;
        private readonly HeroStamina heroStamina;
        private readonly HeroSongs _heroSongs;
        private readonly AudioSource _audioSource;
        
        private bool isAttackEnded;

        private float lastAttackTime;

        
        public PlayerAttackState(StateMachine stateMachine, string animationName, BattleAnimator animator,
            HeroStateMachine hero, HeroAttack heroAttack, HeroAttackStaticData attackData, HeroStamina heroStamina) :
            base(stateMachine, animationName, animator, hero)
        {
            this.heroAttack = heroAttack;
            this.heroStamina = heroStamina;
            this.animator.Attacked += Attack;
            this.animator.PreAttacked += SongAttack;
            
            _heroSongs = heroAttack.transform.parent.GetComponentInChildren<HeroSongs>();
            _audioSource = heroAttack.transform.parent.GetComponentInChildren<AudioSource>();
            
            attackCooldown = attackData.AttackCooldown;
            UpdateAttackTime();
        }

        public void Cleanup()
        {
            animator.Attacked -= Attack;
            animator.PreAttacked -= SongAttack;
        }

        public bool IsCanAttack()
        {
            return Time.time >= lastAttackTime + attackCooldown && heroStamina.IsCanAttack();
        }

        public override void Enter()
        {
            base.Enter();
            isAttackEnded = false;
        }

        public override bool IsCanBeInterrupted()
        {
            return isAttackEnded;
        }

        public override void TriggerAnimation()
        {
            base.TriggerAnimation();
            isAttackEnded = true;
            if (hero.IsBlockingPressed)
            {
                if (IsStayHorizontal() == false)
                    ChangeState(hero.ShieldMoveState);
                else
                    ChangeState(hero.IdleShieldState);
            }
            else
            {
                if (IsStayVertical())
                    ChangeState(hero.IdleState);
                else
                    ChangeState(hero.MoveState);
            }
        }

        private void SongAttack()
        {
            _audioSource.clip = _heroSongs.attackSong;
            _audioSource.Play();
        }
        
        private void Attack()
        {
            heroAttack.Attack();
            heroStamina.WasteToAttack();
        }

        private void UpdateAttackTime()
        {
            lastAttackTime = Time.time;
        }
    }
}