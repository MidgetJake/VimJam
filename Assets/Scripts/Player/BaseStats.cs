using UnityEngine;
using Events;
using Assets.Scripts.Controller;
using UI;

namespace Player {
    [RequireComponent(typeof(EventsHandler))]
    public class BaseStats : MonoBehaviour {
        public static BaseStats main;
        public float health = 100;
        public float movementSpeed = 5f;
        public float dodgeSpeed = 15f;
        public float dodgeTime = 0.33f;
        public int currStamina = 5;
        public int maxStamina = 5;
        public Vector2 minMaxHealth = new Vector2(0, 100);
        public float staminaRegenRate = .33f;
        public float staminaRegenTime = 0;
        public int kills = 0;

        [SerializeField] private HeartContainer hc;
        [SerializeField] private KillCounter cc;
        [SerializeField] private StaminaController m_StaminaController;

        private EventsHandler m_EventsHandler;
        public bool isPlayer = false;
        [SerializeField] private PlayerController m_PlayerController;

        public void Start() {
            if (isPlayer) {
                main = this;
                hc.UpdateHealth((int) health, (int) minMaxHealth.y);
            }

            m_EventsHandler = GetComponent<EventsHandler>();
        }

        private void Update() {
            if (currStamina < maxStamina) {
                m_StaminaController.UpdateStaminaBar(currStamina, maxStamina);
                staminaRegenTime += Time.deltaTime;
                if (staminaRegenTime >= staminaRegenRate) {
                    currStamina++;
                    staminaRegenTime = 0;
                    m_StaminaController.UpdateStaminaBar(currStamina, maxStamina);
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

            if (isPlayer) {
                hc.UpdateHealth((int) health, (int) minMaxHealth.y);
            }
        }

        public virtual void Heal(float heal = 1) {
            health += heal;
            if (health > minMaxHealth.y) { health = minMaxHealth.y; }
            if (isPlayer) {
                hc.UpdateHealth((int) health, (int) minMaxHealth.y);
            }
        }

        public virtual void IncreaseMaxHealth(int modifier) {
            Debug.Log(modifier);
            health += modifier;
            minMaxHealth.y += modifier;
            if (isPlayer) {
                hc.UpdateHealth((int) health, (int) minMaxHealth.y);
            }
        }

        public virtual void IncreaseMaxStamina(int modifier) {
            maxStamina += modifier;
            currStamina += modifier;
            if (isPlayer) {
                m_StaminaController.UpdateStaminaBar(currStamina, maxStamina);
            }
        }

        public virtual void Death() {
            Debug.Log("Oh... yeah... you're dead");
            BackgroundAudio.controller.gameIsPlaying = false;
            Audio.controller.PlayerDeath(transform.position);
        }
        
        public virtual void OnKill(GameObject victim) {
            kills++;
            cc.SetKillCounter(kills);
        }
    }
}
