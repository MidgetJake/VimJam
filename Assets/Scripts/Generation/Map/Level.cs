using Assets.Scripts.Controller;
using Enemies;
using Managers;
using System.Collections.Generic;
using UnityEngine;

public class Grid {
    public Tile[,] Tiles;
    public List<int[]> enemySpawners;
    public Vector2 centerPos;
    public Vector2[] prefabCenterPosArr;
    public bool isBossRoom;
    public DoorController doorControl;
    public int enemyCount;
    public BaseEnemy boss;
}

public class Tile {
    public bool isPrefabArea;
    public bool isEnemySpawner;
    public Vector2 position;
    public Vector2 localPos;
}

namespace Generation.Map {
    public static class Level {

        public static Grid[] rooms;
        public static Grid grid;

        private static Vector2[][] m_sections;
        private static Vector2 m_GridSize;

        // Main generation call
        public static void Generate() {
            m_GridSize = LevelController.controller.gridSize;
            m_sections = SetupSections(m_GridSize.x, m_GridSize.y);
            rooms = new Grid[LevelController.controller.numberOfRooms];

            for (int roomIndex=0; roomIndex < LevelController.controller.numberOfRooms; roomIndex++) {

                Vector2 centerPos = new Vector2(m_GridSize.x * roomIndex, 0);

                List<int[]> enemySpawners = new List<int[]>();
                Tile[,] tiles = new Tile[(int)m_GridSize.x, (int)m_GridSize.y];

                GenerateTiles(centerPos, ref tiles, ref enemySpawners, ref roomIndex);

                Grid room = new Grid {
                    Tiles = tiles,
                    enemySpawners = enemySpawners,
                    centerPos = centerPos,
                    isBossRoom = roomIndex + 1 == LevelController.controller.numberOfRooms,
                    prefabCenterPosArr = GetPrefabSpawnCenterPos(centerPos),
                };
                rooms[roomIndex] = room;
            }
        }

        private static Vector2[] GetPrefabSpawnCenterPos(Vector2 roomCenterPos) {
            int sectionLength = m_sections.Length;
            Vector2[] centerPos = new Vector2[sectionLength];

            for (int index = 0; index < sectionLength; index++) {
                Vector2 test = m_sections[index][0] - (m_sections[index][1] - m_sections[index][0]);
                centerPos[index] = (roomCenterPos + test) - new Vector2(m_sections.Length / 2, m_sections.Length / 2);
            }

            return centerPos;
        }

        private static Tile[,] GenerateTiles(Vector2 centerPos, ref Tile[,] tiles, ref List<int[]> enemySpawner, ref int roomIndex) {
            for (int x = 0; x < m_GridSize.x; x++) {
                for (int y = 0; y < m_GridSize.y; y++) {

                    Vector2 localPos = new Vector2(x, y);
                    (bool isPrefabArea, bool isEnemySpawner) = GiveType(localPos, ref roomIndex);

                    tiles[x, y] = new Tile() {
                        isPrefabArea = isPrefabArea,
                        isEnemySpawner = isEnemySpawner,
                        position = new Vector2(centerPos.x - (x - (m_GridSize.x / 2f)) - .5f, centerPos.y - (y - (m_GridSize.y / 2f)) - .5f),
                        localPos = localPos,
                    };

                    if (isEnemySpawner) { enemySpawner.Add(new int[] { x, y }); }
                }
            }

            return tiles;
        }

        private static (bool, bool) GiveType(Vector2 localPos, ref int roomIndex) {
            bool isSpawn = false;
            bool isEnemySpawner = false;

            // Ensuring the first room has some limited spawn space near the elevator
            // Weirdly it seems that the grid goes from right to left instead of left to right
            if (roomIndex.Equals(0) && localPos.x > LevelController.controller.gridSize.y - LevelController.controller.starterSafeZone) { return (false, false); }

            foreach (var section in m_sections) {
                isSpawn = (localPos.x < section[1].x && localPos.x >= section[0].x) &&
                     (localPos.y < section[1].y && localPos.y >= section[0].y);

                if (isSpawn) { break; }
            }

            // If not spawner check if it's an enemy spawner
            if (!isSpawn) { isEnemySpawner = RandomManager.GetValue(0, 2) != 0; }

            return (isSpawn, isEnemySpawner);
        }

        private static Vector2[][] SetupSections(float x, float y) {
            int padding = LevelController.controller.padding;
            return new Vector2[4][] {
                new Vector2[2] { new Vector2(padding, padding), new Vector2((x/2) - padding, (y/2) - padding) },
                new Vector2[2] { new Vector2((x/2) + padding, padding), new Vector2(x- padding, (y/2 - padding)) },
                new Vector2[2] { new Vector2(padding, (y/2) + padding), new Vector2((x/2) - padding, y - padding) },
                new Vector2[2] { new Vector2((x/2) + padding, (y/2) + padding), new Vector2(x- padding, y - padding) },
            };
        }
            
    }
}
