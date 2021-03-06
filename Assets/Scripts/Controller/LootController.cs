using Managers;
using Player;
using UnityEngine;
using Upgrades;
using Weapons;

namespace Controller {
    public enum DropType {
        Weapon,
        Upgrade,
        Health,
        Ammo
    }
    
    public class LootController : MonoBehaviour {
        public static LootController main;
        public Transform parent;
        public BaseUpgrade[] availableUpgrades;
        public BaseWeapon[] availableWeapons;
        public HealItem healItem;
        public AmmoItem ammoItem;
        public float upgradeChance = .75f;
        public float healthChance = .3f;
        public float ammoChance = .4f;

        public void Start() => main = this;
        
        public (GameObject, DropType) GetLoot(bool skipPickups = false) {
            if (!skipPickups) {
                if (RandomManager.GetFloat() < healthChance) {
                    return (healItem.gameObject, DropType.Health);
                }

                if (RandomManager.GetFloat() < ammoChance && PlayerController.player.extraWeapon != null) {
                    return (ammoItem.gameObject, DropType.Ammo);
                }
            }

            if (RandomManager.GetFloat() > upgradeChance) {
                return (GetWeapon().gameObject, DropType.Weapon);
            } else {
                return (GetUpgrade().gameObject, DropType.Upgrade);
            }
        }

        public BaseUpgrade GetUpgrade() {
            return RandomManager.ItemFromArray(availableUpgrades);
        }

        public BaseWeapon GetWeapon() {
            return RandomManager.ItemFromArray(availableWeapons);
        }
    }
}
