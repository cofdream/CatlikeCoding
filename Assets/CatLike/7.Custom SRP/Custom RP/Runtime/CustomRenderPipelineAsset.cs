using UnityEngine;
using UnityEngine.Rendering;

namespace CatLike
{
    [CreateAssetMenu(menuName = "Rendering/Custom Render Pipline")]
    public class CustomRenderPipelineAsset : RenderPipelineAsset
    {

        [SerializeField] bool useDynamicBatching = false, useGPUInstancing = false, useSPBRBatcher = true;

        protected override RenderPipeline CreatePipeline()
        {
            return new CustomRenderPipline(useDynamicBatching, useGPUInstancing, useSPBRBatcher);
        }
    }
}
