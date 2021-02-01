﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBoard : MonoBehaviour
{
    [SerializeField] Transform ground;
    [SerializeField] GameTile tilePrefab;
    Vector2Int size;
    GameTile[] tileArray;

    Queue<GameTile> searchFrontier = new Queue<GameTile>();
    public void Initialize(Vector2Int size)
    {
        this.size = size;
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
            }
        }
        FindPaths();
    }

    private void FindPaths()
    {
        foreach (var gameTile in tileArray)
        {
            gameTile.ClearPath();
        }

        tileArray[tileArray.Length / 2].BecomeDestination();
        searchFrontier.Enqueue(tileArray[tileArray.Length / 2]);

        while (searchFrontier.Count > 0)
        {
            GameTile gameTile = searchFrontier.Dequeue();
            if (gameTile != null)
            {
                searchFrontier.Enqueue(gameTile.GrowPathNorth());
                searchFrontier.Enqueue(gameTile.GrowPathEast());
                searchFrontier.Enqueue(gameTile.GrowPathSouth());
                searchFrontier.Enqueue(gameTile.GrowPathWest());
            }
        }

        foreach (var gameTile in tileArray)
        {
            gameTile.ShowPath();
        }
    }
}
