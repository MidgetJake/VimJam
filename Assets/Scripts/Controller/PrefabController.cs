using System.Collections.Generic;
using UnityEngine;

namespace Managers {
    public class PrefabController : MonoBehaviour {
        public static PrefabController controller;
        public void Start() => controller = this;

        // List
        [SerializeField] private List<GameObject> m_Desks;
        [SerializeField] private List<GameObject> m_BossDesk;
        [SerializeField] private List<GameObject> m_Walls;
        [SerializeField] private List<GameObject> m_Floor;
        [SerializeField] private List<GameObject> m_Enemies;
        [SerializeField] private List<GameObject> m_Bosses;
        [SerializeField] private GameObject m_StartingWall;
        [SerializeField] private GameObject m_FinishLine;
        [SerializeField] private GameObject m_Player;

        public GameObject GetRandomDesk() => Instantiate(RandomManager.ItemFromList(m_Desks));
        public GameObject GetRandomBossDesk() => Instantiate(RandomManager.ItemFromList(m_BossDesk));
        public GameObject GetRandomWall() => Instantiate(RandomManager.ItemFromList(m_Walls));
        public GameObject GetRandomFloor() => Instantiate(RandomManager.ItemFromList(m_Floor));
        public GameObject GetRandomEnemy() => Instantiate(RandomManager.ItemFromList(m_Enemies));
        public GameObject GetRandomBoss() => Instantiate(RandomManager.ItemFromList(m_Bosses));
        public GameObject GetStartingWall() => Instantiate(m_StartingWall);
        public GameObject GetFinishLine() => Instantiate(m_FinishLine);
        public GameObject GetPlayer() => Instantiate(m_Player);
    }
}