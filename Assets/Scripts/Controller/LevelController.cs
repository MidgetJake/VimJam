using System;
using Assets.Scripts.Generation.Enemies;
using Generation.Map;
using Managers;
using System.Collections.Generic;
using Player;
using UI;
using Controller;
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
        public int enemiesMultiplier = 3;
        public int setEnemiesCount;
        public int maxEnemiesPerRoom = 15;
        public int maxEnemiesFinalRoom = 5;
        public int maxEnemyHealth = 15;
        public int enemyHealthMultiplier = 3;
        public int baseEnemyHealth = 3;
        public int baseBossHealth = 18;
        public int maxBossHeath = 20;
        public int bossHealthMultiplier = 3;
        public int starterSafeZone = 8;
        public int padding = 2;
        public int currentRoom = 0;
        public float secondsCount;
        public Vector2 gridSize = new Vector2(16, 16);

        public int currentLevel;
        public static LevelController controller;

        private Transform m_FloorParent;
        private Transform m_WallsParent;
        private Transform m_DeskParent;
        private Grid m_CurrentRoom;
        private DoorController m_StartElevator;
        private bool m_BossTriggered = false;
        private bool m_IsCounting = false;

        [Header("Debugging")]
        [SerializeField] private bool m_EnableTileView;

        [SerializeField] public TimeCounter tc;
        [SerializeField] public BaseStats bs;
        [SerializeField] private FloorCounter m_FloorCounter;

        public void Start() {
            controller = this;
            NewLevel(); // temp
        }

        public void Update()
        {
            if (m_IsCounting) {
                secondsCount += Time.deltaTime;
                tc.UpdateTimer((int) secondsCount);
            }
        }
        
        public void TriggerRegenLevel() => NewLevel();

        public void ClearLevel() {
            Destroy(m_MainParent.gameObject);
            m_MainParent = new GameObject("Main Parent").transform;
            LootController.main.parent = m_MainParent;
            // Must be 270 for 2d Nav Mesh to work
            m_MainParent.rotation = Quaternion.Euler(270, 0, 0);
            currentRoom = 0;
            m_BossTriggered = false;
            BackgroundAudio.controller.bossMode = false;
        }

        public void NewGame() {
            currentLevel = 0;
            currentRoom = 0;
            PlayerController.player.ResetPlayer();
            secondsCount = 0;
            NewLevel();
        }

        public void NewLevel() {
            ClearLevel();

            currentLevel++;
            m_FloorCounter.SetFloorCounter(currentLevel);

            // Sorting out seed
            if (seeder.seed.Equals(0) || seeder.useRandom) {
                RandomManager.GenerateSeed();
                seeder.seed = RandomManager.Seed;
            } else { RandomManager.SetSeed(seeder.seed); }

            // Sorting out enemy spawn count
            setEnemiesCount = enemiesMultiplier * currentLevel;
            if (setEnemiesCount > maxEnemiesPerRoom) { setEnemiesCount = maxEnemiesPerRoom; }

            // Setting up level
            Level.Generate();
            SetupFloor();
            SetupOfficeDeco();
            SetupWalls();
            SetupFinishLine();

            // FINAL
            SetupNavMesh();
            SpawnPlayer();
            EnemySpawner.Spawn(ref m_MainParent);

            m_CurrentRoom = Level.rooms[currentRoom];

            m_StartElevator.Unlock();
            m_IsCounting = true;
        }

        #region Helpers
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
        #endregion

        #region Generation Setup
        private void SetupNavMesh() {
            //m_MainParent.gameObject.AddComponent<BoxCollider2D>();
            NavMeshSurface2d nav = m_MainParent.gameObject.AddComponent<NavMeshSurface2d>();
            nav.collectObjects = CollectObjects2d.Children;
            nav.useGeometry = NavMeshCollectGeometry.PhysicsColliders;
            Physics2D.SyncTransforms(); // Applying all transformation for dynamic nav mesh build
            nav.BuildNavMesh();
        }
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
            SpawnControl(ref bossDesk, spawnPos, new Quaternion(0, 0, 0, 0), new Vector2(4, 4), officeDecoParent);
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
            m_StartElevator = startingWall.GetComponentInChildren<DoorController>();

            Vector2 pos = Vector2.zero - new Vector2((gridSize.x / 2) + (startingWall.GetComponent<SpriteRenderer>().bounds.size.x /2), 0);
            startingWall.transform.position = pos;

            int roomCount = Level.rooms.Length;
            for (int index=0; index<roomCount; index++) {
                Grid room = Level.rooms[index];

                GameObject wall;
                if (index +1 >= roomCount) { wall = PrefabController.controller.GetFinalWall(); } 
                else { wall = PrefabController.controller.GetRandomWall(); }

                SpawnControl(ref wall, room.centerPos, Quaternion.identity, gridSize, m_WallsParent);
                room.doorControl = wall.gameObject.GetComponentInChildren<DoorController>();
            }
        }
        private void SetupFinishLine() {
            GameObject finishLine = PrefabController.controller.GetFinishLine();
            Vector2 centerPosLastRoom = Level.rooms[numberOfRooms - 1].centerPos;
            Vector2 pos = new Vector2(centerPosLastRoom.x + (gridSize.x / 2) + (finishLine.transform.localScale.x / 2), 0);
            Vector2 scale = new Vector2(finishLine.transform.localScale.x, gridSize.y);
            SpawnControl(ref finishLine, pos, Quaternion.identity, scale, m_MainParent);
        }
        private void SpawnPlayer() {
            Vector2 pos = new Vector2(0 - (gridSize.x / 2) - 1, 0);
            SpawnControl(ref m_Player, pos, Quaternion.identity, m_Player.transform.localScale, null);
        }
        #endregion

        #region Events
        public void RecordDeath(GameObject obj, bool isBoss) {
            if (isBoss) { m_CurrentRoom.boss = null; }
            else { m_CurrentRoom.activeEnemies.Remove(obj); }
            if (m_CurrentRoom.enemyCount > 0) { return; }

            if (m_CurrentRoom.isBossRoom && !m_BossTriggered) { TriggerBoss(); return; }
            
            // Room finished
            if (m_CurrentRoom.doorControl != null) { m_CurrentRoom.doorControl.Unlock(); }

            if (m_CurrentRoom.isBossRoom) {
                m_IsCounting = false;
                return;
            }
            currentRoom++;
            m_CurrentRoom = Level.rooms[currentRoom];
        }

        private void TriggerBoss() {
            // CUTSCENE GOES HERE

            // triggers boss to move and attack
            m_BossTriggered = true;
            BackgroundAudio.controller.bossMode = true;
            m_CurrentRoom.boss.isActive = true;
        }
        #endregion

        // Drawing for debugging purposes
        public void OnDrawGizmos() {
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
