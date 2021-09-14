using System.Collections;
using Camera;
using UnityEngine;

namespace Weapons {
    public class BaseWeapon : MonoBehaviour {
        public Transform followTransform;
        public float followSpeed = 0.75f;
        public Transform firePoint;
        public BaseBullet bullet;
        public BaseWeaponStats weaponStats;
        public bool isDefault = false;
        public int maxAmmo = 100;
        public int currAmmo = 100;
        public bool isEnemyWeapon = false;

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
            if (m_FireCooldown > 0) {
                return;
            }
            
            m_FireCooldown = 1 / (float) weaponStats.fireRate;

            Vector2 aimDir = -(((Vector2)firePoint.position) - aimVector).normalized;
            float totalDelay = 0;

            for (int i = 0; i < weaponStats.bulletCount; i++) {
                if (!isDefault) {
                    if (currAmmo <= 0) {
                        return;
                    }
                    currAmmo--;
                }
                
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
            newBullet.transform.localScale *= weaponStats.bulletStats.bulletSize;
            newBullet.stats = weaponStats.bulletStats;
            newBullet.isEnemyGun = isEnemyWeapon;
            newBullet.moveVector = fireDir;
        }
    }
}
