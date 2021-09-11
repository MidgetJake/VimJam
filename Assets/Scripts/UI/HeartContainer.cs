using System;
using System.Collections;
using UnityEngine;

namespace UI
{
    public class HeartContainer : MonoBehaviour
    {
        public GameObject heartSprite;
        public GameObject brokenHeartSprite;

        private void Start()
        {
            StartCoroutine(TestInFive());
        }

        public void UpdateHealth(int maxHealth, int currHealth)
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
            for (int i = 0; i < maxHealth; i++) {
                if (i + 1 <= currHealth)
                {
                    GameObject heart = Instantiate(heartSprite, transform.position, Quaternion.identity);
                    heart.transform.SetParent(transform);
                }
                else
                {
                    GameObject heart = Instantiate(brokenHeartSprite, transform.position, Quaternion.identity);
                    heart.transform.SetParent(transform);
                }
            }

        }
        private IEnumerator TestInFive() {
            yield return new WaitForSecondsRealtime(5);
            UpdateHealth(5, 3);
        }
    }
}