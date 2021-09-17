using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class TimeCounter : MonoBehaviour
    {
        private TextMeshProUGUI timeCounter; 
        
        private int minuteCount;
        
        

        private void Start()
        {
            timeCounter = GetComponent<TextMeshProUGUI>();
        }

        public void UpdateTimer(int secondsCount)
        {
            timeCounter.text = minuteCount +"m:"+(int)secondsCount + "s";
            if(secondsCount >= 60){
                minuteCount++;
            }    
        }
        
    }
}