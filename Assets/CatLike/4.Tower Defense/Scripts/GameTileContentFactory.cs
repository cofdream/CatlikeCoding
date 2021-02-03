using UnityEngine;

[CreateAssetMenu]
public class GameTileContentFactory : GameObjectFactory
{
    [SerializeField] GameTileContent distinationPrefab = default;
    [SerializeField] GameTileContent emptyPrefab = default;
    [SerializeField] GameTileContent wallPrefab = default;
    [SerializeField] GameTileContent spawnPointPrefab = default;


    public void Reclaim(GameTileContent gameTileContent)
    {
        Debug.Assert(gameTileContent.OriginFactory == this, "Wrong factory reclaimed!");
        Destroy(gameTileContent.gameObject);
    }

    public GameTileContent Get(GameTileContentType contentType)
    {
        switch (contentType)
        {
            case GameTileContentType.Empty: return Get(emptyPrefab);
            case GameTileContentType.Destination: return Get(distinationPrefab);
            case GameTileContentType.Wall: return Get(wallPrefab);
            case GameTileContentType.SpawnPoint: return Get(spawnPointPrefab);
        }

        Debug.Assert(false, "Unsupported type:" + contentType);
        return null;
    }
    private GameTileContent Get(GameTileContent prefab)
    {
        GameTileContent instance = CreateGameObjectInstance(prefab);
        instance.OriginFactory = this;
        return instance;
    }
}