using Managers;
using System;
using UnityEngine;

namespace Assets.Scripts.Controller {

    [Serializable]
    public class SFX {
        public AudioClip elevatorBell;
        public AudioClip playerDeath;
        public AudioClip startGame;

        public AudioClip[] bulletOnHit;
        public AudioClip[] click;
        public AudioClip[] enemyDamage;
        public AudioClip[] playerDamage;
        public AudioClip[] shoot;
    }

    public class Audio : MonoBehaviour {
        [Header("Audio Setup")]
        public SFX sfx;

        public static Audio controller;

        public void Start() => controller = this;

        public void Play(AudioClip clip, Vector3 pos) => AudioSource.PlayClipAtPoint(clip, pos);

        #region
        public void ElevatorBell(Vector3 pos) => Play(sfx.elevatorBell, pos);
        public void PlayerDeath(Vector3 pos) => Play(sfx.playerDeath, pos);
        public void StartGame(Vector3 pos) => Play(sfx.startGame, pos);

        public void BulletOnHit(Vector3 pos) => Play(RandomManager.ItemFromArray(sfx.bulletOnHit), pos);
        public void Click(Vector3 pos) => Play(RandomManager.ItemFromArray(sfx.click), pos);
        public void EnemyDamage(Vector3 pos) => Play(RandomManager.ItemFromArray(sfx.enemyDamage), pos);
        public void PlayerDamage(Vector3 pos) => Play(RandomManager.ItemFromArray(sfx.playerDamage), pos);
        public void Shoot(Vector3 pos) => Play(RandomManager.ItemFromArray(sfx.shoot), pos);
        #endregion
    }
}
