using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class TerrainType
{
    public string name;
    public float height;
    public Texture2D texture;
}

public class PlanetGeneration : MonoBehaviour
{
    [SerializeField]
    GameObject playerPrefab;
    NoiseMapGeneration noiseMapGeneration;
    [SerializeField]
    private int _mapWidthInTiles, _mapDepthInTiles;
    private GameObject _tilePrefab;
    [SerializeField]
    private TerrainType[] _terrainTypes;
    [SerializeField]
    private float _mapScale;
    [SerializeField]
    private float _heightMultiplier;
    [SerializeField]
    private AnimationCurve _heightCurve;
    private Wave[] _waves;
    private float bottomY;
    private float topY;

    public float MapScale
    {
        get { return _mapScale; }
    }
    public float HeightMultiplier
    {
        get { return _heightMultiplier; }
    }
    public AnimationCurve HeightCurve
    {
        get { return _heightCurve; }
    }
    public TerrainType[] TerrainTypes
    {
        get { return _terrainTypes; }
    }
    public Wave[] Waves
    {
        get { return _waves; }
    }

    void Start()
    {
        _waves = new Wave[5];

        for (int i = 0; i < Waves.Length; i++)
        {
            Waves[i] = new Wave
            {
                seed = Random.Range(0, 100000),
                frequency = Random.Range(0.1f, 1.0f), // Adjust range as needed
                amplitude = Random.Range(0.1f, 1.0f)  // Adjust range as needed
            };
        }

        GameObject initialTile = GameObject.CreatePrimitive(PrimitiveType.Plane);

        MeshRenderer meshRenderer = initialTile.GetComponent<MeshRenderer>();
        MeshFilter meshFilter = initialTile.GetComponent<MeshFilter>();
        MeshCollider meshCollider = initialTile.GetComponent<MeshCollider>();
        noiseMapGeneration = GetComponent<NoiseMapGeneration>();

        TileGeneration tileGeneration = initialTile.AddComponent<TileGeneration>();
        tileGeneration._tileRenderer = meshRenderer;
        tileGeneration._meshFilter = meshFilter;
        tileGeneration._meshCollider = meshCollider;
        tileGeneration.PlanetGeneration = this;
        tileGeneration.noiseMapGeneration = noiseMapGeneration;

        _tilePrefab = initialTile;

        GenerateMap();

        PlayerSpawner playerSpawner = gameObject.AddComponent<PlayerSpawner>();
        playerSpawner.InitializePlayerSpawner(playerPrefab, _mapWidthInTiles, _mapDepthInTiles, bottomY, topY, GetTerrainHeightAtPosition);
    }
    public void GenerateMap()
    {
        Vector3 tileSize = _tilePrefab.GetComponent<MeshRenderer>().bounds.size;
        int tileWidth = (int)tileSize.x;
        int tileDepth = (int)tileSize.z;

        var tilePositions = Enumerable.Range(0, _mapWidthInTiles)
                            .SelectMany(xTileIndex => Enumerable.Range(0, _mapDepthInTiles)
                            .Select(zTileIndex => new Vector3(gameObject.transform.position.x + xTileIndex * tileWidth,
                                                              gameObject.transform.position.y,
                                                              gameObject.transform.position.z + zTileIndex * tileDepth)));
        foreach (var tilePosition in tilePositions)
        {
            if (bottomY > tilePosition.y)
            {
                bottomY = tilePosition.y;
            }
            if (topY < tilePosition.y)
            {
                topY = tilePosition.y;
            }
            Instantiate(_tilePrefab, tilePosition, Quaternion.identity);
        }
    }
    private float GetTerrainHeightAtPosition(Vector3 position)
    {
        // Cast a ray downwards from the given position to determine the terrain height
        if (Physics.Raycast(position + Vector3.up * 100f, Vector3.down, out RaycastHit hit, Mathf.Infinity))
        {
            return hit.point.y;
        }

        // Default to zero if no terrain is hit
        return 0f;
    }

}