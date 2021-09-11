using UnityEngine;

namespace Weapons {
    public class BaseWeapon : MonoBehaviour {
        public Transform followTransform;
        public float followSpeed = 0.75f;
        public Transform firePoint;
        public BaseBullet bullet;
        public BaseWeaponStats weaponStats;

        private float m_FireCooldown;
        
        private void FixedUpdate() {
            transform.position = Vector2.Lerp(transform.position, followTransform.position, followSpeed);
        }

        public void Fire(Vector2 aimVector) {
            if (m_FireCooldown > 0) {
                return;
            }

            m_FireCooldown = 1 / (float) weaponStats.fireRate;
            BaseBullet newBullet = Instantiate(bullet, firePoint.position, Quaternion.identity);
            newBullet.stats = weaponStats.bulletStats;
        }
    }
}