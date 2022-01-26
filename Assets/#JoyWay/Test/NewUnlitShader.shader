// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "Instanced/Unlit"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" 
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
		LOD 100
        

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing            
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            UNITY_INSTANCING_BUFFER_START(Props)
            UNITY_DEFINE_INSTANCED_PROP(fixed4, _Color) // Make _Color an instanced property (i.e. an array) #define _Color_arr Props
            UNITY_INSTANCING_BUFFER_END(Props)
            
            v2f vert (appdata v)
            {
                v2f o; 
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o); // necessary only if you want to access instanced properties in the fragment Shader.
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i); // necessary only if any instanced properties are going to be accessed in the fragment Shader.
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv) * UNITY_ACCESS_INSTANCED_PROP(Props, _Color);
                return col;
            }
            ENDCG
        }
    }
}