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
        public int currStamina = 5;
        public int maxStamina = 5;
        public Vector2 minMaxHealth = new Vector2(0, 100);
        public float staminaRegenRate = .33f;
        public float staminaRegenTime = 0;

        [SerializeField] private HeartContainer hc;
        [SerializeField] private KillCounter cc;

        private EventsHandler m_EventsHandler;
        public bool isPlayer = false;
        [SerializeField] private PlayerController m_PlayerController;

        public void Start() => m_EventsHandler = GetComponent<EventsHandler>();

        private void Update() {
            if (currStamina < maxStamina) {
                staminaRegenTime += Time.deltaTime;
                if (staminaRegenTime >= staminaRegenRate) {
                    currStamina++;
                    staminaRegenTime = 0;
                }
            }
        }

        public virtual void TakeDamage(float damage = 1) {
            if (isPlayer) {
                if (m_PlayerController.state == CharacterState.Dodging) {
                    return;
                }
            }

            health -= damage;
            if (health < minMaxHealth.x) { health = minMaxHealth.x; }
            if (health <= minMaxHealth.x) { m_EventsHandler.Death(); }
            hc.UpdateHealth((int)health, (int)minMaxHealth.y);

            // TODO - PLEASE HEALTH BAR UPDATE (SUBTRACT HERE)
        }

        public virtual void Heal(float heal = 1) {
            health += heal;
            if (health > minMaxHealth.y) { health = minMaxHealth.y; }
            hc.UpdateHealth((int)health, (int)minMaxHealth.y);

            // TODO - PLEASE HEALTH BAR UPDATE (ADDING HEALTH)
        }

        public virtual void Death() {
            Debug.Log("Oh... yeah... you're dead");
            Audio.controller.PlayerDeath(transform.position);
        }
        
        public virtual void Kills() {
            kills++;
            cc.SetKillCounter(kills);

        }
    }
}
