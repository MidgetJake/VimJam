using Events;
using System.Collections;
using UnityEngine;

namespace Weapons {
    public class BaseBullet : MonoBehaviour {
        public BaseBulletStats stats;
        public ParticleSystem impactParticle;
        public ParticleSystem impactEnemyParticle;
        public Vector2 moveVector;
        public bool isEnemyGun;
        
        private Rigidbody2D m_Rigidbody2D;
        

        private void Start() {
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
            StartCoroutine(DestroySelf());
        }
        
        private void FixedUpdate() {
            Vector2 movement = (moveVector * (stats.bulletSpeed * Time.deltaTime));
            m_Rigidbody2D.MovePosition(m_Rigidbody2D.position + movement);
        }

        private IEnumerator DestroySelf() {
            // Destroy the bullet after some seconds, don't want it lasting forever if it glitches outside
            yield return new WaitForSeconds(stats.lifetime);
            Destroy(gameObject);
        }

        private void OnHit(bool hitEnemey, ref Collider2D collider) {
            if (hitEnemey) {
                Instantiate(impactEnemyParticle, transform.position, Quaternion.identity);
                // Do enemy hit stuffs
            } else {
                Instantiate(impactParticle, transform.position, Quaternion.identity);
            }

            collider.gameObject.GetComponent<EventsHandler>().OnDamage.Invoke(stats.bulletDamage);

            Destroy(gameObject);
        }
        
        private void OnTriggerEnter2D(Collider2D collider) {
            if (collider.CompareTag("Floor")) { return; }

            if (isEnemyGun && collider.CompareTag("Enemy")) { return; }
            else if (!isEnemyGun && collider.CompareTag("Player")) { return; }
            
            OnHit(collider.CompareTag("Enemy"), ref collider);
        }
    }
}