using Assets.Scripts.Controller;
using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DeathScreen : MonoBehaviour
{
    public static DeathScreen main;

    [SerializeField] private TextMeshProUGUI m_TimeSurvived;
    [SerializeField] private TextMeshProUGUI m_FlooredCleared;
    [SerializeField] private TextMeshProUGUI m_Kills;
    [SerializeField] private GameObject m_Screen;

    [SerializeField] private float m_TimeScoreWait = 1;

    private void Start() => main = this;

    public void Trigger() {
        ResetVals();
        Enable();
        LevelController.controller.ClearLevel();
        StartCoroutine(Display());
    }

    private void ResetVals() {
        m_TimeSurvived.text = "0";
        m_FlooredCleared.text = "0";
        m_Kills.text = "0";
    }

    public void Enable() => m_Screen.SetActive(true);

    public void Disable() {
        ResetVals();
        m_Screen.SetActive(false);
    }

    private IEnumerator Display() {
        yield return new WaitForSeconds(m_TimeScoreWait);
        yield return CountUp(m_TimeSurvived, LevelController.controller.secondsCount);
        yield return new WaitForSeconds(m_TimeScoreWait);
        yield return CountUp(m_FlooredCleared, LevelController.controller.currentLevel, true);
        yield return new WaitForSeconds(m_TimeScoreWait);
        yield return CountUp(m_Kills, PlayerController.player.playerStats.kills, true);
    }

    private IEnumerator CountUp(TextMeshProUGUI text, float number, bool forceInt = false) {
        float displayScore;
        float t = 0;
        float start = 0;
        float end = number;
        float moveDuration = 1;

        while (t < moveDuration) {
            t += Time.deltaTime;
            displayScore = Mathf.Lerp(start, end, t / moveDuration);
            if (forceInt) { displayScore = (int)Math.Round(displayScore); }
            text.text = displayScore.ToString();
            yield return null;
        }
    }
}
