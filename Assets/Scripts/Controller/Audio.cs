using Managers;
using System;
using UnityEngine;

namespace Assets.Scripts.Controller {

    [Serializable]
    public class SFX {
        public AudioClip elevatorBell;
        public AudioClip playerDeath;
        public AudioClip startGame;
        public AudioClip dodge;

        public AudioClip[] bulletOnHit;
        public AudioClip[] click;
        public AudioClip[] enemyDamage;
        public AudioClip[] playerDamage;
        public AudioClip[] shoot;
        public AudioClip[] upgradePickup;
        public AudioClip[] weaponPickup;
    }

    public class Audio : MonoBehaviour {
        [Header("Audio Setup")]
        public SFX sfx;
        [Range(0, 1)] public float volume = 1;

        private float m_OldVolume;

        public static Audio controller;

        public void Start() => controller = this;

        

        // Z Position set to -10 to be on the same axis as camera (volume) 
        public void Play(AudioClip clip, Vector3 pos) => AudioSource.PlayClipAtPoint(clip, new Vector3(pos.x, pos.y, -5), volume);

        public void Mute() {
            m_OldVolume = volume;
            volume = 0;
        }

        public void UnMute() => volume = m_OldVolume;

        #region
        public void ElevatorBell(Vector3 pos) => Play(sfx.elevatorBell, pos);
        public void PlayerDeath(Vector3 pos) => Play(sfx.playerDeath, pos);
        public void StartGame(Vector3 pos) => Play(sfx.startGame, pos);
        public void Dodge(Vector3 pos) => Play(sfx.dodge, pos);

        public void BulletOnHit(Vector3 pos) => Play(RandomManager.ItemFromArray(sfx.bulletOnHit), pos);
        public void Click(Vector3 pos) => Play(RandomManager.ItemFromArray(sfx.click), pos);
        public void EnemyDamage(Vector3 pos) => Play(RandomManager.ItemFromArray(sfx.enemyDamage), pos);
        public void PlayerDamage(Vector3 pos) => Play(RandomManager.ItemFromArray(sfx.playerDamage), pos);
        public void Shoot(Vector3 pos) => Play(RandomManager.ItemFromArray(sfx.shoot), pos);
        public void PickupUpgrade(Vector3 pos) => Play(RandomManager.ItemFromArray(sfx.upgradePickup), pos);
        public void PickupWeapon(Vector3 pos) => Play(RandomManager.ItemFromArray(sfx.weaponPickup), pos);
        #endregion
    }
}
