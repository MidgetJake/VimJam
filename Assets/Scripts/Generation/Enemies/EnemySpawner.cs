using Assets.Scripts.Controller;
using Generation.Map;
using Managers;
using UnityEngine;

namespace Assets.Scripts.Generation.Enemies {
    public static class EnemySpawner {
        private static Transform m_EnemyParent;

        public static void Spawn(ref Transform mainParent) {
            m_EnemyParent = new GameObject("EnemyParent").transform;
            m_EnemyParent.SetParent(mainParent);

            foreach (var room in Level.rooms) {
                int[] randomTile;
                for (int count=0; count<LevelController.controller.enemiesPerRoom; count++) {
                    GameObject enemy = PrefabController.controller.GetRandomEnemy();
                    randomTile = RandomManager.ItemFromList(room.enemySpawners);
                    enemy.transform.SetParent(m_EnemyParent);
                    enemy.transform.position = room.Tiles[randomTile[0], randomTile[1]].position;
                }

                if (!room.isBossRoom) { continue; }
                GameObject boss = PrefabController.controller.GetRandomBoss();
                randomTile = RandomManager.ItemFromList(room.enemySpawners);
                boss.transform.SetParent(m_EnemyParent);
                boss.transform.position = room.Tiles[randomTile[0], randomTile[1]].position;
            }
        }
    }
}
