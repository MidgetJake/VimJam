using UnityEngine;
using UnityEngine.Events;
using Weapons;

namespace Events {
    public class EventsHandler : MonoBehaviour {
        public UnityEvent OnSpawn;
        public UnityEvent OnDeath;
        public UnityEvent<Vector2> OnAttack;
        public UnityEvent<BaseWeaponStats> OnPhaseChange;

        public UnityEvent<float> OnDamage;
        public UnityEvent<float> OnHeal;

        public virtual void Damage(float damage) => OnDamage.Invoke(damage);
        public virtual void Heal(float heal) => OnHeal.Invoke(heal);
        public virtual void Attack(Vector2 target) => OnAttack.Invoke(target);
        public virtual void PhaseChange(BaseWeaponStats stats) => OnPhaseChange.Invoke(stats);
        public virtual void Spawn() => OnSpawn.Invoke();
        public virtual void Death() => OnDeath.Invoke();
    }
}