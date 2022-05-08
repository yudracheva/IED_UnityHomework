using System;

namespace Animations
{
    public class BattleAnimator : SimpleAnimator
    {
        public event Action Attacked;
        
        public event Action PreAttacked;

        public void TriggerAttack() => Attacked?.Invoke();
        
        public void SongAttack() => PreAttacked?.Invoke();
    }
}