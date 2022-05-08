using System;
using Interfaces;
using Services.PlayerData;
using StaticData.Hero.Components;
using UnityEngine;

namespace Hero
{
    public class HeroAttack : MonoBehaviour
    {
        [SerializeField] private Transform attackPoint;

        private HeroAttackStaticData _attackData;
        private PlayerCharacteristics _characteristics;
        
        private Collider[] _hits;

        public void Construct(HeroAttackStaticData data, PlayerCharacteristics characteristics)
        {
            _attackData = data;
            _hits = new Collider[_attackData.MaxAttackedEntitiesCount];
            _characteristics = characteristics;
        }

        public void Attack()
        {
            for (var i = 0; i < Hit(); i++)
            {
                _hits[i].GetComponentInChildren<IDamageableEntity>()
                    .TakeDamage(_characteristics.Damage(), transform.position);
            }
        }

        private int Hit() =>
            Physics.OverlapSphereNonAlloc(attackPoint.position, _attackData.AttackRadius, _hits, _attackData.Mask);

        private void OnDrawGizmosSelected() =>
            Gizmos.DrawWireSphere(attackPoint.position, _attackData.AttackRadius);
    }
}