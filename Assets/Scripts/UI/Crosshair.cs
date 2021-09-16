using UnityEngine;

namespace UI {
    public class Crosshair : MonoBehaviour {
        public Transform anchor;
        public float maxDistance = 200f;
        public UnityEngine.Camera gameCamera;

        public void AimCrosshair(Vector2 mousePos) {
            if (Vector2.Distance(anchor.position, gameCamera.ScreenToWorldPoint(mousePos)) < maxDistance) {
                Vector3 aimPos = gameCamera.ScreenToWorldPoint(mousePos);
                aimPos.z = 0;
                transform.position = aimPos;
            } else {
                Vector2 aimPos = gameCamera.ScreenToWorldPoint(mousePos);
                Vector3 anchorPos = anchor.position;
                
                Vector2 aimDir = ((Vector2) anchorPos) - aimPos;
                transform.position = ((Vector2) anchorPos) + (-aimDir.normalized * maxDistance);
            }
        }
    }
}