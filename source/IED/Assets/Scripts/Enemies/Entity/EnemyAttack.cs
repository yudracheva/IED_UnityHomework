using System.Linq;
using Interfaces;
using StaticData.Enemies;
using UnityEngine;

namespace Enemies.Entity
{
    public class EnemyAttack : MonoBehaviour
    {
        [SerializeField] private Transform attackPoint;
        
        private EnemyAttackStaticData attackData;
        
        private Collider[] hits;
        public void Construct(EnemyAttackStaticData data)
        {
            attackData = data;
            hits = new Collider[attackData.MaxAttackedEntitiesCount];
        }

        public void Attack(float damageCoeff)
        {
            if (Hit(out var hit))
                hit.GetComponentInChildren<IDamageableEntity>().TakeDamage(attackData.Damage * damageCoeff, transform.position);
        }

        private bool Hit(out Collider hit)
        {
            var hitAmount = Physics.OverlapSphereNonAlloc(attackPoint.position, attackData.AttackRadius, hits, attackData.Mask);
            hit = hits.FirstOrDefault();
            return hitAmount > 0;
        }

        private void OnDrawGizmosSelected()
        {
            if (attackData != null)
                Gizmos.DrawWireSphere(attackPoint.position, attackData.AttackRadius);
        }
    }
}
