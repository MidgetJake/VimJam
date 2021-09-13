using Assets.Scripts.Enemies;
using Events;
using Player;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies {
    [RequireComponent(typeof(BaseStats))]
    public class BaseEnemy : BaseStats {
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
            if (!isActiveAndEnabled) { return; }

            if (m_AI.CurrentTarget == null) { return; }

            m_Target = m_AI.CurrentTarget;
            m_Target.position = m_AI.CurrentTarget.position;

            if (!m_CanMove && m_FreezeOnTarget) { return; }
        }

        public override void TakeDamage(float damage = 1) { base.TakeDamage(damage); }

        public override void Death() {
            enabled = false;
            m_AI.enabled = false;
            GetComponent<Loot>().Drop();
            Destroy(gameObject);
        }
    }
}