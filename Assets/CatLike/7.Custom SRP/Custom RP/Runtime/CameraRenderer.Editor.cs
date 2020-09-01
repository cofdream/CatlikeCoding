using UnityEditor;
using UnityEngine;
using UnityEngine.Profiling;
using UnityEngine.Rendering;

namespace CatLike
{
    public partial class CameraRenderer
    {
        partial void PreparBuffer();

        partial void DrawGizmos();

        partial void DrawUnsupporteShaders();

        partial void PrepareForSceneWindow();


#if UNITY_EDITOR

        static ShaderTagId[] legacyShaderTagIds = {
            new ShaderTagId("ForwardBase"),
            new ShaderTagId("PrepassBase"),
            new ShaderTagId("Vertex"),
            new ShaderTagId("VertexLMRGBM"),
            new ShaderTagId("VertexLM"),
        };

        static Material errorMaterial;

        string SampleName { get; set; }

        partial void DrawUnsupporteShaders()
        {
            if (errorMaterial == null)
            {
                errorMaterial = new Material(Shader.Find("Hidden/InternalErrorShader"));
            }

            var drawingSetting = new DrawingSettings(legacyShaderTagIds[0], new SortingSettings(camera))
            {
                overrideMaterial = errorMaterial,
            };
            for (int i = 0; i < legacyShaderTagIds.Length; i++)
            {
                drawingSetting.SetShaderPassName(i, legacyShaderTagIds[i]);
            }
            var filteringSettings = FilteringSettings.defaultValue;

            context.DrawRenderers(cullingResults, ref drawingSetting, ref filteringSettings);
        }

        partial void DrawGizmos()
        {
            if (Handles.ShouldRenderGizmos())
            {
                context.DrawGizmos(camera, GizmoSubset.PreImageEffects);
                context.DrawGizmos(camera, GizmoSubset.PostImageEffects);
            }
        }

        partial void PrepareForSceneWindow()
        {
            if (camera.cameraType == CameraType.SceneView)
            {
                ScriptableRenderContext.EmitWorldGeometryForSceneView(camera);
            }
        }
        partial void PreparBuffer()
        {
            Profiler.BeginSample("Editor Only");
            buffer.name = SampleName = camera.name;
            Profiler.EndSample();
        }
#else
        const string SampleName => bufferName;
#endif
    }
}