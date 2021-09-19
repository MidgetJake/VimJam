using TMPro;
using UnityEngine;

namespace UI
{
    public class FloorCounter : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI floorCounter;

        private void Start()
        {
            floorCounter = GetComponent<TextMeshProUGUI>();
        }
        
        public void SetFloorCounter(int floors)
        {
            floorCounter.text = "Floor: " + floors.ToString();
        }
    }
}