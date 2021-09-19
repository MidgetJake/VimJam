using Controller;
using UnityEngine;
using Weapons;

namespace Upgrades {
    public class RoomSpawner : MonoBehaviour {
        public WeaponDrop drop;

        public void SpawnItem() {
            (GameObject, DropType) loot = LootController.main.GetLoot(true);

            switch (loot.Item2) {
                case DropType.Weapon:
                    WeaponDrop weapon = Instantiate(drop, LootController.main.parent, true);
                    weapon.transform.position = transform.position;
                    weapon.SetWeapon(loot.Item1.GetComponent<BaseWeapon>());
                    break;
                case DropType.Upgrade:
                    GameObject upgrade = Instantiate(loot.Item1, LootController.main.parent, true);
                    upgrade.transform.position = transform.position;
                    break;
            }
        }

    }
}