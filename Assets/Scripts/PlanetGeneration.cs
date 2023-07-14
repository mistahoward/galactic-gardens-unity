using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class TerrainType
{
    public string name;
    public float height;
    public Color color;
}

public class PlanetGeneration : MonoBehaviour
{
    [SerializeField]
    private int _mapWidthInTiles, _mapDepthInTiles;
    [SerializeField]
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
        GenerateMap();
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
            Instantiate(_tilePrefab, tilePosition, Quaternion.identity);
        }
    }

}