using System;
using TMPro;
using UnityEngine;

namespace UI
{
    public class TimeCounter : MonoBehaviour
    {
        private TextMeshProUGUI timeCounter; 
        private float secondsCount; 
        private int minuteCount;
        
        

        private void Start()
        {
            timeCounter = GetComponent<TextMeshProUGUI>();
        }

        private void Update()
        {
            UpdateTimer();
        }

        public void UpdateTimer()
        {
            secondsCount += Time.deltaTime;
            timeCounter.text = minuteCount +"m:"+(int)secondsCount + "s";
            if(secondsCount >= 60){
                minuteCount++;
                secondsCount = 0;
            }    
        }
        
    }
}