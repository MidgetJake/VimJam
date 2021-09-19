using TMPro;
using UnityEngine;

namespace UI
{
    public class KillCounter : MonoBehaviour
    {
        private TextMeshProUGUI killCounter;
        private void Start()
        {
            killCounter = GetComponent<TextMeshProUGUI>();
        }
        public void SetKillCounter(int kills)
        {
            killCounter.text = kills.ToString();
        }
    }
}