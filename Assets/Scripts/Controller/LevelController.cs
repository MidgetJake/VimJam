using Assets.Scripts.Generation.Enemies;
using Generation.Map;
using Managers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Assets.Scripts.Controller {
    public class LevelController : MonoBehaviour {

        [SerializeField] private GameObject m_Player;
        [SerializeField] private Transform m_MainParent;

        [Header("LevelController")]
        public SeedController seeder;
        [SerializeField] private LevelControl m_LevelController;

        [Header("Settings")]
        public int numberOfRooms = 3;
        public int enemiesPerRoom = 3;
        public int m_padding = 2;
        public Vector2 gridSize = new Vector2(16, 16);

        public int currentLevel;
        public static LevelController controller;

        private Transform m_FloorParent;
        private Transform m_WallsParent;
        private Transform m_DeskParent;

        [Header("Debugging")]
        [SerializeField] private bool m_EnableTileView;

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
            //if (m_MainParent != null) { Destroy(m_MainParent.gameObject); }
            //m_MainParent = new GameObject("Main Parent").transform;

            // Setting up level
            Level.Generate();
            SetupFloor();
            SetupOfficeDeco();
            SetupWalls();
            SetupFinishLine();

            // FINAL
            SetupNavMesh();
            SpawnPlayer();
            EnemySpawner.Spawn();
        }

        private void SetupNavMesh() {
            //m_MainParent.transform.rotation = new Quaternion(-90, 0, 0, 0);
            m_MainParent.gameObject.AddComponent<BoxCollider2D>();
            NavMeshSurface2d surface = m_MainParent.gameObject.GetComponent<NavMeshSurface2d>();

            surface.collectObjects = CollectObjects2d.Children;
            surface.useGeometry = NavMeshCollectGeometry.PhysicsColliders;
            
            Physics2D.SyncTransforms();
            surface.BuildNavMesh();
        }

        //Helpers
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

        // Spawn control
        private void SetupOfficeDeco() {
            m_DeskParent = new GameObject("Office Deco").transform;
            m_DeskParent.SetParent(m_MainParent);
            foreach (var room in Level.rooms) {
                GameObject desk;

                int prefabCount = room.prefabCenterPosArr.Length;
                for (int spawnIndex = 0; spawnIndex < prefabCount; spawnIndex++) {
                    if (room.isBossRoom && (spawnIndex.Equals(1) || spawnIndex.Equals(3))) { continue; }

                    Vector2 spawnPos = room.prefabCenterPosArr[spawnIndex];
                    desk = PrefabController.controller.GetRandomDesk();
                    SpawnControl(ref desk, spawnPos, GetRandomRotation(), new Vector2(4, 4), m_DeskParent);
                }

                if (room.isBossRoom) { SetupBossDesk(room, ref m_DeskParent); continue;  }

                desk = PrefabController.controller.GetRandomDesk();
                SpawnControl(ref desk, room.centerPos, GetRandomRotation(), new Vector2(4, 4), m_DeskParent);
            }
        }

        private void SetupBossDesk(Grid room, ref Transform officeDecoParent) {
            GameObject bossDesk = PrefabController.controller.GetRandomBossDesk();
            Vector2 spawnPos = room.centerPos + new Vector2(gridSize.x / 4, 0);
            SpawnControl(ref bossDesk, spawnPos, GetRandomRotation(), new Vector2(4, 4), officeDecoParent);
        }
        private void SetupFloor() {
            m_FloorParent = new GameObject("Floor Parent").transform;

            m_FloorParent.SetParent(m_MainParent);

            foreach (var room in Level.rooms) {
                GameObject floor = PrefabController.controller.GetRandomFloor();
                floor.GetComponent<SpriteRenderer>().size = gridSize;
                // Setting position, scale and parent
                SpawnControl(ref floor, new Vector2(room.centerPos.x, room.centerPos.y), Quaternion.identity, Vector2.one, m_FloorParent);
            }
        }
        private void SetupWalls() {
            m_WallsParent = new GameObject("Wall Parent").transform;
            m_WallsParent.SetParent(m_MainParent);

            // Elevator wall
            GameObject startingWall = PrefabController.controller.GetStartingWall();
            SpawnControl(ref startingWall, Vector2.zero, Quaternion.identity, gridSize, m_WallsParent);

            Vector2 pos = Vector2.zero - new Vector2((gridSize.x / 2) + (startingWall.GetComponent<SpriteRenderer>().bounds.size.x /2), 0);
            startingWall.transform.position = pos;
            // Walls for each room
            foreach (var room in Level.rooms) {
                GameObject wall = PrefabController.controller.GetRandomWall();
                SpawnControl(ref wall, room.centerPos, Quaternion.identity, gridSize, m_WallsParent);
            }
        }
        private void SetupFinishLine() {
            GameObject finishLine = PrefabController.controller.GetFinishLine();
            Vector2 centerPosLastRoom = Level.rooms[enemiesPerRoom - 1].centerPos;
            Vector2 pos = new Vector2(centerPosLastRoom.x + (gridSize.x / 2) + (finishLine.transform.localScale.x / 2), 0);
            Vector2 scale = new Vector2(finishLine.transform.localScale.x, gridSize.y);
            SpawnControl(ref finishLine, pos, Quaternion.identity, scale, m_MainParent);
        }
        private void SpawnPlayer() {
            Vector2 pos = new Vector2(0 - (gridSize.x / 2) - 1, 0);
            SpawnControl(ref m_Player, pos, Quaternion.identity, Vector2.one, null);
        }

        // Drawing for debugging purposes
        private void OnDrawGizmos() {
            if (!m_EnableTileView) { return; }
            // preventing errors while not playing
            if (Level.rooms == null) { return; }

            foreach (var room in Level.rooms) {
                if (room == null) { continue; }

                for (int row = 0; row < gridSize.x; row++) {
                    for (int col = 0; col < gridSize.y; col++) {
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
                    new Vector3(gridSize.x, gridSize.y, .1f)); ;
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
