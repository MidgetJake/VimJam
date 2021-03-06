using Assets.Scripts.Controller;
using Enemies;
using Generation.Map;
using Managers;
using System.Collections.Generic;
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
                room.activeEnemies = new List<GameObject>();

                int[] randomTile; 
                float health;
                for (int count=0; count<enemyCount; count++) {
                    GameObject enemy = PrefabController.controller.GetRandomEnemy();
                    randomTile = RandomManager.ItemFromList(room.enemySpawners);
                    enemy.transform.SetParent(m_EnemyParent);
                    enemy.transform.position = room.Tiles[randomTile[0], randomTile[1]].position;
                    BaseEnemy baseEnemy = enemy.GetComponent<BaseEnemy>();
                    health = SetHealth();
                    baseEnemy.health = health;
                    baseEnemy.minMaxHealth = new Vector2(0, health);
                    room.activeEnemies.Add(enemy);
                }

                if (!room.isBossRoom) { continue; }
                GameObject boss = PrefabController.controller.GetRandomBoss();
                boss.transform.SetParent(m_EnemyParent);
                Vector2 spawnPos = room.centerPos + new Vector2(LevelController.controller.gridSize.x / 3, 0);
                boss.transform.position = spawnPos;
                room.boss = boss.GetComponent<BaseEnemy>();
                health = SetHealth(true);
                room.boss.health = health;
                room.boss.minMaxHealth = new Vector2(0, health);
            }
        }

        private static float SetHealth(bool isBoss = false) {
            LevelController control = LevelController.controller;
            int multiplier = isBoss ? control.bossHealthMultiplier : control.enemyHealthMultiplier;
            int max = isBoss ? control.maxBossHeath : control.maxEnemyHealth;
            int baseHealth = isBoss ? control.baseBossHealth : control.baseEnemyHealth;
            float health = baseHealth + multiplier * (control.currentLevel - 1);
            if (health > max) { health = max; }
            return health;
        }
    }
}
