using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CatLike
{
    public class Graph : MonoBehaviour
    {
        const float pi = Mathf.PI;

        [SerializeField] private Transform pointPrefab = null;
        [SerializeField] [Range(10, 100)] private int resolution = 0;
        [SerializeField] private GraphFunctionName funcion = GraphFunctionName.SineFunction;

        private float step;

        private Transform[] points;

        private GraphFunction[] functions;



        private void Awake()
        {
            functions = new GraphFunction[] {
            SineFunction, Sine2DFunction, MultiSineFunction,MultiSine2DFunction, Ripple, Cylinder, Sphere ,Torus};


            step = 2f / resolution;
            Vector3 scale = Vector3.one * step;

            points = new Transform[resolution * resolution];
            int length = points.Length;

            for (int i = 0; i < length; i++)
            {
                Transform point = Instantiate(pointPrefab);

                point.localScale = scale;

                point.SetParent(transform, false);

                points[i] = point;
            }
        }

        private void Update()
        {
            float t = Time.time;
            GraphFunction f = functions[(int)funcion];
            for (int i = 0, z = 0; z < resolution; z++)
            {
                float v = (z + 0.5f) * step - 1f;

                for (int x = 0; x < resolution; x++, i++)
                {
                    float u = (x + 0.5f) * step - 1f;

                    points[i].localPosition = f(u, v, t);
                }
            }
        }

        private static Vector3 SineFunction(float x, float z, float t)
        {
            Vector3 p;
            p.x = x;
            p.y = Mathf.Sin((x + t) * pi);
            p.z = z;
            return p;
        }

        private static Vector3 Sine2DFunction(float x, float z, float t)
        {
            Vector3 p;
            p.x = x;
            p.y = Mathf.Sin(pi * (x + t));
            p.y += Mathf.Sin(pi * (z + t));
            p.y *= 0.5f;
            p.z = z;

            return p;
        }

        private static Vector3 MultiSineFunction(float x, float z, float t)
        {
            Vector3 p;
            p.x = x;
            p.y = Mathf.Sin((x + t) * pi);
            p.y += Mathf.Sin(2f * (x + t) * pi * 2f) / 2f;
            p.y *= 2f / 3f;
            p.z = z;

            return p;
        }

        private static Vector3 MultiSine2DFunction(float x, float z, float t)
        {
            Vector3 p;
            p.x = x;
            p.y = 4f * Mathf.Sin((x + z + t * 0.5f) * pi);
            p.y += Mathf.Sin((x + t) * pi);
            p.y += Mathf.Sin((z + 2f * t) * pi * 2f) * 0.5f;
            p.y *= 1f / 5.5f;
            p.z = z;

            return p;
        }

        private static Vector3 Ripple(float x, float z, float t)
        {
            Vector3 p;
            p.x = x;
            float d = Mathf.Sqrt(z * z + x * x);
            p.y = Mathf.Sin(pi * (4f * d - t));
            p.y /= 1 + 10 * d;
            p.z = z;
            return p;
        }

        private static Vector3 Cylinder(float u, float v, float t)
        {
            Vector3 p;
            float r = 0.8f + Mathf.Sin(pi * (6f * u + 2f * v + t)) * 0.2f;
            p.x = r * Mathf.Sin(pi * u);
            p.y = v;
            p.z = r * Mathf.Cos(pi * u);
            return p;
        }

        private static Vector3 Sphere(float u, float v, float t)
        {
            Vector3 p;
            float r = 0.8f + Mathf.Sin(pi * (6f * u + t)) * 0.1f;
            r += Mathf.Sin(pi * (4f * v + t)) * 0.1f;
            float s = r * Mathf.Cos(pi * 0.5f * v);
            p.x = s * Mathf.Sin(pi * u);
            p.y = r * Mathf.Sin(pi * 0.5f * v);
            p.z = s * Mathf.Cos(pi * u);
            return p;
        }

        private static Vector3 Torus(float u, float v, float t)
        {
            Vector3 p;
            float r1 = 0.65f + Mathf.Sin(pi * (6f * u + t)) * 0.1f;
            float r2 = 0.2f + Mathf.Sin(pi * (6f * v + t)) * 0.05f;
            float s = r2 * Mathf.Cos(pi * v) + r1;
            p.x = s * Mathf.Sin(pi * u);
            p.y = r2 * Mathf.Sin(pi * v);
            p.z = s * Mathf.Cos(pi * u);
            return p;
        }
    } 
}