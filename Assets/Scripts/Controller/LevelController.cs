using Generation.Map;
using Managers;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Controller {
    public class LevelController : MonoBehaviour {

        [SerializeField] private GameObject m_Player;

        [Header("LevelController")]
        public SeedController seeder;
        [SerializeField] private LevelControl m_LevelController;

        [Header("Level settings")]
        [SerializeField] private int enemiesPerLevel = 2;

        private Transform m_MainParent;

        public int currentLevel;

        public static LevelController controller;

        public void Start() {
            controller = this;
            NewLevel(); // temp
        }

        public void TriggerRegenLevel() => NewLevel();

        public void NewLevel() {
            currentLevel++;

            // Sorting out seed
            if (seeder.seed.Equals(0) || seeder.useRandom) {
                RandomManager.GenerateSeed();
                seeder.seed = RandomManager.Seed;
            }
            else { RandomManager.SetSeed(seeder.seed); }

            // Destroying all sprite in map
            if (m_MainParent != null) { Destroy(m_MainParent.gameObject); }
            m_MainParent = new GameObject("Main Parent").transform;

            // Setting up level
            Level.Generate();
            SetupFloor();
            SetupOfficeDeco();
            SetupWalls();
            SetupFinishLine();

            // FINAL
            SpawnPlayer();
        }

        private void SpawnControl (ref GameObject obj, Vector3 position, Quaternion rotation, Vector2 scale, Transform parent) {
            obj.transform.position = position;
            obj.transform.rotation = rotation;
            obj.transform.localScale = scale;
            obj.transform.SetParent(parent);
        }
        private Quaternion GetRandomRotation() {
            float randomRotation2 = RandomManager.ItemFromList(new List<int>() { 0, 90, 180, 270 });
            return new Quaternion(0, 0, randomRotation2, 0);
        }

        private void SetupOfficeDeco() {
            Transform officeDecoParent = new GameObject("Office Deco").transform;
            officeDecoParent.SetParent(m_MainParent);
            foreach (var room in Level.rooms) {
                //if (room.isBossRoom) { SetupBossRoom(room, ref officeDecoParent); continue; }

                GameObject desk;

                int prefabCount = room.prefabCenterPosArr.Length;
                for (int spawnIndex = 0; spawnIndex < prefabCount; spawnIndex++) {
                    if (room.isBossRoom && (spawnIndex.Equals(1) || spawnIndex.Equals(3))) { continue; }

                    Vector2 spawnPos = room.prefabCenterPosArr[spawnIndex];
                    desk = PrefabController.controller.GetRandomDesk();
                    SpawnControl(ref desk, spawnPos, GetRandomRotation(), new Vector2(4, 4), officeDecoParent);
                }

                if (room.isBossRoom) { SetupBossDesk(room, ref officeDecoParent); continue;  }

                desk = PrefabController.controller.GetRandomDesk();
                SpawnControl(ref desk, room.centerPos, GetRandomRotation(), new Vector2(4, 4), officeDecoParent);
            }
        }
        private void SetupBossDesk(Grid room, ref Transform officeDecoParent) {
            GameObject bossDesk = PrefabController.controller.GetRandomBossDesk();
            Vector2 spawnPos = room.centerPos + new Vector2(Level.gridSize.x / 4, 0);
            SpawnControl(ref bossDesk, spawnPos, GetRandomRotation(), new Vector2(4, 4), officeDecoParent);
        }
        private void SetupFloor() {
            Transform floorParent = new GameObject("Floor Parent").transform;
            floorParent.SetParent(m_MainParent);
            foreach (var room in Level.rooms) {
                GameObject floor = PrefabController.controller.GetRandomFloor();
                floor.GetComponent<SpriteRenderer>().size = Level.gridSize;
                // Setting position, scale and parent
                SpawnControl(ref floor, new Vector3(room.centerPos.x, room.centerPos.y, 1), Quaternion.identity, Vector2.one, floorParent);
            }
        }
        private void SetupWalls() {
            Transform wallsParent = new GameObject("Wall Parent").transform;
            wallsParent.SetParent(m_MainParent);

            // Elevator wall
            GameObject startingWall = PrefabController.controller.GetStartingWall();
            SpawnControl(ref startingWall, Vector2.zero, Quaternion.identity, Level.gridSize, wallsParent);

            Vector2 pos = Vector2.zero - new Vector2((Level.gridSize.x / 2) + (startingWall.GetComponent<SpriteRenderer>().bounds.size.x /2), 0);
            startingWall.transform.position = pos;
            // Walls for each room
            foreach (var room in Level.rooms) {
                GameObject wall = PrefabController.controller.GetRandomWall();
                SpawnControl(ref wall, room.centerPos, Quaternion.identity, Level.gridSize, wallsParent);
            }
        }
        private void SetupFinishLine() {
            GameObject finishLine = PrefabController.controller.GetFinishLine();
            Vector2 centerPosLastRoom = Level.rooms[Level.maxRooms - 1].centerPos;
            Vector2 pos = new Vector2(centerPosLastRoom.x + (Level.gridSize.x / 2) + (finishLine.transform.localScale.x / 2), 0);
            Vector2 scale = new Vector2(finishLine.transform.localScale.x, Level.gridSize.y);
            SpawnControl(ref finishLine, pos, Quaternion.identity, scale, m_MainParent);
        }
        private void SpawnPlayer() {
            Vector2 pos = new Vector2(0 - (Level.gridSize.x / 2) - 1, 0);
            SpawnControl(ref m_Player, pos, Quaternion.identity, Vector2.one, null);
        }

        // Drawing for debugging purposes
        private void OnDrawGizmos() {
            // preventing errors while not playing
            if (Level.rooms == null) { return; }

            foreach (var room in Level.rooms) {
                if (room == null) { continue; }

                for (int row = 0; row < Level.gridSize.x; row++) {
                    for (int col = 0; col < Level.gridSize.y; col++) {
                        Tile tile = room.Tiles[row, col];

                        if (tile.isEnemySpawner) { Gizmos.color = Color.blue; }
                        else if (tile.isPrefabArea) { Gizmos.color = Color.red; }
                        else { Gizmos.color = Color.green; }

                        room.Tiles.GetLength(0);

                        Gizmos.DrawCube(tile.position, new Vector3(.6f, .6f, .6f)); ;
                    }
                }

                Gizmos.color = room.isBossRoom ? new Color(1, .92f, .016f, .1f) : new Color(0,0,0,.1f);
                Gizmos.DrawCube(new Vector3(room.centerPos.x, room.centerPos.y, -1), 
                    new Vector3(Level.gridSize.x, Level.gridSize.y, .1f)); ;
            }

        }
    }

    [System.Serializable]
    public struct LevelControl {
        [Tooltip("Selecting preview level")]
        [InspectorButton("TriggerPreviousLevel")] public string prevLevel;
        [Tooltip("Regenerating current level")]
        [InspectorButton("TriggerRegenLevel")] public string regenLevel;
        [Tooltip("Selecting the next level")]
        [InspectorButton("TriggerNextLevel")] public string nextLevel;
    }

    [System.Serializable]
    public struct SeedController {
        public int seed;
        public bool useRandom;
    }
}
