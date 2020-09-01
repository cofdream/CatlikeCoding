using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CatLike
{
    public class Game : PersistableObject
    {
        public static Game Instance { get; private set; }

        [SerializeField] private ShapeFactory shapeFactory = null;

        public KeyCode createKey = KeyCode.C;
        public KeyCode destoryKey = KeyCode.X;
        public KeyCode newGameKey = KeyCode.N;
        public KeyCode saveKey = KeyCode.S;
        public KeyCode loadKey = KeyCode.L;


        public PersistentStorage storage;

        private List<Shape> shapes;

        private const int saveVersion = 2;

        public float CreateSpeed { get; set; }
        private float CreateProgress;

        public float DestructionSpeed { get; set; }
        private float destructionProgress;

        public int levelCount;
        private int loadingLevelBuildIndex;

        public SpawnZone SpawnZoneOfLevel { get; set; }

        private void Start()
        {
            Instance = this;
            shapes = new List<Shape>();

            if (Application.isEditor)
            {
                for (int i = 0; i < SceneManager.sceneCount; i++)
                {
                    Scene lodingLevel = SceneManager.GetSceneAt(i);
                    if (lodingLevel.name.Contains("Level"))
                    {
                        SceneManager.SetActiveScene(lodingLevel);
                        loadingLevelBuildIndex = lodingLevel.buildIndex;

                        return;
                    }
                }
            }


            StartCoroutine(LoadLevel(1));
        }

        private void OnEnable()
        {
            Instance = this;
        }


        void Update()
        {
            if (Input.GetKeyDown(createKey))
            {
                CreateShape();
            }
            else if (Input.GetKeyDown(destoryKey))
            {
                DestoryShape();
            }
            else if (Input.GetKeyDown(newGameKey))
            {
                BeginNewGame();
            }
            else if (Input.GetKeyDown(saveKey))
            {
                storage.Save(this, saveVersion);
            }
            else if (Input.GetKeyDown(loadKey))
            {
                BeginNewGame();
                storage.Load(this);
            }
            else
            {
                for (int i = 1; i <= levelCount; i++)
                {
                    if (Input.GetKeyDown(KeyCode.Alpha0 + i))
                    {
                        BeginNewGame();
                        StartCoroutine(LoadLevel(i));
                        return;
                    }
                }
            }


            CreateProgress += Time.deltaTime * CreateSpeed;
            while (CreateProgress >= 1f)
            {
                CreateProgress -= 1f;
                CreateShape();
            }

            destructionProgress += Time.deltaTime * DestructionSpeed;
            while (destructionProgress >= 1f)
            {
                destructionProgress -= 1f;
                DestoryShape();
            }
        }

        private void CreateShape()
        {
            Shape instance = shapeFactory.GetRandom();
            Transform transform = instance.transform;
            transform.localPosition = SpawnZoneOfLevel.SpawnPoint;
            transform.localRotation = Random.rotation;
            transform.localScale = Vector3.one * Random.Range(0.1f, 1f);
            instance.SetColor(Random.ColorHSV(hueMin: 0f, hueMax: 1f, saturationMin: 0.5f, saturationMax: 1f, valueMin: 0.25f, valueMax: 1f, alphaMin: 1f, alphaMax: 1f));
            shapes.Add(instance);
        }

        private void DestoryShape()
        {
            if (shapes.Count > 0)
            {
                int index = Random.Range(0, shapes.Count);
                shapeFactory.Reclaim(shapes[index]);
                //删除列表元素，会影响元素的顺序，但是删除变得高效
                int lastIndex = shapes.Count - 1;
                shapes[index] = shapes[lastIndex];
                shapes.RemoveAt(lastIndex);
            }
        }

        private void BeginNewGame()
        {
            foreach (var shape in shapes)
            {
                shapeFactory.Reclaim(shape);
            }
            shapes.Clear();
        }

        public override void Save(GameDataWriter writer)
        {
            writer.Write(shapes.Count);
            writer.Write(loadingLevelBuildIndex);
            for (int i = 0; i < shapes.Count; i++)
            {
                writer.Write(shapes[i].ShapeId);
                writer.Write(shapes[i].materialId);
                shapes[i].Save(writer);
            }
        }
        public override void Load(GameDataReader reader)
        {

            int version = reader.Version;
            if (version > saveVersion)
            {
                return;
            }
            int count = version <= 0 ? -version : reader.ReadInt();
            StartCoroutine(LoadLevel(version < 2 ? 1 : reader.ReadInt()));
            for (int i = 0; i < count; i++)
            {
                int shapeId = version > 0 ? reader.ReadInt() : 0;
                int materialId = version > 0 ? reader.ReadInt() : 0;
                Shape instance = shapeFactory.Get(shapeId, materialId);
                instance.Load(reader);
                shapes.Add(instance);
            }
        }

        private System.Collections.IEnumerator LoadLevel(int levelBuildIndex)
        {
            enabled = false;
            if (loadingLevelBuildIndex > 0)
            {
                yield return SceneManager.UnloadSceneAsync(loadingLevelBuildIndex);
            }

            yield return SceneManager.LoadSceneAsync(levelBuildIndex, LoadSceneMode.Additive);

            SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(levelBuildIndex));
            loadingLevelBuildIndex = levelBuildIndex;
            enabled = true;
        }

    } 
}