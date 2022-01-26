using System;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Rendering.Universal;

public class OutlineFeature : ScriptableRendererFeature
{
    [Serializable]
	public class RenderSettings
    {
        public Material OverrideMaterial = null;
		public LayerMask LayerMask = 0;
		public bool ignoreDepth = true;
	}

    [Serializable]
	public class BlurSettings
    {
        public Material BlurMaterial;
        public int DownSample = 1;
        public int PassesCount = 1;
    }

	[SerializeField] private string renderTextureName;
	[SerializeField] private string bluredTextureName;
    [SerializeField] private RenderPassEvent renderPassEvent;

    private RenderTargetHandle renderTexture;
    private RenderTargetHandle bluredTexture;

    private RenderPass renderPass;
	private BlurPass blurPass;
    private OutlinePass outlinePass;

    [SerializeField] private RenderSettings renderSettings;
	[SerializeField] private BlurSettings blurSettings;
	[SerializeField] private Material outlineMaterial;
	[SerializeField] private string OutlinedObjectLayerName;

	public override void Create()
    {
		renderTexture.Init(renderTextureName);
		bluredTexture.Init(bluredTextureName);

		renderPass = new RenderPass(renderTexture, renderSettings.LayerMask, renderSettings.OverrideMaterial, renderSettings.ignoreDepth);
		blurPass = new BlurPass(renderTexture.Identifier(), bluredTexture, blurSettings.BlurMaterial, blurSettings.DownSample, blurSettings.PassesCount);
		outlinePass = new OutlinePass(outlineMaterial, OutlinedObjectLayerName);

		renderPass.renderPassEvent = renderPassEvent;
		blurPass.renderPassEvent = renderPassEvent;
		outlinePass.renderPassEvent = renderPassEvent;
	}

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
		if (!renderSettings.ignoreDepth)
		{
			var depthTexture = renderer.cameraDepthTarget;
			renderPass.SetDepthTexture(depthTexture);
		}

		renderer.EnqueuePass(renderPass);
		renderer.EnqueuePass(blurPass);
		renderer.EnqueuePass(outlinePass);
	}
}
