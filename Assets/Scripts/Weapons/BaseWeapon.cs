using System.Collections;
using UnityEngine;

namespace Weapons {
    public class BaseWeapon : MonoBehaviour {
        public Transform followTransform;
        public float followSpeed = 0.75f;
        public Transform firePoint;
        public BaseBullet bullet;
        public BaseWeaponStats weaponStats;

        [SerializeField] private bool m_IsEnemyGun;

        private float m_FireCooldown;

        private void Update() {
            if (m_FireCooldown > 0) {
                m_FireCooldown -= Time.deltaTime;
            }
        }
        
        private void FixedUpdate() {
            transform.position = Vector2.Lerp(transform.position, followTransform.position, followSpeed);
        }

        public void ChangeStats(BaseWeaponStats stats) => weaponStats = stats;

        public void Fire(Vector2 aimVector) {
            if (m_FireCooldown > 0) { return; }
            
            m_FireCooldown = 1 / (float) weaponStats.fireRate;

            Vector2 aimDir = -(((Vector2)firePoint.position) - aimVector).normalized;
            float totalDelay = 0;

            for (int i = 0; i < weaponStats.bulletCount; i++) {
                Vector2 fireDir = aimDir;
                float fireDelay = 0;
                
                if (weaponStats.bulletOffsets.Length > 0) {
                    // rotate aiming vector
                    fireDir = Quaternion.AngleAxis(weaponStats.bulletOffsets[i], Vector3.forward) * fireDir;
                }

                if (weaponStats.bulletFireDelay.Length > 0 && i <= weaponStats.bulletFireDelay.Length) {
                    fireDelay = totalDelay + weaponStats.bulletFireDelay[i];
                    totalDelay = fireDelay;
                }

                StartCoroutine(FireBullet(fireDir, fireDelay));
            }
        }

        private IEnumerator FireBullet(Vector2 fireDir, float delay = 0) {
            yield return new WaitForSecondsRealtime(delay);
            
            BaseBullet newBullet = Instantiate(bullet, firePoint.position, Quaternion.identity);
            newBullet.stats = weaponStats.bulletStats;
            newBullet.moveVector = fireDir;
            newBullet.isEnemyGun = m_IsEnemyGun;
        }
    }
}