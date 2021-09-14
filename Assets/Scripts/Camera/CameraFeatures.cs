using System.Collections;
using Cinemachine;
using Player;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Camera {
    public class CameraFeatures : MonoBehaviour {
        public static CameraFeatures mainFeature;
        public CinemachineVirtualCamera cineCamera;
        public PixelPerfectCamera pixelPerfectCamera;
        public RectTransform[] borders;
        public float borderVelocity = 1;
        public float intensityScale = 1;

        private float m_ShakeTime = 0;
        private float m_TotalShakeTime = 0;
        private float m_StartingShakeIntensity = 0;
        private float m_BorderMove = 0;
        private float m_BorderTime = 0;
        private float m_OriginalZoom = 100;
        private float m_CurrentZoom = 0;
        
        private void Start() {
            mainFeature = this;
            m_CurrentZoom = m_OriginalZoom;
        }
        
        private void Update() {
            if (m_ShakeTime > 0) {
                m_ShakeTime -= Time.deltaTime;
                CinemachineBasicMultiChannelPerlin perlin = cineCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
                perlin.m_AmplitudeGain = Mathf.Lerp(m_StartingShakeIntensity, 0, 1 - (m_ShakeTime / m_TotalShakeTime));
                if (m_ShakeTime <= 0) {
                    perlin.m_AmplitudeGain = 0;
                }
            }

            if (m_BorderTime > 0) {
                m_BorderTime -= Time.deltaTime;
                m_BorderMove = Mathf.SmoothDamp(m_BorderMove, 0, ref borderVelocity, 1 - (1 - m_BorderTime));
                m_CurrentZoom = Mathf.Lerp(m_CurrentZoom, m_OriginalZoom, 1 - m_BorderTime);
                // It goes Top, Right, Bottom, Left. EVEN if the code is top bottom right left...
                // I don't know either...
                borders[0].SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, m_BorderMove);
                borders[1].SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, m_BorderMove);
                borders[2].SetInsetAndSizeFromParentEdge(RectTransform.Edge.Right, 0, m_BorderMove);
                borders[3].SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, m_BorderMove);

                pixelPerfectCamera.assetsPPU = (int) m_CurrentZoom;
            }
        }

        public void ZoomTo(Transform target, float zoomTime = -1) {
            cineCamera.Follow = target;

            if (zoomTime > 0) {
                StartCoroutine(ReturnToPLayer(zoomTime));
            }
        }
        
        public void ShakeCamera(float scale, float time) {
            scale *= intensityScale;
            CinemachineBasicMultiChannelPerlin perlin = cineCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
            perlin.m_AmplitudeGain = scale;
            m_StartingShakeIntensity = scale;
            m_ShakeTime = time;
            m_TotalShakeTime = time;
            m_BorderMove += 9 * scale;
            if (m_BorderMove > 150) {
                m_BorderMove = 150;
            }
            m_BorderTime = 1;
            m_CurrentZoom += scale;
            if (m_CurrentZoom < 75) {
                m_CurrentZoom = 75;
            }
        }

        private IEnumerator ReturnToPLayer(float time) {
            yield return new WaitForSeconds(time);

            cineCamera.Follow = PlayerController.player.transform;
        }
    }
}
