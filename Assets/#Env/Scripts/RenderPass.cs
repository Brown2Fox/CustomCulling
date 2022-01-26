using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class RenderPass : ScriptableRenderPass
{
    private RenderTargetHandle destination;

    private List<ShaderTagId> shaderTagIdList = new List<ShaderTagId>() { new ShaderTagId("UniversalForward") };
    private FilteringSettings filteringSettings;
    private RenderStateBlock renderStateBlock;

    private Material overrideMaterial;
    private RenderTargetIdentifier depth;
    private bool ignoreDepth;

    public RenderPass(RenderTargetHandle destination, int layerMask, Material overrideMaterial, bool ignoreDepth)
    {
        this.destination = destination;
        this.overrideMaterial = overrideMaterial;
        this.ignoreDepth = ignoreDepth;

        filteringSettings = new FilteringSettings(RenderQueueRange.opaque, layerMask);
        renderStateBlock = new RenderStateBlock(RenderStateMask.Nothing);

    }

    public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
    {
        cmd.GetTemporaryRT(destination.id, cameraTextureDescriptor);
		if(!ignoreDepth)
		{
            ConfigureTarget(destination.Identifier(), depth);
            ConfigureClear(ClearFlag.Color, Color.clear);
        }
        else
		{
            ConfigureTarget(destination.Identifier());
            ConfigureClear(ClearFlag.All, Color.clear);
        }
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        SortingCriteria sortingCriteria = renderingData.cameraData.defaultOpaqueSortFlags;
        DrawingSettings drawingSettings = CreateDrawingSettings(shaderTagIdList, ref renderingData, sortingCriteria);
        drawingSettings.overrideMaterial = overrideMaterial;

        context.DrawRenderers(renderingData.cullResults, ref drawingSettings, ref filteringSettings, ref renderStateBlock);
    }

    public void SetDepthTexture(RenderTargetIdentifier depth)
    { 
        this.depth = depth; 
    }
}
