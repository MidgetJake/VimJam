using System;
using Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StaminaController : MonoBehaviour
    {

        private Image StaminaBarSlider = null;

        private void Start()
        {
        }

        public void UpdateStaminaBar(float currstamina, float maxstamina)
        {
            StaminaBarSlider.fillAmount = currstamina / maxstamina;
        }
        
    }

}

