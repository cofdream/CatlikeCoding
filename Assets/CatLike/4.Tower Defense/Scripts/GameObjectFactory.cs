using UnityEngine;
using UnityEngine.SceneManagement;

public class GameObjectFactory : ScriptableObject
{
    Scene contentScene;

    protected T CreateGameObjectInstance<T>(T prefab) where T : MonoBehaviour
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
        T instance = GameObject.Instantiate<T>(prefab);
        SceneManager.MoveGameObjectToScene(instance.gameObject, contentScene);
        return instance;
    }
}