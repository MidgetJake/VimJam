using Managers;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Controller {
    public class LevelController : MonoBehaviour {

        [SerializeField] private Sprite m_FloorBackground;

        [Header("LevelController")]
        public SeedController seeder;
        [SerializeField] private LevelControl m_LevelController;

        private Transform m_MainParent;

        public int currentLevel;

        public void Start() => NewLevel(); // temp

        public void TriggerRegenLevel() => NewLevel();

        public void NewLevel() {
            // Sorting out seed
            if (seeder.seed.Equals(0) || seeder.useRandom) {
                RandomManager.GenerateSeed();
                seeder.seed = RandomManager.Seed;
            }
            else { RandomManager.SetSeed(seeder.seed); }

            // Destroying all sprite in map
            if (m_MainParent != null) { Destroy(m_MainParent.gameObject); }
            m_MainParent = new GameObject("Main Parent").transform;

            Generation.Generate();
            SetupTextures();
            SetupOfficeDeco();
            SetupWalls();
        }

        private void SetupTextures() {
            // Setting up floor background
            SetupFloor();
        }

        private void SetupOfficeDeco() {
            Transform officeDecoParent = new GameObject("Office Deco").transform;
            officeDecoParent.SetParent(m_MainParent);
            foreach (var room in Generation.rooms) {
                //if (room.isBossRoom) { SetupBossRoom(room, ref officeDecoParent); continue; }

                int prefabCount = room.prefabCenterPosArr.Length;
                for (int spawnIndex = 0; spawnIndex < prefabCount; spawnIndex++) {
                    if (room.isBossRoom && (spawnIndex.Equals(1) || spawnIndex.Equals(3))) { continue; }

                    Vector2 spawnPos = room.prefabCenterPosArr[spawnIndex];

                    GameObject desk = PrefabController.controller.GetRandomDesk();
                    float randomRotation = RandomManager.ItemFromList(new List<int>() { 0, 90, 180, 270 });
                    desk = Instantiate(desk, spawnPos, new Quaternion(0, 0, randomRotation, 0), officeDecoParent);
                    desk.transform.localScale = new Vector3(4, 4, 1);
                }

                if (room.isBossRoom) { SpawnBossDesk(room, ref officeDecoParent); continue;  }

                GameObject desk2 = PrefabController.controller.GetRandomDesk();
                float randomRotation2 = RandomManager.ItemFromList(new List<int>() { 0, 90, 180, 270 });
                desk2 = Instantiate(desk2, room.centerPos, new Quaternion(0, 0, randomRotation2, 0), officeDecoParent);
                desk2.transform.localScale = new Vector3(4, 4, 1);
            }
        }

        private void SpawnBossDesk(Grid room, ref Transform officeDecoParent) {
            GameObject bossDesk = PrefabController.controller.GetRandomBossDesk();
            float randomRotation2 = RandomManager.ItemFromList(new List<int>() { 0, 90, 180, 270 });
            bossDesk = Instantiate(bossDesk, room.centerPos + new Vector2(Generation.gridSize.x/4, 0), new Quaternion(0, 0, randomRotation2, 0), officeDecoParent);
            bossDesk.transform.localScale = new Vector3(4, 4, 1);
        }

        private void SetupFloor() {
            Transform floorParent = new GameObject("Floor Parent").transform;
            floorParent.SetParent(m_MainParent);
            foreach (var room in Generation.rooms) {
                GameObject newFloor = new GameObject();
                SpriteRenderer sprite = newFloor.AddComponent<SpriteRenderer>();
                sprite.sprite = m_FloorBackground;
                sprite.size = Generation.gridSize;
                sprite.drawMode = SpriteDrawMode.Tiled;

                // Setting position, scale and parent
                newFloor.transform.localScale = Vector3.one;
                newFloor.transform.position = new Vector3(room.centerPos.x, room.centerPos.y, 1);
                newFloor.transform.SetParent(floorParent);
            }
        }

        private void SetupWalls() {
            Transform wallsParent = new GameObject("Wall Parent").transform;
            wallsParent.SetParent(m_MainParent);

            // Elevator wall
            GameObject startingWall = PrefabController.controller.GetStartingWall();
            startingWall = Instantiate(startingWall, Vector3.zero, Quaternion.identity, wallsParent);
            startingWall.transform.localScale = Generation.gridSize;

            // Walls for each room
            foreach (var room in Generation.rooms) {
                GameObject wall = PrefabController.controller.GetRandomWall();
                wall = Instantiate(wall, room.centerPos, Quaternion.identity, wallsParent);
                wall.transform.localScale = Generation.gridSize;
            }
        }


        // Drawing for debugging purposes
        private void OnDrawGizmos() {
            // preventing errors while not playing
            if (Generation.rooms == null) { return; }

            foreach (var room in Generation.rooms) {
                if (room == null) { continue; }

                for (int row = 0; row < Generation.gridSize.x; row++) {
                    for (int col = 0; col < Generation.gridSize.y; col++) {
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
                    new Vector3(Generation.gridSize.x, Generation.gridSize.y, .1f)); ;
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
