using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CatLike
{
    public class FPSCounter : MonoBehaviour
    {
        public int FrameRange = 60;

        public int AverageFPS { get; private set; }

        public int HighestFPS { get; private set; }
        public int LowestFPS { get; private set; }

        private int[] fpsBuffer;
        private int fpsBufferIndex;

        private void Update()
        {
            if (fpsBuffer == null || fpsBuffer.Length != FrameRange)
            {
                InitializeBuffer();
            }
            UpdateBuffer();
            CalclateFPS();
        }

        private void InitializeBuffer()
        {
            if (FrameRange <= 0)
            {
                FrameRange = 1;
            }
            fpsBuffer = new int[FrameRange];
            fpsBufferIndex = 0;
        }

        private void UpdateBuffer()
        {
            fpsBuffer[fpsBufferIndex++] = (int)(1f / Time.unscaledDeltaTime);
            if (fpsBufferIndex >= FrameRange)
            {
                fpsBufferIndex = 0;
            }
        }

        private void CalclateFPS()
        {
            int sum = 0;
            int hightest = 0;
            int lowerest = int.MaxValue;
            for (int i = 0; i < FrameRange; i++)
            {
                int fps = fpsBuffer[i];
                sum += fps;
                if (fps > hightest)
                {
                    hightest = fps;
                }
                if (fps < lowerest)
                {
                    lowerest = fps;
                }

            }
            AverageFPS = sum / FrameRange;
            HighestFPS = hightest;
            LowestFPS = lowerest;
        }
    } 
}