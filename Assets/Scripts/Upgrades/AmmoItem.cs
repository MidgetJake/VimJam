using Items;
using Player;

namespace Upgrades {
    public class AmmoItem : BaseInteractable {
        public int ammoAmount = 25;
        public override void Interact(PlayerController player) {
            if (player.extraWeapon == null) {
                return;
            }
            
            if (player.holdingDefault) {
                if (player.extraWeapon.currAmmo < player.extraWeapon.maxAmmo) {
                    player.extraWeapon.currAmmo += ammoAmount;
                }
                
                if (player.extraWeapon.currAmmo > player.extraWeapon.maxAmmo) {
                    player.extraWeapon.currAmmo = player.extraWeapon.maxAmmo;
                }
            } else {
                if (player.currentWeapon.currAmmo < player.currentWeapon.maxAmmo) {
                    player.currentWeapon.currAmmo += ammoAmount;
                }
                
                if (player.currentWeapon.currAmmo > player.currentWeapon.maxAmmo) {
                    player.currentWeapon.currAmmo = player.currentWeapon.maxAmmo;
                }
            }
            
            Destroy(gameObject);
        }
    }
}