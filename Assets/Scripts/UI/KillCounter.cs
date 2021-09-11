using TMPro;
using UnityEngine;

namespace UI
{
    public class KillCounter : MonoBehaviour
    {
        public TextMeshProUGUI killCounter;
        
        void SetKillCounter(int kills)
        {
            killCounter.text = "Kills:" + kills.ToString();
        }
    }
}