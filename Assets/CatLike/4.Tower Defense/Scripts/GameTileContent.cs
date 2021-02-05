using System;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class GameTileContent : MonoBehaviour
{
    [SerializeField] GameTileContentType gameTileContentType = default;

    public GameTileContentType GameTileContentType => gameTileContentType;


    GameTileContentFactory originFactory;

    public GameTileContentFactory OriginFactory
    {
        get => originFactory;
        set
        {
            Debug.Assert(originFactory == null, "Redefined origin factory");
            originFactory = value;
        }
    }

    public bool BlockPath => gameTileContentType == GameTileContentType.Wall || gameTileContentType == GameTileContentType.Tower;

    public void Recycle()
    {
        originFactory.Reclaim(this);
    }

    public virtual void GameUpdate() { }
}