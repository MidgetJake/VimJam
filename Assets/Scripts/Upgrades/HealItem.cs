using Items;
using Player;

namespace Upgrades {
    public class HealItem : BaseInteractable{
        public override void Interact(PlayerController player) {
            if (player.playerStats.health < player.playerStats.minMaxHealth.y) {
                player.playerStats.Heal(1);
                Destroy(gameObject);
            }
        }
    }
}