using UnityEngine;

namespace UI {
    public class Crosshair : MonoBehaviour {
        public Transform anchor;
        public float maxDistance = 2f;

        public void AimCrosshair(Vector2 mousePos) {
            if (Vector2.Distance(anchor.position, mousePos) < maxDistance) {
                transform.position = mousePos;
            } else {
                
            }
        }
    }
}