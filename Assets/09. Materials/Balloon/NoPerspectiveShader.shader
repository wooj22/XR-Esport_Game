Shader "Custom/NoPerspectiveShader"
{
    Properties
    {
        _BaseColor("Base Color", Color) = (1,1,1,1)
        _BaseMap("Base Map", 2D) = "white" {}
    }

        SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            Name "ForwardBase"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float4 color : COLOR;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float4 color : COLOR;
            };

            Varyings vert(Attributes v)
            {
                Varyings o;
                float4 positionWS = mul(unity_ObjectToWorld, v.positionOS);
                float4 positionCS = mul(unity_MatrixVP, positionWS);

                // 원근법을 제거하기 위해 z 값을 고정
                positionCS.z = positionCS.w * 0.5; // Adjust this factor as needed

                o.positionCS = positionCS;
                o.color = v.color;
                return o;
            }

            half4 frag(Varyings i) : SV_Target
            {
                return i.color;
            }
            ENDHLSL
        }
    }

        FallBack "Diffuse"
}

