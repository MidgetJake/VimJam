using Assets.Scripts.Controller;
using Player;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour {
    public static DeathScreen main;
    public float scale = 5;

    [SerializeField] private Image m_Blank;
    [SerializeField] private TextMeshProUGUI m_TimeSurvived;
    [SerializeField] private TextMeshProUGUI m_FlooredCleared;
    [SerializeField] private TextMeshProUGUI m_Kills;
    [SerializeField] private GameObject m_Screen;

    [SerializeField] private float m_TimeScoreWait = 1;
    private bool m_Going;
    private float m_Time = 0;
    private Color m_StartingColour;

    private void Start() {
        main = this;
        m_StartingColour = m_Blank.color;
    }

    private void Update() {
        if (m_Going) {
            m_Time += Time.deltaTime;
            Color color = m_Blank.color;
            color.a = Mathf.Lerp(color.a, 1, m_Time / .5f);
            m_Blank.color = color;

            if (m_Time >= .5f) {
                m_Going = false;
                m_Screen.SetActive(true);
                LevelController.controller.ClearLevel();
            }
        }
    }

    public void Trigger() {
        ResetVals();
        Enable();
        StartCoroutine(Display());
    }

    private void ResetVals() {
        m_TimeSurvived.text = "0";
        m_FlooredCleared.text = "0";
        m_Kills.text = "0";
        m_Blank.color = m_StartingColour;
    }

    public void Enable() {
        LevelController.controller.ClearEnemies();
        BackgroundAudio.controller.Mute();
        m_Time = 0;
        m_Going = true;
    }

    public void Disable() {
        ResetVals();
        m_Screen.SetActive(false);
        BackgroundAudio.controller.Unmute();
    }

    private IEnumerator Display() {
        yield return new WaitForSeconds(m_TimeScoreWait + 1.5f);
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
