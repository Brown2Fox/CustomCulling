using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class OutlinePass : ScriptableRenderPass
{
    private string profilerTag = "Outline";
    private Material material;

    public OutlinePass(Material material, string profilerTag)
    {
        this.material = material;
        this.profilerTag = profilerTag;
    }

    public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
    {
        var cmd = CommandBufferPool.Get(profilerTag);

        using (new ProfilingScope(cmd, new ProfilingSampler(profilerTag)))
		{
			var mesh = RenderingUtils.fullscreenMesh;
			cmd.DrawMesh(mesh, Matrix4x4.identity, material, 0, 0);
		}
        
        context.ExecuteCommandBuffer(cmd);
        CommandBufferPool.Release(cmd);
    }
}
