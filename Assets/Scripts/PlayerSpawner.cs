using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    private int mapWidth;
    private int mapDepth;
    private float bottomMapY;
    private float topMapY;
    private System.Func<Vector3, float> getTerrainHeightAtPosition;
    public Vector3 PlayerSpawnPoint;
    private CameraController _cameraController;

    private void Start()
    {
        Vector3 spawnPosition = GetRandomPositionWithinTileSystem();
        GameObject playerInstance = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        SetCameraToPlayer(playerInstance);
    }
    private void SetCameraToPlayer(GameObject playerInstance)
    {
        _cameraController.SetTarget(playerInstance.transform);
    }

    public void InitializePlayerSpawner(GameObject prefab, int workingMapWidth, int workingMapDepth, float workingBottomMapY, float workingTopMapY, System.Func<Vector3, float> terrainHeightFunc, CameraController cameraController)
    {
        playerPrefab = prefab;
        mapWidth = workingMapWidth;
        mapDepth = workingMapDepth;
        bottomMapY = workingBottomMapY;
        topMapY = workingTopMapY;
        getTerrainHeightAtPosition = terrainHeightFunc;
        _cameraController = cameraController;
    }

    private Vector3 GetRandomPositionWithinTileSystem()
    {
        float randomX = Random.Range(0f, mapWidth * topMapY);
        float randomZ = Random.Range(0f, mapDepth * topMapY);
        Vector3 incompletCoordinate = new(randomX, 0f, randomZ);
        float terrainY = getTerrainHeightAtPosition(incompletCoordinate);
        var workingPlayerSpawnPoint = new Vector3(randomX, terrainY, randomZ);
        PlayerSpawnPoint = workingPlayerSpawnPoint;
        return workingPlayerSpawnPoint;
    }
}
