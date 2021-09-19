using System;
using Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StaminaController : MonoBehaviour
    {

        [SerializeField] private Image StaminaBarSlider = null;

        public void UpdateStaminaBar(float currstamina, float maxstamina)
        {
            StaminaBarSlider.fillAmount = currstamina / maxstamina;
        }
        
    }

}

