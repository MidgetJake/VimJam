using Assets.Scripts.Controller;
using UnityEngine;
using UnityEngine.Events;

public class FinishLevel : MonoBehaviour
{
    [SerializeField] private UnityEvent endOfLevel;


    public void OnTriggerEnter2D(Collider2D collision) {
        Debug.Log("Trigger");
        if (!collision.CompareTag("Player")) { return; }
        endOfLevel.Invoke();
        LevelController.controller.NewLevel();
    }
}
