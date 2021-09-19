using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class TimeCounter : MonoBehaviour
    {
        private TextMeshProUGUI timeCounter;

        private void Start()
        {
            timeCounter = GetComponent<TextMeshProUGUI>();
        }

        public void UpdateTimer(int secondsCount) {
            Debug.Log(secondsCount);
            string secs = "" + secondsCount % 60;
            if (secondsCount % 60 < 10) {
                secs = "0" + secs;
            }
            
            timeCounter.text = (secondsCount / 60) + ":" + secs;    
        }
        
    }
}