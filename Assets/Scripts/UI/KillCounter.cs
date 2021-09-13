﻿using TMPro;
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
        void SetKillCounter(int kills)
        {
            killCounter.text = "Kills:" + kills.ToString();
        }
    }
}