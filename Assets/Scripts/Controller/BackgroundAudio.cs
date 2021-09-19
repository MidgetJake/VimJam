using Managers;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controller {
    [Serializable]
    public class Music {
        public AudioClip intro;

        public AudioClip[] musicBreak;
        public AudioClip[] track;
        public AudioClip[] boss;
    }

    [Serializable]
    public class TempoChanges {
        public float startMenuTempo;
        public float mainTempo;
        public float bossTempo;
    }

    public enum MusicType {
        StartMenu,
        Main,
        Boss,
    }

    public class BackgroundAudio : MonoBehaviour {
        [Header("Settings")]
        [SerializeField] private int m_ChanceOfBreak = 25;
        [SerializeField] private Vector2 m_MinMaxVolume = new Vector2(.4f, .5f);
        [SerializeField] private float m_MaxDistance = 10;

        public static BackgroundAudio controller;

        [Header("Game Status")]
        public bool muteBackgroundMusic = false;
        public bool gameIsPlaying = false;
        public bool bossMode = false;
        public MusicType currentActive;

        [Header("Audio Setup")]
        public TempoChanges tempo;
        public Music music;

        private AudioSource m_AudioSource;
        private float m_TargetVolume;
        private float m_TargetPitch;
        private bool m_TransitionVolume;
        private bool m_TransitionPitch;

        public void Start() {
            controller = this;
            m_AudioSource = GetComponent<AudioSource>();
            currentActive = MusicType.StartMenu;
            PlayIntro();
        }

        public void Update() {
            if (m_TransitionVolume) {
                m_AudioSource.volume = Mathf.Lerp(m_AudioSource.volume, m_TargetVolume, .01f);
                if (m_AudioSource.volume == m_TargetVolume) { m_TransitionVolume = false; }
            }

            if (m_TransitionPitch) {
                m_AudioSource.pitch = Mathf.Lerp(m_AudioSource.pitch, m_TargetPitch, .01f);
                if (m_AudioSource.pitch == m_TargetPitch) { m_TransitionPitch = false; }
            }
        }

        public void IntensityAudioVolume(float? distance) {
            if (!distance.HasValue) { m_TargetVolume = m_MinMaxVolume.x; } 
            else {
                float percentage = distance.Value / m_MaxDistance;
                if (percentage > 1) { percentage = 1; }
                else if (percentage < 0) { percentage = 0; }
                m_TargetVolume = m_MinMaxVolume.y - ((percentage * m_MinMaxVolume.y) - m_MinMaxVolume.x);
            }
            m_TransitionVolume = true;
        }

        public void Mute() => m_AudioSource.volume = 0;
        public void Unmute() => m_AudioSource.volume = m_MinMaxVolume.x;

        public IEnumerator Play(AudioClip clip) {
            m_AudioSource.clip = clip;
            m_AudioSource.Play();

            GetClipLengthFromTempoChange();
            
            // Waiting for the music to end
            while (m_AudioSource.isPlaying) { yield return null; }

            if (gameIsPlaying) {
                if (bossMode) { currentActive = MusicType.Boss; PlayBoss(); } 
                else {
                    currentActive = MusicType.Main;
                    if (RandomManager.RollChances(m_ChanceOfBreak)) { PlayBreak(); }
                    else { PlayTrack(); }
                }
            } else { PlayBreak(); }
        }

        #region
        public void PlayIntro() => StartCoroutine(Play(music.intro));
        public void PlayBreak() => StartCoroutine(Play(RandomManager.ItemFromArray(music.musicBreak)));
        public void PlayTrack() => StartCoroutine(Play(RandomManager.ItemFromArray(music.track)));
        public void PlayBoss() => StartCoroutine(Play(RandomManager.ItemFromArray(music.boss)));
        #endregion

        private void GetClipLengthFromTempoChange() {
            float currentTempo;
            switch (currentActive) {
                case MusicType.StartMenu:
                    currentTempo = tempo.startMenuTempo;
                    break;
                case MusicType.Boss:
                    currentTempo = tempo.bossTempo;
                    break;
                default:
                    currentTempo = tempo.mainTempo;
                    break;
            }

            if (m_TargetPitch != currentTempo) { 
                m_TransitionPitch = true;
                m_TargetPitch = currentTempo;
            }
        }
    }
}
