using TMPro;
using UnityEngine;
using Weapons;
using Image = UnityEngine.UI.Image;

namespace UI {
    public class AmmoCount : MonoBehaviour {
        public static AmmoCount main;
        public BaseWeapon defaultWeapon;
        public BaseWeapon extraWeapon;
        
        public Image extraWeaponImage;
        
        public TextMeshProUGUI extraAmmoCount;
        
        public GameObject defaultView;
        public GameObject extraView;
        public GameObject defaultSelected;
        public GameObject extraSelected;

        private bool m_HoldingDefault = true;

        private void Start() {
            main = this;
        }
        
        public void PickupWeapon(ref BaseWeapon weapon, bool isDefault = false) {
            if (isDefault) {
                defaultWeapon = weapon;
                defaultView.SetActive(true);
            } else {
                extraWeapon = weapon;
                extraView.SetActive(true);
                extraWeaponImage.sprite = extraWeapon.icon;
                extraAmmoCount.text = extraWeapon.currAmmo + "/" + extraWeapon.maxAmmo;
            }
        }

        public void UpdateAmmo() {
            if (extraWeapon == null) {
                return;
            }
            extraAmmoCount.text = extraWeapon.currAmmo + "/" + extraWeapon.maxAmmo;
        }

        public void SwapWeapon() {
            if (m_HoldingDefault) {
                defaultSelected.SetActive(false);
                extraSelected.SetActive(true);
            } else {
                defaultSelected.SetActive(true);
                extraSelected.SetActive(false);
            }

            m_HoldingDefault = !m_HoldingDefault;
        }
    }
}
