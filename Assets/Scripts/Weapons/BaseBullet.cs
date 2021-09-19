using Assets.Scripts.Controller;
using Events;
using System.Collections;
using Camera;
using Player;
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
            if (hitEnemey && isEnemyGun) { return; }
            
            if (hitEnemey) {
                Instantiate(impactEnemyParticle, transform.position, Quaternion.identity);
                if (Vector3.Distance(transform.position, PlayerController.player.transform.position) < 10) {
                    CameraFeatures.mainFeature.ShakeCamera(3, 0.7f);
                }
                // Do enemy hit stuffs
            } else {
                Instantiate(impactParticle, transform.position, Quaternion.identity);
            }

            EventsHandler eHandler = collider.gameObject.GetComponent<EventsHandler>();
            if (eHandler != null) {
                eHandler.OnDamage.Invoke(stats.bulletDamage);
                if (isEnemyGun && Vector3.Distance(transform.position, PlayerController.player.transform.position) < 10) {
                    CameraFeatures.mainFeature.ShakeCamera(1, 0.3f);
                }
            }
            
            Destroy(gameObject);
        }
        
        public void OnTriggerEnter2D(Collider2D collider) {
            if (collider.CompareTag("Floor")) { return; }
            if (!isEnemyGun && collider.CompareTag("Player")) { return; }

            // Play audio
            switch (collider.tag) {
                case "Player":
                    Audio.controller.PlayerDamage(transform.position);
                    break;
                case "Enemy":
                    Audio.controller.EnemyDamage(transform.position);
                    break;
                default:
                    Audio.controller.BulletOnHit(transform.position);
                    break;
            }

            /*if ((isEnemyGun && collider.CompareTag("Player")) || 
                (!isEnemyGun && collider.CompareTag("Enemy"))) {*/
            OnHit(collider.CompareTag("Enemy"), ref collider);
            // }
        }
    }
}
