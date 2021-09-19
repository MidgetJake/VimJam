using Player;
using UnityEngine;

namespace Items {
    public abstract class BaseInteractable : MonoBehaviour {
        public string actionDesc = "Interact";
        public bool needsGrab = true;
        
        public abstract void Interact(PlayerController player);
        
        protected void OnTriggerEnter2D(Collider2D other) {
            if (!other.transform.CompareTag("Player")) { return; }

            PlayerController player = other.transform.GetComponent<PlayerController>();
            if (needsGrab) {
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
        }
    }
}
