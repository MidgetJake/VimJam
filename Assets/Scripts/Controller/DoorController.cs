using Assets.Scripts.Controller;
using UnityEngine;
using Upgrades;

namespace Controller {
    public class DoorController : MonoBehaviour
    {
        public bool doorLocked = true;

        [SerializeField] private RoomSpawner m_Spawner;
        [SerializeField] private bool m_IsElevator = false;

        public void Unlock() {
            doorLocked = false;
            if (m_IsElevator) { Audio.controller.ElevatorBell(transform.position); }
            GetComponent<Animation>().Play();

            if (m_Spawner != null) {
                m_Spawner.SpawnItem();
            }
        }
    }
}
