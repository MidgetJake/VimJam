using UnityEngine;
using Events;
using UI;

namespace Player {
    [RequireComponent(typeof(EventsHandler))]
    public class BaseStats : MonoBehaviour {
        public float health = 100;
        public float movementSpeed = 5f;
        public float dodgeSpeed = 15f;
        public float dodgeTime = 0.33f;
        public float currStamina = 10;
        public float maxStamina = 10;
        public int kills;
        public Vector2 minMaxHealth = new Vector2(0, 100);

        [SerializeField] private HeartContainer hc;
        [SerializeField] private KillCounter cc;
        
        private EventsHandler m_EventsHandler;
        private bool m_IsPlayer = false;

        public void Start() {
            m_EventsHandler = GetComponent<EventsHandler>();
        }

        public virtual void TakeDamage(float damage = 1) {
            health -= damage;
            if (health < minMaxHealth.x) { health = minMaxHealth.x; }
            if (health <= minMaxHealth.x) { m_EventsHandler.Death(); }
            hc.UpdateHealth((int)health, (int)minMaxHealth.y);
        }

        public virtual void Heal(float heal = 1) {
            health += heal;
            if (health > minMaxHealth.y) { health = minMaxHealth.y; }
            hc.UpdateHealth((int)health, (int)minMaxHealth.y);
        }

        public virtual void Death() {
            Debug.Log("Oh... yeah... you're dead");
        }
        
        public virtual void Kills()
        {
            kills++;
            cc.SetKillCounter(kills);

        }
        
    }
}