using Assets.Scripts.Controller;
using Assets.Scripts.Enemies;
using Controller;
using Events;
using Items;
using Managers;
using Player;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies {
    [RequireComponent(typeof(BaseStats))]
    public class BaseEnemy : BaseStats {
        [HideInInspector]
        public bool isActive = true;

        public BaseInteractable[] guaranteedDrops = new BaseInteractable[0];

        private bool m_CanMove = true;
        private EnemyAI m_AI;

        [HideInInspector] public EventsHandler eventHandler;

        [SerializeField] private bool m_ShowLaser;
        [SerializeField] private Transform m_Target;
        [SerializeField] private bool m_FreezeOnTarget = false;

        public void Start() {
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.updateRotation = false;
            agent.updateUpAxis = false;

            m_AI = GetComponent<EnemyAI>();
            eventHandler = GetComponent<EventsHandler>();

            base.Start();
        }

        public void Update() {
            if (!isActive) { return; }
            if (!isActiveAndEnabled) { return; }

            if (m_AI.CurrentTarget == null) { return; }

            m_Target = m_AI.CurrentTarget;
            m_Target.position = m_AI.CurrentTarget.position;

            if (!m_CanMove && m_FreezeOnTarget) { return; }
        }

        public override void TakeDamage(float damage = 1) { 
            if (isActive) { base.TakeDamage(damage); } 
        }

        public override void Death() {
            enabled = false;
            m_AI.enabled = false;
            LevelController.controller.RecordDeath(gameObject, m_AI.isBoss);
            GetComponent<Loot>().Drop();
            main.OnKill(gameObject);
            foreach (BaseInteractable interactable in guaranteedDrops) {
                BaseInteractable inter = Instantiate(interactable, LootController.main.parent, true);
                inter.transform.position = transform.position;
            }
            
            Destroy(gameObject);
        }
    }
}