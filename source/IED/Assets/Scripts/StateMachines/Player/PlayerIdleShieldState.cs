﻿using Animations;
using Hero;
using UnityEngine;

namespace StateMachines.Player
{
    public class PlayerIdleShieldState : PlayerBaseMachineState
    {
        private readonly int _floatValueHash;
        private readonly HeroRotate _heroRotate;

        public PlayerIdleShieldState(
            StateMachine stateMachine, 
            string animationName,
            string floatValueName,
            BattleAnimator animator,
            HeroStateMachine hero,
            HeroRotate heroRotate) 
            : base(stateMachine, animationName, animator, hero)
        {
            _floatValueHash = Animator.StringToHash(floatValueName);
            _heroRotate = heroRotate;
        }

        public override void LogicUpdate()
        {
            base.LogicUpdate();
            if (hero.IsBlockingPressed)
            {
                if (IsStayHorizontal() == false)
                {
                    ChangeState(hero.ShieldMoveState);
                }
                else if (Mathf.Approximately(hero.RotateAngle, 0) == false)
                {
                    _heroRotate.Rotate(hero.RotateAngle);
                    SetFloat(_floatValueHash, Mathf.Sign(hero.RotateAngle));
                }
                else
                {
                    SetFloat(_floatValueHash, 0);
                }
            }
            else
            {
                if (IsStayVertical())
                    ChangeState(hero.IdleState);
                else
                    ChangeState(hero.MoveState);
            }
        }

        public override bool IsCanBeInterrupted()
        {
            return true;
        }
    }
}