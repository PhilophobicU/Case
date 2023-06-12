Shader "Unlit/Shader1URP"
{
    Properties
    {
        _ColorA ("ColorA", Color) = (1, 1, 1, 1)
        _Frequency("Frequency",Range(0,10)) = 1
        _Amplitude("Amplitude",Range(0,10)) = 1
    }
    SubShader
    {
        Tags
        {
            "RenderType" = "Transparent"
            "RenderPipeline"="UniversalPipeline"
            "Queue"="Transparent"
        }
        Pass
        {
            Cull Off
            ZWrite Off
            ZTest LEqual
            Blend One One

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"


            float4 _ColorA;
            float _Frequency;
            float _Amplitude;

            struct VertexInput
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
                float2 uv2 : TEXCOORD1;
            };

            struct FragmentInput
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : TEXCOORD1;
                
            };

            FragmentInput vert(VertexInput v)
            {
                FragmentInput o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
                o.uv = v.uv;
                return o;
            }
            

            fixed4 frag(FragmentInput i) : SV_Target
            {
                float topBottomRemover = (abs(i.normal.y) < 0.999);
                float4 gradient = lerp(_ColorA, float4(0,0,0,0), i.uv.y);
                float flash = abs(cos(_Time.y * _Frequency) * _Amplitude) + 1;

                
                return gradient * topBottomRemover * flash;
            }
            ENDCG
        }
    }
}