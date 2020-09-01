using UnityEngine;
using UnityEngine.Rendering;

namespace CatLike
{
    public class CustomRenderPipline : RenderPipeline
    {
        bool useDynamicBatching, useGPUInstancing;
        public CustomRenderPipline(bool useDynamicBatching, bool useGPUInstancing, bool useSPBRBatching)
        {
            this.useDynamicBatching = useDynamicBatching;
            this.useGPUInstancing = useGPUInstancing;
            GraphicsSettings.useScriptableRenderPipelineBatching = useSPBRBatching;
        }

        CameraRenderer renderer = new CameraRenderer();

        protected override void Render(ScriptableRenderContext context, Camera[] cameras)
        {
            foreach (var camera in cameras)
            {
                renderer.Render(context, camera, useDynamicBatching, useGPUInstancing);
            }
        }
    }
}