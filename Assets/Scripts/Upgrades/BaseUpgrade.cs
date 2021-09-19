using System;
using Assets.Scripts.Controller;
using System.Collections;
using Assets.Scripts.Controller;
using Items;
using Player;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Upgrades {
    public enum UpgradeAttribute {
        Health,
        Stamina,
        WeaponDamage,
        WeaponSpeed,
        WeaponRange,
        BulletSpeed,
        BulletSize,
        CritChance,
        PassThrough,
    }

    public enum Method {
        Add,
        Multiply,
        Boolean
    }
    
    public class BaseUpgrade : BaseInteractable {
        public UpgradeAttribute[] upgradeAttributes;
        public Method[] methods;
        public float[] values;
        public string description;

        private Light2D m_Light;
        private float m_TransitionTime;
        public float m_TargetIntensity;
        private Vector2 m_MinMaxIntensity = new Vector2(0, 2.25f);

        public void Start() {
            m_Light = GetComponent<Light2D>();
        }

        // This method is nasty. But I don't care enough to do another way
        private void OnInteract(PlayerController player) {
            for (int i = 0; i < upgradeAttributes.Length; i++) {
                float value = 0;
                
                switch (upgradeAttributes[i]) {
                    case UpgradeAttribute.Health:
                        value = player.playerStats.minMaxHealth.y;
                        player.playerStats.minMaxHealth.y = (int) DoMethod(methods[i], value, values[i]);
                        player.playerStats.health += (int) value;
                        break;
                    case UpgradeAttribute.Stamina:
                        value = player.playerStats.maxStamina;
                        player.playerStats.maxStamina = (int) DoMethod(methods[i], value, values[i]);
                        break;
                    case UpgradeAttribute.WeaponDamage:
                        value = player.currentWeapon.weaponStats.bulletStats.bulletDamage;
                        player.currentWeapon.weaponStats.bulletStats.bulletDamage =
                            DoMethod(methods[i], value, values[i]);
                        break;
                    case UpgradeAttribute.WeaponSpeed:
                        value = player.currentWeapon.weaponStats.fireRate;
                        player.currentWeapon.weaponStats.fireRate = DoMethod(methods[i], value, values[i]);
                        break;
                    case UpgradeAttribute.WeaponRange:
                        value = player.currentWeapon.weaponStats.bulletStats.lifetime;
                        player.currentWeapon.weaponStats.bulletStats.lifetime = DoMethod(methods[i], value, values[i]);
                        break;
                    case UpgradeAttribute.BulletSpeed:
                        value = player.currentWeapon.weaponStats.bulletStats.bulletSpeed;
                        player.currentWeapon.weaponStats.bulletStats.bulletSpeed =
                            DoMethod(methods[i], value, values[i]);
                        break;
                    case UpgradeAttribute.PassThrough:
                        player.currentWeapon.weaponStats.bulletStats.passThroughEnemies = ((int) value == 1);
                        break;
                    case UpgradeAttribute.BulletSize:
                        value = player.currentWeapon.weaponStats.bulletStats.bulletSize;
                        player.currentWeapon.weaponStats.bulletStats.bulletSize =
                            DoMethod(methods[i], value, values[i]);
                        break;
                    case UpgradeAttribute.CritChance:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            
            Destroy(gameObject);
        }

        private float DoMethod(Method method, float value, float modifier) {
            if (method == Method.Add) {
                return value + modifier;
            } else {
                return value * modifier;
            }
        }

        public override void Interact(PlayerController player) {
            Audio.controller.PickupUpgrade(transform.position);
            OnInteract(player);
        }
    }
}
