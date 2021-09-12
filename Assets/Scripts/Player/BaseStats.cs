using UnityEngine;
using Events;

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

        public void Start() {
            m_EventsHandler = GetComponent<EventsHandler>();
        }

        public virtual void TakeDamage(float damage = 1) {
            health -= damage;
            if (health < minMaxHealth.x) { health = minMaxHealth.x; }
            if (health <= minMaxHealth.x) { m_EventsHandler.Death(); }

            //if (m_IsPlayer) { m_Player.selfStats.health = health; }
        }

        public virtual void Heal(float heal = 1) {
            health += heal;
            if (health > minMaxHealth.y) { health = minMaxHealth.y; }
            //if (m_IsPlayer) { m_Player.selfStats.health = health; }
        }

        public virtual void Death() {
            Debug.Log("Oh... yeah... you're dead");
        }
    }
}