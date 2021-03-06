using Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Weapons;

public class Target {
    public GameObject obj;
    public float distance;
    public bool inView;
}

namespace Enemies {
    public class EnemyAI : MonoBehaviour {
        [Header("Personality")]
        public bool canMove = true;
        public bool canShootWhileMoving = true;
        public bool isBoss = false;
        [Range(1, 7)] public float minDistanceFromPlayer;
        [Range(1, 50)] [SerializeField] private float m_DetectionRange = 10;
        [Tooltip("Seconds")]
        [Range(0, 50)] [SerializeField] private float m_DetectionRate = 1;
        public bool canForget = true;
        [Range(0, 10)] [SerializeField] private float m_TimeUntilForget = 2;
        [Tooltip("Seconds")]
        [Range(0, 10)] [SerializeField] private float m_ShootRate = .2f;

        [Header("Other")]
        public Transform CurrentTarget;
        private bool canShoot = false;
        private bool isMoving = false;
        private List<Target> m_ActiveTargets;
        private bool isForgetting = false;
        private bool isShooting = false;
        private NavMeshAgent agent;
        private EventsHandler m_EventHandler;
        private BaseEnemy m_BaseEnemy;

        [SerializeField] private bool m_Debug;

        public void Start() {
            StartCoroutine(Detection());
            agent = GetComponent<NavMeshAgent>();
            m_EventHandler = GetComponent<EventsHandler>();
            // Setting agent to correct spawn location
            agent.Warp(transform.position);
            m_BaseEnemy = GetComponent<BaseEnemy>();
            
            // Boss shouldn't be active until all enemies are dead
            m_BaseEnemy.isActive = !isBoss;
        }

        private void Update() {
            if (!m_BaseEnemy.isActive) { return; }
            if (!isActiveAndEnabled) { return; }
            if (CurrentTarget == null) { return; }

            LookAt();

            // Move closer if not close enough
            if (Vector2.Distance(transform.position, CurrentTarget.position) > minDistanceFromPlayer) { 
                if (canMove) { MoveToPlayer(); } 
            } else {
                agent.SetDestination(transform.position);
                isMoving = false;
            }
            
            if (!canShootWhileMoving && isMoving) { return; }
            if (!isShooting) { StartCoroutine(Shoot()); }
        }

        private void LookAt() {
            Vector3 dir = CurrentTarget.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

        private void MoveToPlayer() {
            isMoving = true;
            agent.SetDestination(CurrentTarget.position);
        }

        private IEnumerator Shoot() {
            if (isActiveAndEnabled) {
                isShooting = true;

                yield return new WaitForSeconds(m_ShootRate);

                if (CurrentTarget != null) { m_EventHandler.Attack(CurrentTarget.position); }

                if (CurrentTarget != null) { StartCoroutine(Shoot()); }
                else { isShooting = false; }
            }
        }

        private IEnumerator Detection() {
            if (isActiveAndEnabled) {
                yield return new WaitForSeconds(m_DetectionRate);

                CheckForCollisions();
                if (m_ActiveTargets.Count >= 1) { CheckForObstruction(); }
                else { StartForgetting(); }

                StartCoroutine(Detection());
            }
        }

        private IEnumerator Forget() {
            isForgetting = true;
            yield return new WaitForSeconds(m_TimeUntilForget);
            CurrentTarget = null;
            isForgetting = false;
        }

        private void CheckForObstruction() {
            List<Target> targetInSight = new List<Target>();

            foreach (var target in m_ActiveTargets) {
                // BOSS CAN SEE ALL
                if (isBoss) {
                    if (target.obj.CompareTag("Player")) { targetInSight.Add(target); }
                    continue;
                }

                bool obstructed = false;

                GameObject targetObj = target.obj;

                Vector3 direction = (targetObj.transform.position - transform.position).normalized;

                int mask = ~LayerMask.GetMask("Ignore Raycast");
                RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, m_DetectionRange, mask);
                if (hit) {
                    if (!hit.transform.CompareTag("Player")) { obstructed = true; }
                    else if (!hit.transform.gameObject.GetHashCode().Equals(targetObj.GetHashCode())) { obstructed = true; }
                }

                // Viewing the targetting
                if (m_Debug) { Debug.DrawLine(transform.position, targetObj.transform.position, obstructed ? Color.red : Color.green); }

                if (obstructed) { continue; }

                targetInSight.Add(target);
            }

            // If no targets in sight, then reset
            if (targetInSight.Count <= 0) {
                StartForgetting();
                return;
            }

            CurrentTarget = targetInSight[0].obj.transform;
        }

        private void StartForgetting() {
            if (isForgetting || CurrentTarget == null || !canForget) { return; }
            StartCoroutine(Forget());
        }

        private void CheckForCollisions() {
            m_ActiveTargets = new List<Target>();

            Collider2D[] collisions = Physics2D.OverlapCircleAll(transform.position, m_DetectionRange, 1 << LayerMask.NameToLayer("Character"));
            if (collisions.Length <= 0) { return; }

            foreach (var collision in collisions) {
                GameObject targetObj = collision.gameObject;
                Target target = new Target() {
                    obj = targetObj,
                    distance = Vector3.Distance(transform.position, targetObj.transform.position),
                };
                m_ActiveTargets.Add(target);
            }
        }
    }
}