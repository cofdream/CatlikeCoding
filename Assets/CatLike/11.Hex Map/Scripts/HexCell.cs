using UnityEngine;

namespace CatLike.HexMap
{
    public class HexCell : MonoBehaviour
    {
        public HexCoordinates coordinates;
        public Color Color;
        public RectTransform uiRect;

        public int Elevation
        {
            get => elevation;
            set
            {
                elevation = value;
                Vector3 position = transform.localPosition;
                position.y = value * HexMetrics.elevationStep;
                transform.localPosition = position;

                Vector3 uiPosition = uiRect.localPosition;
                uiPosition.z = value * -HexMetrics.elevationStep;
                uiRect.localPosition = uiPosition;
            }
        }

        [SerializeField]
        private HexCell[] neighbors = new HexCell[System.Enum.GetValues(typeof(HexDirection)).Length];

        private int elevation;


        public HexCell GetNeighbor(HexDirection direction)
        {
            return neighbors[(int)direction];
        }
        public void SetNeighbor(HexDirection direction, HexCell cell)
        {
            neighbors[(int)direction] = cell;
            cell.neighbors[(int)direction.Opposite()] = this;
        }

        public HexEdgeType GetEdgeType(HexDirection direction)
        {
            return HexMetrics.GetEdgeType(elevation, neighbors[(int)direction].elevation);
        }
    }
}