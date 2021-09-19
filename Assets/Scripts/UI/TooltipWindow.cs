using TMPro;
using UnityEngine;

namespace UI {
    public class TooltipWindow : MonoBehaviour {
        public static TooltipWindow main;
        public TextMeshProUGUI titleText;
        public TextMeshProUGUI descriptionText;
        public GameObject parent;

        private void Start() {
            main = this;
        }
        
        public void ToggleVisible(bool visible) {
            parent.SetActive(visible);
        }

        public void SetDetails(string title, string desc) {
            titleText.text = title;
            descriptionText.text = desc;
            ToggleVisible(true);
        }
    }
}
