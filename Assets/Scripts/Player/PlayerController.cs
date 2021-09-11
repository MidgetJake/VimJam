using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private HeartContainer hc;
    public void DamagePlayer(int dmg)
    {
        if (currHealth > 0)
        {
            currHealth -= dmg;
                
        }
    }
}
