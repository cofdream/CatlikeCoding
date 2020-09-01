using UnityEngine;
using UnityEngine.Rendering;

namespace CatLike
{
    public partial class CameraRenderer
    {
        const string bufferName = "Renderer Camera";

        static ShaderTagId unlitShaderTagId = new ShaderTagId("SRPDefaultUnlit");//渲染 Unlit Shader

        CommandBuffer buffer = new CommandBuffer() { name = bufferName, };

        ScriptableRenderContext context;
        Camera camera;

        CullingResults cullingResults;

        bool useDynamicBatching, useGPUInstancing;


        public void Render(ScriptableRenderContext context, Camera camera, bool useDynamicBatching, bool useGPUInstancing)
        {
            this.context = context;
            this.camera = camera;

            PreparBuffer();
            PrepareForSceneWindow();
            if (!Cull())
            {
                return;
            }

            Setup();
            DrawVisibleGeometry(useDynamicBatching, useGPUInstancing);
            DrawUnsupporteShaders();
            DrawGizmos();
            Submit();
        }

        private void DrawVisibleGeometry(bool useDynamicBatching, bool useGPUInstancing)
        {
            var sortingSettings = new SortingSettings(camera) { criteria = SortingCriteria.CommonOpaque };
            var drawingSetting = new DrawingSettings(unlitShaderTagId, sortingSettings)
            {
                enableDynamicBatching = useDynamicBatching,
                enableInstancing = useGPUInstancing,
            };
            var filteringSettings = new FilteringSettings(RenderQueueRange.opaque);

            context.DrawRenderers(cullingResults, ref drawingSetting, ref filteringSettings);

            context.DrawSkybox(camera);

            sortingSettings.criteria = SortingCriteria.CommonTransparent;
            drawingSetting.sortingSettings = sortingSettings;
            filteringSettings.renderQueueRange = RenderQueueRange.transparent;

            context.DrawRenderers(cullingResults, ref drawingSetting, ref filteringSettings);

        }

        private void Setup()
        {
            context.SetupCameraProperties(camera);
            CameraClearFlags flags = camera.clearFlags;
            buffer.ClearRenderTarget(
                flags <= CameraClearFlags.Depth,
                flags == CameraClearFlags.Color,
                flags == CameraClearFlags.Color ? camera.backgroundColor : Color.clear
                );
            buffer.BeginSample(SampleName);
            ExecuteBuffer();
        }

        private void Submit()
        {
            buffer.EndSample(SampleName);
            ExecuteBuffer();
            context.Submit();
        }

        private void ExecuteBuffer()
        {
            context.ExecuteCommandBuffer(buffer);
            buffer.Clear();
        }

        private bool Cull()
        {
            if (camera.TryGetCullingParameters(out ScriptableCullingParameters p))
            {
                cullingResults = context.Cull(ref p);
                return true;
            }
            return false;
        }
    }
}