using UnityEngine;

namespace CatLike
{
    public class Shape : PersistableObject
    {
        private static int colorPropertyId = Shader.PropertyToID("_Color");
        private static MaterialPropertyBlock sharedPropertyBlock;

        public int ShapeId
        {
            get => shapeId;
            set
            {
                if (shapeId == int.MinValue && value != int.MinValue)
                {
                    shapeId = value;
                }
            }
        }
        private int shapeId = int.MinValue;

        public int materialId { get; private set; }

        private Color color;

        private MeshRenderer meshRenderer;


        private void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
        }

        public void SetMaterial(Material material, int materialId)
        {
            meshRenderer.material = material;
            this.materialId = materialId;
        }

        public void SetColor(Color color)
        {
            this.color = color;
            if (sharedPropertyBlock == null)
            {
                sharedPropertyBlock = new MaterialPropertyBlock();
            }
            sharedPropertyBlock.SetColor(colorPropertyId, color);
            meshRenderer.SetPropertyBlock(sharedPropertyBlock);
        }

        public override void Save(GameDataWriter writer)
        {
            base.Save(writer);
            writer.Write(color);
        }

        public override void Load(GameDataReader reader)
        {
            base.Load(reader);
            SetColor(reader.Version > 0 ? reader.ReadColor() : Color.white);
        }
    } 
}