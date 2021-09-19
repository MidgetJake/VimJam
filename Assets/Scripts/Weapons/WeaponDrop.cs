using Assets.Scripts.Controller;
using Items;
using Player;
using UnityEngine;

namespace Weapons {
    public class WeaponDrop : BaseInteractable {
        public BaseWeapon weapon;

        public void SetWeapon(BaseWeapon newWeapon) {
            weapon = newWeapon;
            title = newWeapon.title;
            actionDesc = newWeapon.description;
        }

        public override void Interact(PlayerController player) {
            Audio.controller.PickupWeapon(transform.position);
            BaseWeapon newWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
            player.PickupWeapon(newWeapon);
            Destroy(gameObject);
        }
    }
}
