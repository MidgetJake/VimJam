using Controller;
using Managers;
using UnityEngine;
using Weapons;

namespace Assets.Scripts.Enemies {
    public class Loot : MonoBehaviour {
        public WeaponDrop drop;
        
        public void Drop() {
            if (RandomManager.GetFloat() > .2f) {
                return;
            }
            
            (GameObject, DropType) loot = LootController.main.GetLoot();

            switch (loot.Item2) {
                case DropType.Weapon:
                    WeaponDrop weapon = Instantiate(drop, LootController.main.parent, true);
                    weapon.transform.position = transform.position;
                    weapon.SetWeapon(loot.Item1.GetComponent<BaseWeapon>());
                    break;
                case DropType.Upgrade:
                case DropType.Health:
                case DropType.Ammo:
                    GameObject upgrade = Instantiate(loot.Item1, LootController.main.parent, true);
                    upgrade.transform.position = transform.position;
                    break;
            }
        }
    }
}
