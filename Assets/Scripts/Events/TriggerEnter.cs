using System;
using Player;
using UnityEngine;
using UnityEngine.Events;

namespace Events {
    public class TriggerEnter : MonoBehaviour {
        public UnityEvent enterEvent;
        public GameObject errorObject;
        
        private void OnTriggerEnter2D(Collider2D other) {
            if (!other.CompareTag("Player")) {
                return;
            }

            if (PlayerController.player.currentWeapon == null) {
                errorObject.SetActive(true);
                return;
            }
            
            enterEvent.Invoke();
        }
    }
}
