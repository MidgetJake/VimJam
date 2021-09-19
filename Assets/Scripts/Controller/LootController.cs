using Managers;
using UnityEngine;
using Upgrades;
using Weapons;

namespace Controller {
    public enum DropType {
        Weapon,
        Upgrade
    }
    
    public class LootController : MonoBehaviour {
        public static LootController main;
        public Transform parent;
        public BaseUpgrade[] availableUpgrades;
        public BaseWeapon[] availableWeapons;
        public float upgradeChance = .75f;

        public void Start() => main = this;
        
        public (GameObject, DropType) GetLoot() {
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
