using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CatLike
{
    [CreateAssetMenu]
    public class ShapeFactory : ScriptableObject
    {
        [SerializeField]
        private Shape[] prefabs = null;

        [SerializeField]
        private Material[] materials = null;

        [SerializeField]
        private bool recycle = false;

        private List<Shape>[] pools = null;

        private Scene poolScene;

        private void CreatPools()
        {
            pools = new List<Shape>[prefabs.Length];
            for (int i = 0; i < pools.Length; i++)
            {
                pools[i] = new List<Shape>();
            }

            if (Application.isEditor)
            {
                poolScene = SceneManager.GetSceneByName(name);
                if (poolScene.isLoaded)
                {
                    GameObject[] rootObjects = poolScene.GetRootGameObjects();
                    foreach (var rootObject in rootObjects)
                    {
                        Shape pooledShape = rootObject.GetComponent<Shape>();
                        if (!pooledShape.gameObject.activeSelf)
                        {
                            pools[pooledShape.ShapeId].Add(pooledShape);
                        }
                    }

                    return;
                }
            }

            poolScene = SceneManager.CreateScene(name);
        }

        public Shape Get(int shapeId, int materialId)
        {

            Shape instance;
            if (recycle)
            {
                if (pools == null)
                {
                    CreatPools();
                }
                List<Shape> pool = pools[shapeId];
                int lastIndex = pool.Count - 1;

                if (lastIndex >= 0)
                {
                    instance = pool[lastIndex];
                    instance.gameObject.SetActive(true);
                    pool.RemoveAt(lastIndex);
                }
                else
                {
                    instance = Instantiate(prefabs[shapeId]);
                    instance.ShapeId = shapeId;
                    SceneManager.MoveGameObjectToScene(instance.gameObject, poolScene);
                }
            }
            else
            {
                instance = Instantiate(prefabs[shapeId]);
                instance.ShapeId = shapeId;
            }
            instance.SetMaterial(materials[materialId], materialId);
            return instance;
        }

        public Shape GetRandom()
        {
            return Get(
                Random.Range(0, prefabs.Length),
                Random.Range(0, materials.Length)
                );
        }


        public void Reclaim(Shape shapeToRecycle)
        {
            if (recycle)
            {
                if (pools == null)
                {
                    CreatPools();
                }

                pools[shapeToRecycle.ShapeId].Add(shapeToRecycle);
                shapeToRecycle.gameObject.SetActive(false);
            }
            else
            {
                Destroy(shapeToRecycle.gameObject);
            }
        }

    }

}