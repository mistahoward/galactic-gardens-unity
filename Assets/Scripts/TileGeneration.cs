using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TileGeneration : MonoBehaviour
{
    [SerializeField]
    PlanetGeneration PlanetGeneration;
    [SerializeField]
    NoiseMapGeneration noiseMapGeneration;
    [SerializeField]
    private MeshRenderer _tileRenderer;
    [SerializeField]
    private MeshFilter _meshFilter;
    [SerializeField]
    private MeshCollider _meshCollider;
    private float _mapScale;
    private float _heightMultiplier;
    private AnimationCurve _heightCurve;
    private Wave[] _waves;
    private TerrainType[] _terrainTypes;
    void Start()
    {
        _waves = PlanetGeneration.Waves;
        _mapScale = PlanetGeneration.MapScale;
        _heightMultiplier = PlanetGeneration.HeightMultiplier;
        _heightCurve = PlanetGeneration.HeightCurve;
        _terrainTypes = PlanetGeneration.TerrainTypes;
        GenerateTile();
    }
    void GenerateTile()
    {
        // calculate tile depth and width based on the mesh vertices
        Vector3[] meshVertices = _meshFilter.mesh.vertices;
        int tileDepth = (int)Mathf.Sqrt(meshVertices.Length);
        int tileWidth = tileDepth;
        // calculate the offsets based on the tile position
        float offsetX = -gameObject.transform.position.x;
        float offsetZ = -gameObject.transform.position.z;
        // generate a heightMap using noise
        float[,] heightMap = noiseMapGeneration.GenerateNoiseMap(tileDepth, tileWidth, _mapScale, offsetX, offsetZ, _waves);
        // build a Texture2D from the height map
        Texture2D tileTexture = BuildTexture(heightMap);
        _tileRenderer.material.mainTexture = tileTexture;
        // update the tile mesh vertices according to the height map
        UpdateMeshVertices(heightMap);
    }
    private void UpdateMeshVertices(float[,] heightMap)
    {
        int tileDepth = heightMap.GetLength(0);
        int tileWidth = heightMap.GetLength(1);
        Vector3[] meshVertices = _meshFilter.mesh.vertices;
        // iterate through all the heightMap coordinates, updating the vertex index
        int vertexIndex = 0;
        for (int zIndex = 0; zIndex < tileDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < tileWidth; xIndex++)
            {
                float height = heightMap[zIndex, xIndex];
                Vector3 vertex = meshVertices[vertexIndex];
                // change the vertex Y coordinate, proportional to the height value. The height value is evaluated by the heightCurve function, in order to correct it.
                meshVertices[vertexIndex] = new Vector3(vertex.x, _heightCurve.Evaluate(height) * _heightMultiplier, vertex.z);
                vertexIndex++;
            }
        }
        // update the vertices in the mesh and update its properties
        _meshFilter.mesh.vertices = meshVertices;
        _meshFilter.mesh.RecalculateBounds();
        _meshFilter.mesh.RecalculateNormals();
        // update the mesh collider
        _meshCollider.sharedMesh = _meshFilter.mesh;
    }
    private TerrainType ChooseTerrainType(float height)
    {
        // for each terrain type, check if the height is lower than the one for the terrain type
        foreach (TerrainType terrainType in _terrainTypes)
        {
            // return the first terrain type whose height is higher than the generated one
            if (height < terrainType.height)
            {
                return terrainType;
            }
        }
        return _terrainTypes[^1];
    }
    private Texture2D BuildTexture(float[,] heightMap)
    {
        int tileDepth = heightMap.GetLength(0);
        int tileWidth = heightMap.GetLength(1);
        Color[] colorMap = new Color[tileDepth * tileWidth];
        for (int zIndex = 0; zIndex < tileDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < tileWidth; xIndex++)
            {
                // transform the 2D map index is an Array index
                int colorIndex = zIndex * tileWidth + xIndex;
                float height = heightMap[zIndex, xIndex];
                // choose a terrain type according to the height value
                TerrainType terrainType = ChooseTerrainType(height);
                // assign the color according to the terrain type
                colorMap[colorIndex] = terrainType.color;
            }
        }
        // create a new texture and set its pixel colors
        Texture2D tileTexture = new(tileWidth, tileDepth)
        {
            wrapMode = TextureWrapMode.Clamp
        };
        tileTexture.SetPixels(colorMap);
        tileTexture.Apply();
        return tileTexture;
    }
}
