using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    [SerializeField] Transform ground;
    [SerializeField] GameTile tilePrefab;
    [SerializeField] Texture2D gridTexture;
    Vector2Int size;
    GameTile[] tileArray;
    GameTileContentFactory gameTileContentFactory;

    Queue<GameTile> searchFrontier = new Queue<GameTile>();

    private bool showGrid, showPaths;
    public bool ShowGrid
    {
        get => showGrid;
        set
        {
            showGrid = value;
            Material material = ground.GetComponent<MeshRenderer>().material;
            if (showGrid)
            {
                material.mainTexture = gridTexture;
                material.SetTextureScale("_MainTex", size);
            }
            else
            {
                material.mainTexture = null;
            }
        }
    }
    public bool ShowPaths
    {
        get => showPaths;
        set
        {
            showPaths = value;
            if (showPaths)
            {
                foreach (var gameTile in tileArray)
                {
                    gameTile.ShowPath();
                }
            }
            else
            {
                foreach (var gameTile in tileArray)
                {
                    gameTile.HidePath();
                }
            }
        }
    }

    public void Initialize(Vector2Int size, GameTileContentFactory gameTileContentFactory)
    {
        this.size = size;
        this.gameTileContentFactory = gameTileContentFactory;

        ground.localScale = new Vector3(size.x, size.y, 1f);

        tileArray = new GameTile[size.x * size.y];

        Vector2 offest = new Vector2((size.x - 1) * 0.5f, (size.y - 1) * 0.5f);
        for (int y = 0, i = 0; y < size.y; y++)
        {
            for (int x = 0; x < size.x; x++, i++)
            {
                GameTile tile = tileArray[i] = Instantiate(tilePrefab);
                tile.transform.SetParent(transform, false);
                tile.transform.localPosition = new Vector3(x - offest.x, 0, y - offest.y);
                if (x > 0)
                {
                    GameTile.MakeEastWestNeighbors(tile, tileArray[i - 1]);
                }
                if (y > 0)
                {
                    GameTile.MakeNorthSouthNeighbors(tile, tileArray[i - size.x]);
                }

                tile.IsAlternative = (x & 1) == 0;
                if ((y & 1) == 0)
                {
                    tile.IsAlternative = !tile.IsAlternative;
                }

                tile.GameTileContent = gameTileContentFactory.Get(GameTileContentType.Empty);
            }
        }
        ToggleDestination(tileArray[tileArray.Length / 2]);
    }

    private bool FindPaths()
    {
        foreach (var gameTile in tileArray)
        {
            if (gameTile.GameTileContent.GameTileContentType == GameTileContentType.Destination)
            {
                gameTile.BecomeDestination();
                searchFrontier.Enqueue(gameTile);
            }
            else
            {
                gameTile.ClearPath();
            }
        }

        if (searchFrontier.Count == 0)
        {
            return false;
        }

        while (searchFrontier.Count > 0)
        {
            GameTile gameTile = searchFrontier.Dequeue();
            if (gameTile != null)
            {
                if (gameTile.IsAlternative)
                {
                    searchFrontier.Enqueue(gameTile.GrowPathNorth());
                    searchFrontier.Enqueue(gameTile.GrowPathSouth());
                    searchFrontier.Enqueue(gameTile.GrowPathEast());
                    searchFrontier.Enqueue(gameTile.GrowPathWest());
                }
                else
                {
                    searchFrontier.Enqueue(gameTile.GrowPathWest());
                    searchFrontier.Enqueue(gameTile.GrowPathEast());
                    searchFrontier.Enqueue(gameTile.GrowPathSouth());
                    searchFrontier.Enqueue(gameTile.GrowPathNorth());
                }

            }
        }
        foreach (var gameTile in tileArray)
        {
            if (!gameTile.HasPath)
            {
                return false;
            }
        }

        if (showPaths)
        {
            foreach (var gameTile in tileArray)
            {
                gameTile.ShowPath();
            }
        }

        return true;
    }

    public GameTile GetTile(Ray ray)
    {
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            // hit.point 的（0，0）在屏幕中间，所以做一个偏移。
            int x = (int)(hit.point.x + size.x * 0.5f);
            int y = (int)(hit.point.z + size.y * 0.5f);
            if (x >= 0 && x < size.x && y >= 0 && y < size.y)
            {
                // y每增加1，数组内元素就增加一次size.x数量
                return tileArray[x + y * size.x];
            }
        }
        return null;
    }

    public void ToggleDestination(GameTile gameTile)
    {
        if (gameTile.GameTileContent.GameTileContentType == GameTileContentType.Destination)
        {
            gameTile.GameTileContent = gameTileContentFactory.Get(GameTileContentType.Empty);
            FindPaths();
        }
        else
        {
            gameTile.GameTileContent = gameTileContentFactory.Get(GameTileContentType.Destination);
            FindPaths();
        }
    }
    public void ToggleWall(GameTile gameTile)
    {
        if (gameTile.GameTileContent.GameTileContentType == GameTileContentType.Wall)
        {
            gameTile.GameTileContent = gameTileContentFactory.Get(GameTileContentType.Empty);
            FindPaths();
        }
        else if (gameTile.GameTileContent.GameTileContentType == GameTileContentType.Empty)
        {
            gameTile.GameTileContent = gameTileContentFactory.Get(GameTileContentType.Wall);
            if (!FindPaths())// 防止路径不可达
            {
                gameTile.GameTileContent = gameTileContentFactory.Get(GameTileContentType.Empty);
                FindPaths();
            }
        }
    }

}
