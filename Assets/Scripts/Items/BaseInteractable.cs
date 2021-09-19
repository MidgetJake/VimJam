using Player;
using UI;
using UnityEngine;

namespace Items {
    public abstract class BaseInteractable : MonoBehaviour {
        public string title = "Item";
        public string actionDesc = "Interact";
        public bool needsGrab = true;
        
        public abstract void Interact(PlayerController player);

        protected void OnTriggerEnter2D(Collider2D other) {
            if (!other.transform.CompareTag("Player")) { return; }

            PlayerController player = other.transform.GetComponent<PlayerController>();
            if (needsGrab) {
                TooltipWindow.main.SetDetails(title, actionDesc);
                player.OnEnterInteractable(this);
            } else {
                Interact(player);
            }
        }

        protected void OnTriggerExit2D(Collider2D other) {
            if (!other.transform.CompareTag("Player")) {
                return;
            }
            
            PlayerController player = other.transform.GetComponent<PlayerController>();
            player.OnExitInteractable(this);
            TooltipWindow.main.ToggleVisible(false);
        }
    }
}
