using UnityEngine;

namespace Assets.Scripts.Controller {
    

    public class Sound {

        public AudioSource source;

        public void Play(AudioClip clip) {
            source.clip = clip;
            
            source.Play();
        }
    }
}
