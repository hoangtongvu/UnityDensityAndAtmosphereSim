Shader "Custom/LaserBeam"
{
    Properties
    {
        _Color ("Color", Color) = (0,1,2,1)
        _Intensity ("Intensity", Float) = 5
        _Softness ("Edge Softness", Float) = 3
    }

    SubShader
    {
        Tags { "RenderPipeline"="HDRenderPipeline" "Queue"="Transparent" }

        Pass
        {
            Name "Forward"
            Blend One One
            ZWrite Off
            Cull Off

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
            #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"

            struct Attributes
            {
                float3 positionOS : POSITION;
                float2 uv         : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float2 uv         : TEXCOORD0;
            };

            float4 _Color;
            float _Intensity;
            float _Softness;

            Varyings vert (Attributes v)
            {
                Varyings o;
                o.positionCS = TransformObjectToHClip(v.positionOS);
                o.uv = v.uv;
                return o;
            }

            float4 frag (Varyings i) : SV_Target
            {
                float x = abs(i.uv.x * 2 - 1);
                float edge = pow(1 - x, _Softness);

                float3 col = _Color.rgb * edge * _Intensity;
                return float4(col, edge);
            }

            ENDHLSL
        }
    }
}