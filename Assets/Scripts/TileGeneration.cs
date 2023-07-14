using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TileGeneration : MonoBehaviour
{
    [SerializeField]
    NoiseMapGeneration noiseMapGeneration;
    [SerializeField]
    private MeshRenderer tileRenderer;
    [SerializeField]
    private MeshFilter meshFilter;
    [SerializeField]
    private MeshCollider meshCollider;
    [SerializeField]
    private float mapScale;
    private float[,] noiseMap;
    void Start()
    {
        GenerateTile();
    }
    void GenerateTile()
    {
        // calculate tile depth and width based on the mesh vertices
        Vector3[] meshVertices = meshFilter.mesh.vertices;
        int tileDepth = (int)Mathf.Sqrt(meshVertices.Length);
        int tileWidth = tileDepth;
        // calculate the offsets based on the tile position
        float[,] heightMap = noiseMapGeneration.GenerateNoiseMap(tileDepth, tileWidth, mapScale);
        noiseMap = heightMap;
        // generate a heightMap using noise
        Texture2D tileTexture = BuildTexture(heightMap);
        tileRenderer.material.mainTexture = tileTexture;
    }
    private Texture2D BuildTexture(float[,] heightMap)
    {
        int tileDepth = noiseMap.GetLength(0);
        int tileWidth = noiseMap.GetLength(1);
        Color[] colorMap = new Color[tileDepth * tileWidth];
        for (int zIndex = 0; zIndex < tileDepth; zIndex++)
        {
            for (int xIndex = 0; xIndex < tileWidth; xIndex++)
            {
                // transform the 2D map index is an Array index
                int colorIndex = zIndex * tileWidth + xIndex;
                float height = heightMap[zIndex, xIndex];
                // assign as color a shade of grey proportional to the height value
                colorMap[colorIndex] = Color.Lerp(Color.black, Color.white, height);
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
