using TMPro;
using UnityEngine;

namespace UI
{
    public class FloorCounter : MonoBehaviour
    {
        private TextMeshProUGUI floorCounter;

        private void Start()
        {
            floorCounter = GetComponent<TextMeshProUGUI>();
        }
        
        void SetFloorCounter(int floors)
        {
            floors++;
            floorCounter.text = "Floor:" + floors.ToString();
        }
    }
}