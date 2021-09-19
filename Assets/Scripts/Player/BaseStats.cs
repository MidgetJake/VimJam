using UnityEngine;
using Events;
using Assets.Scripts.Controller;

namespace Player {
    [RequireComponent(typeof(EventsHandler))]
    public class BaseStats : MonoBehaviour {
        public float health = 100;
        public float movementSpeed = 5f;
        public float dodgeSpeed = 15f;
        public float dodgeTime = 0.33f;
        public int currStamina = 10;
        public int maxStamina = 10;
        public Vector2 minMaxHealth = new Vector2(0, 100);

        private EventsHandler m_EventsHandler;
        private bool m_IsPlayer = false;

        public void Start() => m_EventsHandler = GetComponent<EventsHandler>();

        public virtual void TakeDamage(float damage = 1) {
            if (m_IsPlayer) { Audio.controller.PlayerDeath(transform.position); }

            health -= damage;
            if (health < minMaxHealth.x) { health = minMaxHealth.x; }
            if (health <= minMaxHealth.x) { m_EventsHandler.Death(); }

            // TODO - PLEASE HEALTH BAR UPDATE (SUBTRACT HERE)
        }

        public virtual void Heal(float heal = 1) {
            health += heal;
            if (health > minMaxHealth.y) { health = minMaxHealth.y; }

            // TODO - PLEASE HEALTH BAR UPDATE (ADDING HEALTH)
        }

        public virtual void Death() {
            Debug.Log("Oh... yeah... you're dead");
        }
    }
}