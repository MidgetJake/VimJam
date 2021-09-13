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

        private void Update()
        {
        }
        
    }
}