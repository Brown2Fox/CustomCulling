using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BlurPass : ScriptableRenderPass
{
	private int tmpBlurRTId1 = Shader.PropertyToID("_TempBlurTexture1");
	private int tmpBlurRTId2 = Shader.PropertyToID("_TempBlurTexture2");

	private RenderTargetIdentifier tmpBlurRT1;
	private RenderTargetIdentifier tmpBlurRT2;

	private RenderTargetIdentifier source;
	private RenderTargetHandle destination;

	private int passesCount;
	private int downSample;
	private Material blurMaterial;

    public BlurPass(RenderTargetIdentifier source, RenderTargetHandle destination, Material blurMaterial, int downSample, int passesCount)
    {
        this.source = source;
        this.destination = destination;

        this.blurMaterial = blurMaterial;
        this.downSample = downSample;
        this.passesCount = passesCount;
    }

    public override void Configure(CommandBuffer cmd, RenderTextureDescriptor cameraTextureDescriptor)
    {
        var width = Mathf.Max(1, cameraTextureDescriptor.width >> downSample);
        var height = Mathf.Max(1, cameraTextureDescriptor.height >> downSample);
        var blurTextureDesc = new RenderTextureDescriptor(width, height, RenderTextureFormat.ARGB32, 0, 0);

        tmpBlurRT1 = new RenderTargetIdentifier(tmpBlurRTId1);
        tmpBlurRT2 = new RenderTargetIdentifier(tmpBlurRTId2);

        cmd.GetTemporaryRT(tmpBlurRTId1, blurTextureDesc, FilterMode.Bilinear);
        cmd.GetTemporaryRT(tmpBlurRTId2, blurTextureDesc, FilterMode.Bilinear);
        cmd.GetTemporaryRT(destination.id, blurTextureDesc, FilterMode.Bilinear);

        ConfigureTarget(destination.Identifier());
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        var cmd = CommandBufferPool.Get("BlurPass");

        if (passesCount > 0)
        {
            cmd.Blit(source, tmpBlurRT1, blurMaterial, 0);
            for (int i = 0; i < passesCount - 1; i++)
            {
                cmd.Blit(tmpBlurRT1, tmpBlurRT2, blurMaterial, 0);
                var t = tmpBlurRT1;
                tmpBlurRT1 = tmpBlurRT2;
                tmpBlurRT2 = t;
            }
            cmd.Blit(tmpBlurRT1, destination.Identifier());
        }
        else
            cmd.Blit(source, destination.Identifier());

        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }
}
