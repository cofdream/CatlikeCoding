﻿using System.Collections;
using UnityEngine;

namespace CatLike
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class Cube : MonoBehaviour
    {
        public int xSize, ySize, zSize;

        private Mesh mesh = null;
        private Vector3[] vertices = null;

        private void Awake()
        {
            Generate();
        }

        private void Generate()
        {
            GetComponent<MeshFilter>().mesh = mesh = new Mesh();
            mesh.name = "Procedural Cube";

            CreateVertices();

            CreateTriangles();
        }
        private void CreateVertices()
        {

            //角数  的顶点数
            int cornerVertices = 8;
            //边  的顶点数
            int edgeVertices = (xSize + ySize + zSize - 3) * 4;
            //无边无角的面  的顶点数
            int faceVertices =
                ((xSize - 1) * (ySize - 1) +
                 (xSize - 1) * (zSize - 1) +
                 (ySize - 1) * (zSize - 1)) * 2;

            vertices = new Vector3[cornerVertices + edgeVertices + faceVertices];

            int v = 0;

            //四周
            for (int y = 0; y <= ySize; y++)
            {
                for (int x = 0; x <= xSize; x++)
                {
                    vertices[v++] = new Vector3(x, y, 0);
                }

                for (int z = 1; z <= zSize; z++)
                {
                    vertices[v++] = new Vector3(xSize, y, z);
                }
                for (int x = xSize - 1; x >= 0; x--)
                {
                    vertices[v++] = new Vector3(x, y, zSize);
                }

                for (int z = zSize - 1; z > 0; z--)
                {
                    vertices[v++] = new Vector3(0, y, z);
                }
            }

            //顶部
            for (int z = 1; z < zSize; z++)
            {
                for (int x = 1; x < xSize; x++)
                {
                    vertices[v++] = new Vector3(x, ySize, z);
                }
            }

            //底部
            for (int z = 1; z < zSize; z++)
            {
                for (int x = 1; x < xSize; x++)
                {
                    vertices[v++] = new Vector3(x, 0, z);
                }
            }

            mesh.vertices = vertices;
        }

        private void CreateTriangles()
        {
            int quads = (xSize + ySize + xSize * zSize + ySize * zSize) * 2;
            int[] triangles = new int[quads * 6];

            int ring = (xSize + zSize) * 2;
            int t = 0, v = 0;

            for (int y = 0; y < ySize; y++, v++)//这个v++是下面单独执行的那一次自增
            {
                for (int q = 0; q < ring - 1; q++, v++)
                {
                    t = SetQuad(triangles, t, v, v + 1, v + ring, v + ring + 1);
                }
                t = SetQuad(triangles, t, v, v - ring + 1, v + ring, v + 1);
            }

            t = CreateTopFace(triangles, t, ring);

            mesh.triangles = triangles;
        }



        private void OnDrawGizmos()
        {
            if (vertices == null) return;

            Gizmos.color = Color.black;
            for (int i = 0; i < vertices.Length; i++)
            {
                Gizmos.DrawSphere(vertices[i], 0.1f);
            }


            return;
            int rang = (xSize + zSize) * 2;

            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(vertices[rang * (ySize + 1) - 1], 0.1f);

            Gizmos.color = Color.red;
            Gizmos.DrawSphere(vertices[rang * ySize], 0.1f);
        }


        private static int SetQuad(int[] triangles, int i, int v00, int v10, int v01, int v11)
        {
            triangles[i] = v00;
            triangles[i + 1] = triangles[i + 4] = v01;
            triangles[i + 2] = triangles[i + 3] = v10;
            triangles[i + 5] = v11;

            return i + 6;
        }

        private int CreateTopFace(int[] triangles, int t, int rang)
        {
            int v = rang * ySize;

            for (int x = 0; x < xSize - 1; x++, v++)
            {
                t = SetQuad(triangles, t, v, v + 1, v + rang - 1, v + rang);
            }
            t = SetQuad(triangles, t, v, v + 1, v + rang - 1, v + 2);



            int vMin = rang * (ySize + 1) - 1;
            int vMid = vMin + 1;
            int vMax = v + 2;

            for (int z = 1; z < zSize - 1; z++, vMin--, vMid++, vMax++)
            {
                t = SetQuad(triangles, t, vMin, vMid, vMin - 1, vMid + xSize - 1);

                for (int x = 1; x < xSize - 1; x++, vMid++)
                {
                    t = SetQuad(triangles, t, vMid, vMid + 1, vMid + xSize - 1, vMid + xSize);
                }
                t = SetQuad(triangles, t, vMid, vMax, vMid + xSize - 1, vMax + 1);
            }


            int vTop = vMin - 2;
            t = SetQuad(triangles, t, vMin, vMid, vMin - 1, vMin - 2);
            for (int x = 0; x < xSize - 1; x++, vTop--, vMid++)
            {
                t = SetQuad(triangles, t, vMid, vMid + 1, vTop, vTop - 1);
            }
            t = SetQuad(triangles, t, vMid, vTop - 2, vTop, vTop - 1);

            return t;
        }
    }

}