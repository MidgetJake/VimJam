using Events;
using Player;
using UnityEngine;
using UnityEngine.AI;

namespace Enemies {
    [RequireComponent(typeof(BaseStats))]
    public class BaseEnemy : BaseStats {
        
        private LineRenderer m_Laser;
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

            if (m_ShowLaser) { m_Laser = GetComponent<LineRenderer>(); }
            m_AI = GetComponent<EnemyAI>();

            eventHandler = GetComponent<EventsHandler>();
        }

        public void Update() {
            if (!isActiveAndEnabled) { return; }

            if (m_AI.CurrentTarget == null) { return; }

            m_Target = m_AI.CurrentTarget;
            m_Target.position = m_AI.CurrentTarget.position;
            if (m_ShowLaser) { UpdateLaserPointer(); }

            if (!m_CanMove && m_FreezeOnTarget) { return; }
        }

        public override void TakeDamage(float damage = 1) { base.TakeDamage(damage); }

        public override void Death() {
            m_Laser.enabled = false;
            enabled = false;
            m_AI.enabled = false;
        }

        private void UpdateLaserPointer() {
            m_Laser.SetPosition(0, transform.position);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.forward);
            if (hit) {
                m_Laser.SetPosition(1, hit.point);
                m_CanMove = hit.transform.gameObject.layer != LayerMask.NameToLayer("Character");
                return;
            }

            m_CanMove = true;
        }
    }
}