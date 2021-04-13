using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CatLike.HexMap
{
    public class HexGrid : MonoBehaviour
    {
        public int Width = 6;
        public int Height = 6;
        public Color defaultColor = Color.white;

        public HexCell cellPrefab;
        public Text cellLabelPrefab;


        HexCell[] cells;

        Canvas gridCanvas;
        HexMesh hexMesh;

        private void Awake()
        {
            gridCanvas = GetComponentInChildren<Canvas>();
            hexMesh = GetComponentInChildren<HexMesh>();

            cells = new HexCell[Height * Width];

            for (int z = 0, i = 0; z < Height; z++)
            {
                for (int x = 0; x < Width; x++)
                {
                    CreateCell(x, z, i++);
                }
            }
        }

        private void Start()
        {
            hexMesh.Triangulate(cells);
        }

        public void ColorCell(Vector3 position, Color color)
        {
            position = transform.InverseTransformPoint(position);
            HexCoordinates hexCoordinates = HexCoordinates.FromPostion(position);
            int index = hexCoordinates.X + hexCoordinates.Z * Width + hexCoordinates.Z / 2;
            HexCell cell = cells[index];
            cell.Color = color;
            hexMesh.Triangulate(cells);
        }

        void CreateCell(int x, int z, int i)
        {
            Vector3 position;
            position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
            position.y = 0f;
            position.z = z * (HexMetrics.outerRadius * 1.5f);

            HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab, transform, false);
            cell.transform.localPosition = position;
            cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
            cell.Color = defaultColor;

            if (x > 0)
            {
                cell.SetNeighbor(HexDirection.W, cells[i - 1]);
            }
            if (z > 0)
            {
                if ((z & 1) == 0)
                {
                    cell.SetNeighbor(HexDirection.SE, cells[i - Width]);
                    if (x > 0)
                    {
                        cell.SetNeighbor(HexDirection.SW, cells[i - Width - 1]);
                    }
                }
                else
                {
                    cell.SetNeighbor(HexDirection.SW, cells[i - Width]);
                    if (x < Width - 1)
                    {
                        cell.SetNeighbor(HexDirection.SE, cells[i - Width + 1]);
                    }
                }
            }

            Text label = Instantiate<Text>(cellLabelPrefab);
            label.rectTransform.SetParent(gridCanvas.transform, false);
            label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
            label.text = cell.coordinates.ToStringOnSeparateLine();
        }
    }
}
