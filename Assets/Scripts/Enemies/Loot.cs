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
            
            if (loot.Item2 == DropType.Weapon) {
                WeaponDrop item = Instantiate(drop, LootController.main.parent, true);
                item.weapon = loot.Item1.GetComponent<BaseWeapon>();
                item.transform.position = transform.position;
            } else {
                GameObject item = Instantiate(loot.Item1, LootController.main.parent, true);
                item.transform.position = transform.position;
            }
        }
    }
}
