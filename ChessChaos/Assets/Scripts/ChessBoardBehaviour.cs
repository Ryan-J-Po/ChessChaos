using UnityEngine;
using UnityEngine.UIElements;

public class ChessBoardBehaviour : MonoBehaviour
{

    [Header("Art")]
    [SerializeField]
    private Material _tileMaterial;
    
    //LOGIC

    private const int TILE_COUNT_X = 8;
    private const int TILE_COUNT_Y = 8;

    private GameObject[,] tiles;

    private Camera _camera;

    private Vector2Int _currentHover;

    void Awake()
    {
        GenerateAllTiles(1, TILE_COUNT_X, TILE_COUNT_Y);
    }

    private void Update()
    {
        if (!_camera)
        {
            _camera = Camera.main;

            return;
        }

        RaycastHit info;
        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out info, 100, LayerMask.GetMask("Tile")))
        {
            // Get the indexes of the tile I've hit
            Vector2Int hitPosition = LookupTileIndex(info.transform.gameObject);

            // If we're hovering a tile after not over any tiles / First time hovering during the session...
            if (_currentHover == -Vector2Int.one)
            {
                _currentHover = hitPosition;
                tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
            }

            // If we were already hovering a tile, change the previous one
            if (_currentHover != hitPosition)
            {
                tiles[_currentHover.x, _currentHover.y].layer = LayerMask.NameToLayer("Tile");
                _currentHover = hitPosition;
                tiles[hitPosition.x, hitPosition.y].layer = LayerMask.NameToLayer("Hover");
            }
        }
        else
        {
            if (_currentHover != -Vector2Int.one)
            {
                tiles[_currentHover.x, _currentHover.y].layer = LayerMask.NameToLayer("Tile");
                _currentHover = -Vector2Int.one;
            }
        }

    }
    // Board Generation
    private void GenerateAllTiles(float tileSize, int tileCountX, int tileCountY)
    {
        tiles = new GameObject[tileCountX, tileCountY];
        for (int x = 0; x < tileCountX; x++)
        {
            for (int y = 0; y < tileCountY; y++)
            {
                tiles[x, y] = GenerateSingleTile(tileSize, x, y);
            }
        }
    }

    private GameObject GenerateSingleTile(float tileSize, int x, int y)
    {
        GameObject tileObject = new GameObject(string.Format("x:{0}, Y:{1}", x, y));
        tileObject.transform.parent = transform;

        Mesh mesh = new Mesh();
        tileObject.AddComponent<MeshFilter>().mesh = mesh;
        tileObject.AddComponent<MeshRenderer>().material = _tileMaterial;

        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(x * tileSize, 0, y * tileSize);
        vertices[1] = new Vector3(x * tileSize, 0, (y+1) * tileSize);
        vertices[2] = new Vector3((x+1) * tileSize, 0, y * tileSize);
        vertices[3] = new Vector3((x + 1) * tileSize, 0, (y + 1) * tileSize);

        int[] triangles = new int[] { 0, 1, 2, 1, 3, 2 };

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        mesh.RecalculateNormals();

        tileObject.layer = LayerMask.NameToLayer("Tile");
        tileObject.AddComponent<BoxCollider>();

        return tileObject;
    }

    // Operations
    private Vector2Int LookupTileIndex(GameObject hitInfo)
    {
        for (int x = 0; x < TILE_COUNT_X; x++)
        {
            for (int y = 0; y < TILE_COUNT_Y; y++)
            {
                if (tiles[x, y] == hitInfo)
                {
                    return new Vector2Int(x, y);
                }
            }
        }

        // Will break game due to returning out of bounds indices. Shouldn't happen.
        return -Vector2Int.one;
    }
}
