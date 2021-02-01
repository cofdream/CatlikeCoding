using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour
{
    public int Width = 6;
    public int Height = 6;
    public Color defaultColor = Color.white;

    public HexCell cellPrefab;

    public Canvas gridCanvas;
    public Text cellLabelPrefab;
    public HexMesh hexMesh;

    HexCell[] cells;



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
        cell.HexCoordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        cell.Color = defaultColor;

        Text label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        label.text = cell.HexCoordinates.ToStringOnSeparateLine();
    }
}
