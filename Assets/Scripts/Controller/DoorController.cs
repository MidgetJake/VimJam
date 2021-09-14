using UnityEngine;

public class DoorController : MonoBehaviour
{
    public bool doorLocked = true;

    public void Unlock() {
        doorLocked = false;
        GetComponent<Animation>().Play();
    }
}
