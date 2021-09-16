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

    public class BackgroundAudio : MonoBehaviour {
        [Header("Settings")]
        [SerializeField] private int m_ChanceOfBreak = 25;

        [Header("Game Status")]
        public bool gameIsPlaying = false;
        public bool bossMode = false;

        [Header("Audio Setup")]
        public Music music;

        private AudioSource m_AudioSource;

        private static BackgroundAudio controller;

        public void Start() {
            controller = this;
            m_AudioSource = GetComponent<AudioSource>();
            PlayIntro();
        }

        public IEnumerator Play(AudioClip clip) {
            m_AudioSource.clip = clip;
            m_AudioSource.Play();
            yield return new WaitForSeconds(clip.length);

            if (gameIsPlaying) {
                if (bossMode) { PlayBoss(); } 
                else {
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
    }
}
