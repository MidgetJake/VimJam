using Assets.Scripts.Controller;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool doorLocked = true;

    [SerializeField] private bool m_IsElevator = false;

    public void Unlock() {
        doorLocked = false;
        if (m_IsElevator) { Audio.controller.ElevatorBell(transform.position); }
        GetComponent<Animation>().Play();
    }
}
