using Items;
using Player;
using UnityEngine;

namespace Weapons {
    public class WeaponDrop : BaseInteractable {
        public BaseWeapon weapon;

        public override void Interact(PlayerController player) {
            BaseWeapon newWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
            player.PickupWeapon(newWeapon);
            Destroy(gameObject);
        }
    }
}
