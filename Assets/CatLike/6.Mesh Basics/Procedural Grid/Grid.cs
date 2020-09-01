using System.Collections;
using UnityEngine;

namespace CatLike
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class Grid : MonoBehaviour
    {
        public int xSize, ySize;

        private Vector3[] vertuces;

        private Mesh mesh;

        private void Awake()
        {
            Generate();
        }

        private void Generate()
        {
            GetComponent<MeshFilter>().mesh = mesh = new Mesh();
            mesh.name = "Procedural Gird";

            vertuces = new Vector3[(xSize + 1) * (ySize + 1)];
            Vector2[] uv = new Vector2[vertuces.Length];
            Vector4[] tangents = new Vector4[vertuces.Length];
            Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);
            for (int i = 0, y = 0; y <= ySize; y++)
            {
                for (int x = 0; x <= xSize; x++, i++)
                {
                    vertuces[i] = new Vector3(x, y);
                    uv[i] = new Vector2((float)x / xSize, (float)y / ySize);

                    tangents[i] = tangent;
                }
            }

            mesh.vertices = vertuces;
            mesh.uv = uv;
            mesh.tangents = tangents;

            int[] triangles = new int[xSize * ySize * 6];
            for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++)
            {
                for (int x = 0; x < xSize; x++, ti += 6, vi++)
                {
                    triangles[ti] = vi;
                    triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                    triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                    triangles[ti + 5] = vi + xSize + 2;

                    mesh.triangles = triangles;
                    mesh.RecalculateNormals();
                }
            }

            #region 两个三角面
            //int[] tri = new int[6];
            //tri[0] = 0;
            //tri[4] = tri[1] = xSize;
            //tri[3] = tri[2] = (xSize + 1) * ySize;
            //tri[5] = (xSize + 1) * (ySize + 1) - 1;
            //mesh.triangles = tri; 
            //mesh.RecalculateNormals();
            #endregion


        }

        private void OnDrawGizmos()
        {
            if (vertuces == null)
            {
                return;
            }
            Gizmos.color = Color.black;
            for (int i = 0; i < vertuces.Length; i++)
            {
                Gizmos.DrawSphere(vertuces[i], 0.1f);
            }
        }
    }
}