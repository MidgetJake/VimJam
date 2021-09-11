using System.Collections.Generic;
using UnityEngine;

namespace Managers {
    public class PrefabController : MonoBehaviour {
        public static PrefabController controller;
        public void Start() => controller = this;

        // List
        [SerializeField] private List<GameObject> m_Desks;
        [SerializeField] private List<GameObject> m_BossDesk;
        [SerializeField] private GameObject m_StartingWall;
        [SerializeField] private List<GameObject> m_Walls;

        public GameObject GetRandomDesk() => RandomManager.ItemFromList(m_Desks);
        public GameObject GetRandomBossDesk() => RandomManager.ItemFromList(m_BossDesk);
        public GameObject GetRandomWall() => RandomManager.ItemFromList(m_Walls);
        public GameObject GetStartingWall() => m_StartingWall;
    }
}