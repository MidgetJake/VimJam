using Enemies;
using Events;
using Managers;
using System;
using UnityEngine;
using Weapons;

namespace Assets.Scripts.Enemies {
    [Serializable]
    public enum BossPhases {
        Phase1 = 0,
        Phase2 = 1,
        Phase3 = 2,
    }

    [Serializable] 
    public class Attacks {
        public BossPhases phase;
        public BaseWeaponStats[] attacks;
    }

    public class BossPhaseControl : MonoBehaviour {
        public BossPhases currentPhase = BossPhases.Phase1;

        private EventsHandler m_EventsHandler;
        private BaseEnemy m_BaseEnemy;
        private int m_MaxNumberOfPhases;

        [SerializeField] private Attacks[] m_ListOfAttacks;

        public void Start() {
            m_EventsHandler = GetComponent<EventsHandler>();
            m_BaseEnemy = GetComponent<BaseEnemy>();
            m_MaxNumberOfPhases = Enum.GetNames(typeof(BossPhases)).Length;
        }

        public void CheckPhaseState() {
            if (m_BaseEnemy.health > (m_BaseEnemy.minMaxHealth.y / m_MaxNumberOfPhases) * 2) {
                AttemptPhaseChange(BossPhases.Phase1);
            } 
            else if (m_BaseEnemy.health > (m_BaseEnemy.minMaxHealth.y / m_MaxNumberOfPhases) * 1) {
                AttemptPhaseChange(BossPhases.Phase2);
            } else {
                AttemptPhaseChange(BossPhases.Phase3);
            }

            if (!RandomManager.RollChances(50)) { return; }
            ChangeWeaponStats();
        }

        private void ChangeWeaponStats() {
            BaseWeaponStats stat = RandomManager.ItemFromArray(m_ListOfAttacks[(int)currentPhase].attacks);
            m_EventsHandler.PhaseChange(stat);
        }

        private void AttemptPhaseChange(BossPhases newPhase) {
            if (newPhase == currentPhase) { return; }
            currentPhase = newPhase;
            ChangeWeaponStats();
        }
    }
}
