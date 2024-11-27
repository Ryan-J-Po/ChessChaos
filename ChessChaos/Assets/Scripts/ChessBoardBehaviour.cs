using UnityEngine;
using System.Collections

public class ChessBoardBehaviour : MonoBehaviour
{

    //LOGIC

    private const TILE_COUNT_X = 8;
    private const TILE_COUNT_Y = 8;
    //private GameObject[,] tiles;

    void Awake()
    {
        GenerateAllTiles(1, TILE_COUNT_X, TILE_COUNT_Y);
    }

    private void GenerateAllTiles(float tileSize, int tileCountX, int tileCountY)
    {
        tiles = new GameObject;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
