using Unity.Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class RoomContentGenerator : MonoBehaviour
{
    [SerializeField] private RoomGenerator playerRoom, defaultRoom;
    [SerializeField] private GraphTest graphTest;
    [SerializeField] private Transform itemParent;
    [SerializeField] private CinemachineCamera cinemachineCamera;
    [SerializeField] private GameObject teleportPrefab; // NEW: assign in Inspector

    public UnityEvent RegenerateDungeon;

    private List<GameObject> spawnedObjects = new List<GameObject>();
    private Vector2Int playerSpawnPoint; // NEW: store player room center for teleport logic

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            foreach (var item in spawnedObjects)
            {
                Destroy(item);
            }
            RegenerateDungeon?.Invoke();
        }
    }

    public void GenerateRoomContent(DungeonData dungeonData)
    {
        // Clear old objects
        foreach (GameObject item in spawnedObjects)
        {
            DestroyImmediate(item);
        }
        spawnedObjects.Clear();

        // Spawn player first
        SelectPlayerSpawnPoint(dungeonData);

        // Spawn enemies/items in other rooms
        SelectEnemySpawnPoints(dungeonData);

        // Spawn the teleport exit in the farthest room
        PlaceExitTeleport(dungeonData);

        // Parent all spawned objects under itemParent
        foreach (GameObject item in spawnedObjects)
        {
            if (item != null)
                item.transform.SetParent(itemParent, false);
        }
    }

    private void SelectPlayerSpawnPoint(DungeonData dungeonData)
    {
        int randomRoomIndex = UnityEngine.Random.Range(0, dungeonData.roomsDictionary.Count);
        playerSpawnPoint = dungeonData.roomsDictionary.Keys.ElementAt(randomRoomIndex); // store for teleport

        // Run Dijkstra from player spawn
        graphTest.RunDijkstraAlgorithm(playerSpawnPoint, dungeonData.floorPositions);

        Vector2Int roomIndex = playerSpawnPoint;

        List<GameObject> placedPrefabs = playerRoom.ProcessRoom(
            playerSpawnPoint,
            dungeonData.roomsDictionary.Values.ElementAt(randomRoomIndex),
            dungeonData.GetRoomFloorWithoutCorridors(roomIndex)
        );

        FocusCameraOnThePlayer(placedPrefabs[placedPrefabs.Count - 1].transform);

        spawnedObjects.AddRange(placedPrefabs);

        // Remove player room from dictionary so enemies aren't placed there
        dungeonData.roomsDictionary.Remove(playerSpawnPoint);
    }

    private void FocusCameraOnThePlayer(Transform playerTransform)
    {
        cinemachineCamera.LookAt = playerTransform;
        cinemachineCamera.Follow = playerTransform;
    }

    private void SelectEnemySpawnPoints(DungeonData dungeonData)
    {
        foreach (KeyValuePair<Vector2Int, HashSet<Vector2Int>> roomData in dungeonData.roomsDictionary)
        {
            spawnedObjects.AddRange(
                defaultRoom.ProcessRoom(
                    roomData.Key,
                    roomData.Value,
                    dungeonData.GetRoomFloorWithoutCorridors(roomData.Key)
                )
            );
        }
    }

    // NEW: Finds farthest room from player spawn using Dijkstra results and spawns teleport there
    private void PlaceExitTeleport(DungeonData dungeonData)
    {
        if (teleportPrefab == null)
        {
            Debug.LogWarning("Teleport prefab not assigned in RoomContentGenerator!");
            return;
        }

        Dictionary<Vector2Int, int> distances = graphTest.GetDistances();
        int maxDistance = -1;
        Vector2Int bestTile = playerSpawnPoint;

        // Search all remaining rooms (should be FightingPit rooms in your case)
        foreach (var room in dungeonData.roomsDictionary)
        {
            Vector2Int roomCenter = room.Key;
            if (distances.TryGetValue(roomCenter, out int dist) && dist > maxDistance)
            {
                maxDistance = dist;
                bestTile = roomCenter;
            }
        }

        // Center teleport on chosen tile
        Vector3 spawnPos = (Vector3)(bestTile + new Vector2(0.5f, 0.5f));
        GameObject teleport = Instantiate(teleportPrefab, spawnPos, Quaternion.identity);
        spawnedObjects.Add(teleport);
    }
}
