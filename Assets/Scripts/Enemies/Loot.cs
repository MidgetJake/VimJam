using UnityEngine;

namespace Assets.Scripts.Enemies {
    public class Loot : MonoBehaviour {
        public void Drop() {
            // EXAMPLES
            // PrefabController.controller.GrabRandomUpgrade;
            // PrefabController.controller.GrabRandomWeapon;
            GameObject loot = new GameObject();
            loot.transform.position = transform.position;
        }
    }
}
