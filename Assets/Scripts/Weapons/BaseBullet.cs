using UnityEngine;

namespace Weapons {
    public class BaseBullet : MonoBehaviour {
        public BaseBulletStats stats;

        private Vector2 m_MoveVector;
        private Rigidbody2D m_Rigidbody2D;

        private void Start() {
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
        }
        
        private void FixedUpdate() {
            Vector2 movement = (m_MoveVector * (stats.bulletSpeed * Time.deltaTime));
            m_Rigidbody2D.MovePosition(m_Rigidbody2D.position + movement);
        }
    }
}