using Assets.Scripts.Controller;
using Enemies;
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

                int enemyCount = LevelController.controller.setEnemiesCount;
                if (room.isBossRoom && LevelController.controller.setEnemiesCount > LevelController.controller.maxEnemiesFinalRoom) {
                    enemyCount = LevelController.controller.maxEnemiesFinalRoom;
                }

                // Plussing boss as enemy for wall detection
                room.enemyCount = enemyCount;

                int[] randomTile;
                for (int count=0; count<enemyCount; count++) {
                    GameObject enemy = PrefabController.controller.GetRandomEnemy();
                    randomTile = RandomManager.ItemFromList(room.enemySpawners);
                    enemy.transform.SetParent(m_EnemyParent);
                    enemy.transform.position = room.Tiles[randomTile[0], randomTile[1]].position;
                }

                if (!room.isBossRoom) { continue; }
                GameObject boss = PrefabController.controller.GetRandomBoss();
                boss.transform.SetParent(m_EnemyParent);

                Vector2 spawnPos = room.centerPos + new Vector2(LevelController.controller.gridSize.x / 3, 0);
                boss.transform.position = spawnPos;

                room.boss = boss.GetComponent<BaseEnemy>();
            }
        }
    }
}
