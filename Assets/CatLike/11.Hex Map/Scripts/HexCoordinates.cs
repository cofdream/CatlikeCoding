﻿using UnityEngine;

namespace CatLike.HexMap
{
    [System.Serializable]
    public class HexCoordinates
    {
        [SerializeField] private int x;
        [SerializeField] private int z;
        public int X { get { return x; } }
        public int Y { get { return -X - Z; } }
        public int Z { get { return z; } }
        public HexCoordinates(int x, int z)
        {
            this.x = x;
            this.z = z;
        }

        public override string ToString()
        {
            return $"({x.ToString()},{Y.ToString()},{z.ToString()})";
        }
        public string ToStringOnSeparateLine()
        {
            return $"{x.ToString()}\n{Y.ToString()}\n{z.ToString()}";
        }
        public static HexCoordinates FromOffsetCoordinates(int x, int z)
        {
            return new HexCoordinates(x - z / 2, z);
        }
        public static HexCoordinates FromPostion(Vector3 position)
        {
            float x = position.x / (HexMetrics.innerRadius * 2f);
            float y = -x;
            float offset = position.z / (HexMetrics.outerRadius * 3f);
            x -= offset;
            y -= offset;

            int iX = Mathf.RoundToInt(x);
            int iY = Mathf.RoundToInt(y);
            int iZ = Mathf.RoundToInt(-x - y);

            if (iX + iY + iZ != 0)
            {
                float dX = Mathf.Abs(x - iX);
                float dY = Mathf.Abs(y - iY);
                float dZ = Mathf.Abs(-x - y - iZ);

                if (dX > dY && dX > dZ)
                {
                    iX = -iY - iZ;
                }
                else if (dZ > dY)
                {
                    iZ = -iX - iY;
                }
            }

            return new HexCoordinates(iX, iZ);
        }
    } 
}