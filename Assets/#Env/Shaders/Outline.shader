Shader "Custom/Outline"
{
    Properties
    {
        _Color ("Glow Color", Color ) = ( 1, 1, 1, 1)
        _Intensity ("Intensity", Float) = 2
        [Enum(UnityEngine.Rendering.BlendMode)] _SrcBlend ("Src Blend", Float) = 1
        [Enum(UnityEngine.Rendering.BlendMode)] _DstBlend ("Dst Blend", Float) = 1
    }
    
    SubShader
    {       
        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        
        struct Attributes
        {
            float4 positionOS   : POSITION;
            float2 uv           : TEXCOORD0;
        };
        
        struct Varyings
        {
            half4 positionCS    : SV_POSITION;
            half2 uv            : TEXCOORD0;
        };
 
        TEXTURE2D_X(_OutlineRenderTexture);
        SAMPLER(sampler_OutlineRenderTexture);
 
        TEXTURE2D_X(_OutlineBluredTexture);
        SAMPLER(sampler_OutlineBluredTexture);
 
        half4 _Color;
        half _Intensity;
 
        Varyings Vertex(Attributes input)
        {
            Varyings output;
            output.positionCS = float4(input.positionOS.xy, 0.0, 1.0); 
            output.uv = input.uv;
            if (_ProjectionParams.x < 0.0) 
                output.uv.y = 1.0 - output.uv.y;    
            return output;      
        }
 
        half4 Fragment(Varyings input) : SV_Target
        {
            float2 uv = UnityStereoTransformScreenSpaceTex(input.uv);
            half4 prepassColor = SAMPLE_TEXTURE2D_X(_OutlineRenderTexture, sampler_OutlineRenderTexture, uv);
            half4 bluredColor = SAMPLE_TEXTURE2D_X(_OutlineBluredTexture, sampler_OutlineBluredTexture,uv);
            half4 difColor = max( 0, bluredColor - prepassColor);
            half4 color = difColor* _Color * _Intensity;
            color.a = 1;    
            return color;
        }
        
        ENDHLSL        
     
        Pass
        {
            Blend [_SrcBlend] [_DstBlend]
            ZTest Always
            ZWrite Off
            Cull Off
 
            HLSLPROGRAM
            #pragma vertex Vertex
            #pragma fragment Fragment        
 
            ENDHLSL         
        }
    }
}