using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class PlayerController : MonoBehaviour
{
    public TextMeshProUGUI KillCounter;

    private int kills;
    // Start is called before the first frame update
    void Start()
    {
        SetKillCounter();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetKillCounter()
    {
        KillCounter.text = "Kills:" + kills.ToString();
    }
}
