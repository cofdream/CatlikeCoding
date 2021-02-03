using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class GameTileContentFactory : ScriptableObject
{
    [SerializeField] GameTileContent distinationPrefab = default;
    [SerializeField] GameTileContent emptyPrefab = default;
    [SerializeField] GameTileContent wallPrefab = default;


    Scene contentScene;

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
        }

        Debug.Assert(false, "Unsupported type:" + contentType);
        return null;
    }
    private GameTileContent Get(GameTileContent prefab)
    {
        GameTileContent instance = Instantiate(prefab);
        instance.OriginFactory = this;
        MoveToFactoryScene(instance.gameObject);
        return instance;
    }
    private void MoveToFactoryScene(GameObject gameObject)
    {
        if (!contentScene.isLoaded)
        {
            if (Application.isEditor)
            {
                contentScene = SceneManager.GetSceneByName(name);
                if (!contentScene.isLoaded)
                {
                    contentScene = SceneManager.CreateScene(name);
                }
            }
            else
            {
                contentScene = SceneManager.CreateScene(name);
            }
        }
        SceneManager.MoveGameObjectToScene(gameObject, contentScene);
    }
}