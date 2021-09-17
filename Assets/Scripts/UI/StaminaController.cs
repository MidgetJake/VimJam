using System;
using Player;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class StaminaController : MonoBehaviour
    {
        [SerializeField] private BaseStats bs;
        
        public bool hasRegenerated;
        public bool sprinting;

        [Range(0, 50)] private float staminaDrain = 0.5f;
        [Range(0, 50)] private float staminaRegen = 0.5f;

        private Image StaminaBarSlider = null;
        private Image StaminabarContainer = null;

        private void Start()
        {
            
        }

        private void Update()
        {
            if (sprinting)
            {
                if (bs.currStamina <= bs.maxStamina - 0.01)
                {
                    bs.currStamina += staminaRegen * Time.deltaTime;

                    if (bs.currStamina >= bs.maxStamina)
                    {
                        //set normal speed
                        UpdateStaminaBar(1);
                        hasRegenerated = true;
                    }
                }
            }
        }

        public void Sprinting()
        {
            if (hasRegenerated)
            {
                sprinting = true;
                bs.currStamina -= staminaDrain * Time.deltaTime;
                UpdateStaminaBar(1);

                if (bs.currStamina <= 0)
                {
                    hasRegenerated = false;
                    UpdateStaminaBar(0);
                }
            }
        }
        
        public void UpdateStaminaBar(int value)
        {
            StaminaBarSlider.fillAmount = bs.currStamina / bs.currStamina;
        }
        
    }

}

