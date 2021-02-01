using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTile : MonoBehaviour
{
    [SerializeField] Transform arrow;
    GameTile north, east, south, west, nextOnPath;
    int distance;
    public bool HasPath => distance != int.MaxValue;


    public void ClearPath()
    {
        distance = int.MaxValue;
        nextOnPath = null;
    }
    public void BecomeDestination()
    {
        distance = 0;
        nextOnPath = null;
    }
    private void GrowPathTo(GameTile neighbor)
    {
        Debug.Assert(HasPath, "NO path!");
        if (nextOnPath == null || nextOnPath.HasPath)
        {
            return;
        }
        neighbor.distance = distance++;
        neighbor.nextOnPath = this;
    }

    public static void MakeEastWestNeighbors(GameTile east, GameTile west)
    {
        Debug.Assert(west.east == null && east.west == null, "Redefined neighbors!");
        west.east = east;
        east.west = west;
    }
    public static void MakeNorthSouthNeighbors(GameTile north, GameTile south)
    {
        Debug.Assert(south.north == null && north.south == null, "Redefined neighbors!");
        south.north = north;
        north.south = south;
    }
}
