using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    public GameObject playerPrefab;
    private int mapWidth;
    private int mapDepth;
    private float bottomMapY;
    private float topMapY;
    private System.Func<Vector3, float> getTerrainHeightAtPosition;

    private void Start()
    {
        Vector3 spawnPosition = GetRandomPositionWithinTileSystem();
        Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
    }

    public void InitializePlayerSpawner(GameObject prefab, int workingMapWidth, int workingMapDepth, float workingBottomMapY, float workingTopMapY, System.Func<Vector3, float> terrainHeightFunc)
    {
        playerPrefab = prefab;
        mapWidth = workingMapWidth;
        mapDepth = workingMapDepth;
        bottomMapY = workingBottomMapY;
        topMapY = workingTopMapY;
        getTerrainHeightAtPosition = terrainHeightFunc;
    }

    private Vector3 GetRandomPositionWithinTileSystem()
    {
        float randomX = Random.Range(0f, mapWidth * topMapY);
        float randomZ = Random.Range(0f, mapDepth * topMapY);
        Vector3 incompletCoordinate = new(randomX, 0f, randomZ);
        float terrainY = getTerrainHeightAtPosition(incompletCoordinate);
        return new Vector3(randomX, terrainY, randomZ);
    }
}
