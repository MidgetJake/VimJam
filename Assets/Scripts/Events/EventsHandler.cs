using UnityEngine;
using UnityEngine.Events;

namespace Events {
    public class EventsHandler : MonoBehaviour {
        public UnityEvent OnSpawn;
        public UnityEvent OnDeath;

        public UnityEvent<float> OnDamage;
        public UnityEvent<float> OnHeal;

        public virtual void Damage(float damage) => OnDamage.Invoke(damage);
        public virtual void Heal(float heal) => OnHeal.Invoke(heal);
        public virtual void Spawn() => OnSpawn.Invoke();
        public virtual void Death() => OnDeath.Invoke();
    }
}