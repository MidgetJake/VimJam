using Managers;
using System.Collections.Generic;
using UnityEngine;

public class Grid {
    public Tile[,] Tiles;
    public Vector2 centerPos;
    public Vector2[] prefabCenterPosArr;
    public bool isBossRoom;
}

public class Tile {
    public bool isPrefabArea;
    public bool isEnemySpawner;
    public Vector2 position;
    public Vector2 localPos;
}

public static class Generation
{
    public static int maxRooms = 3;
    public static Vector2 gridSize = new Vector2(16, 16);
    public static Grid[] rooms;
    public static Grid grid;

    private static Vector2[][] m_sections;
    private static float m_padding = 2;

    // Main generation call
    public static void Generate() {
        m_sections = SetupSections(gridSize.x, gridSize.y);

        rooms = new Grid[maxRooms];

        for (int roomIndex=0; roomIndex < maxRooms; roomIndex++) {
            Vector2 centerPos = new Vector2(gridSize.x * roomIndex, 0);

            Grid room = new Grid {
                Tiles = GenerateTiles(centerPos),
                centerPos = centerPos,
                isBossRoom = roomIndex + 1 == maxRooms,
                prefabCenterPosArr = GetPrefabSpawnCenterPos(centerPos),
            };
            rooms[roomIndex] = room;
        }
    }

    private static Vector2[] GetPrefabSpawnCenterPos(Vector2 roomCenterPos) {
        int sectionLength = m_sections.Length;
        Vector2[] centerPos = new Vector2[sectionLength];

        for (int index=0; index<sectionLength; index++) {
            Vector2 test = m_sections[index][0] - (m_sections[index][1] - m_sections[index][0]);
            centerPos[index] = (roomCenterPos + test) - new Vector2(m_sections.Length/2, m_sections.Length/2);
        }

        return centerPos;
    }

    private static Tile[,] GenerateTiles(Vector2 centerPos) {
        Tile[,] tiles = new Tile[(int)gridSize.x, (int)gridSize.y];

        for (int x=0; x<gridSize.x; x++) {
            for (int y=0; y<gridSize.y; y++) {

                Vector2 localPos = new Vector2(x, y);
                (bool isPrefabArea, bool isEnemySpawner) = GiveType(localPos);

                tiles[x, y] = new Tile() {
                    isPrefabArea = isPrefabArea,
                    isEnemySpawner = isEnemySpawner,
                    position =  new Vector2(centerPos.x - (x - (gridSize.x / 2f)) - .5f, centerPos.y - (y - (gridSize.y / 2f)) - .5f),
                    localPos = localPos,
                };
            }
        }

        return tiles;
    }

    private static (bool, bool) GiveType(Vector2 localPos) {
        bool isSpawn = false;
        bool isEnemySpawner = false;

        foreach (var section in m_sections) {
            isSpawn = (localPos.x < section[1].x && localPos.x >= section[0].x) &&
                 (localPos.y < section[1].y && localPos.y >= section[0].y);

            if(isSpawn) { break; }
        }

        // If not spawner check if it's an enemy spawner
        if (!isSpawn) { isEnemySpawner = RandomManager.GetValue(0, 2) != 0; }

        return (isSpawn, isEnemySpawner);
    }

    private static Vector2[][] SetupSections(float x, float y) => 
        new Vector2[4][] {
            new Vector2[2] { new Vector2(m_padding, m_padding), new Vector2((x/2) - m_padding, (y/2) - m_padding ) }, 
            new Vector2[2] { new Vector2((x/2) + m_padding, m_padding), new Vector2(x- m_padding, (y/2 - m_padding)) },
            new Vector2[2] { new Vector2(m_padding, (y/2) + m_padding), new Vector2((x/2) - m_padding, y -m_padding ) }, 
            new Vector2[2] { new Vector2((x/2) + m_padding, (y/2) + m_padding), new Vector2(x- m_padding, y - m_padding ) },
        };
    
}
