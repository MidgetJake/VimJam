using Assets.Scripts.Controller;
using Items;
using Player;
using UnityEngine;

namespace Weapons {
    public class WeaponDrop : BaseInteractable {
        public BaseWeapon weapon;

        [SerializeField] private Sprite m_DefaultWeaponIcon;
        [SerializeField] private SpriteRenderer m_Renderer;

        public void SetWeapon(BaseWeapon newWeapon) {
            weapon = newWeapon;
            title = newWeapon.title;
            actionDesc = newWeapon.description;
            m_Renderer.sprite = newWeapon.icon ?? m_DefaultWeaponIcon;
        }

        public override void Interact(PlayerController player) {
            Audio.controller.PickupWeapon(transform.position);
            BaseWeapon newWeapon = Instantiate(weapon, transform.position, Quaternion.identity);
            player.PickupWeapon(newWeapon);
            Destroy(gameObject);
        }
    }
}
