using UnityEngine;

namespace Weapons {
    public class BaseBullet : MonoBehaviour {
        public BaseBulletStats stats;
        public Vector2 moveVector;
        
        private Rigidbody2D m_Rigidbody2D;

        private void Start() {
            m_Rigidbody2D = GetComponent<Rigidbody2D>();
        }
        
        private void FixedUpdate() {
            Vector2 movement = (moveVector * (stats.bulletSpeed * Time.deltaTime));
            m_Rigidbody2D.MovePosition(m_Rigidbody2D.position + movement);
        }
    }
}