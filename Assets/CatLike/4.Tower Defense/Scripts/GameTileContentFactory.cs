using UnityEngine;

[CreateAssetMenu]
public class GameTileContentFactory : GameObjectFactory
{
    [SerializeField] GameTileContent distinationPrefab = default;
    [SerializeField] GameTileContent emptyPrefab = default;
    [SerializeField] GameTileContent wallPrefab = default;
    [SerializeField] GameTileContent spawnPointPrefab = default;
    [SerializeField] Tower[] towerPrefabArray = default;


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

        Debug.Assert(false, "Unsupported non-tower type:" + contentType);
        return null;
    }
    private T Get<T>(T prefab) where T : GameTileContent
    {
        T instance = CreateGameObjectInstance(prefab);
        instance.OriginFactory = this;
        return instance;
    }
    public Tower Get(TowerType towerType)
    {
        Debug.Assert((int)towerType < towerPrefabArray.Length, "Unsipported tower type!");
        Tower towerPrefab = towerPrefabArray[(int)towerType];
        Debug.Assert(towerType == towerPrefab.TowerType, "Tower prefab at wrong index!");
        return Get(towerPrefab);
    }

}