Shader "Revealing Under Light Decal (URP)"
{
    Properties
    {
        _BaseColor("Color", Color) = (1,1,1,1)
        _BaseMap("Albedo (RGB)", 2D) = "white" {}
        _LightPosition("Light Position", Vector) = (0,0,0,0)
        _LightDirection("Light Direction", Vector) = (0,0,1,0)
        _LightAngle("Light Angle", Range(0,180)) = 45
        _Intensity("Light Intensity", Float) = 0
        _StrengthScalor("Strength", Float) = 50
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "Queue"="Transparent"
            "RenderPipeline"="UniversalPipeline"
            "DecalProjector"="True"      // 👈 Necesario para Decal Projectors URP
        }

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Name "DecalRevealPass"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 positionWS : TEXCOORD1;
            };

            sampler2D _BaseMap;

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                float4 _BaseColor;
                float3 _LightPosition;
                float3 _LightDirection;
                float _LightAngle;
                float _Intensity;
                float _StrengthScalor;
            CBUFFER_END

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                OUT.positionHCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _BaseMap);
                OUT.positionWS = TransformObjectToWorld(IN.positionOS.xyz);
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // Calcular dirección entre el pixel y la luz
                float3 toLight = normalize(_LightPosition - IN.positionWS);

                // Proyectar con dot product entre forward de linterna y dirección
                float dotDir = dot(toLight, normalize(_LightDirection));

                // Ángulo mínimo de luz (cos)
                float angleLimit = cos(_LightAngle * 3.14159 / 180.0);

                // Comparar y amplificar
                float strength = saturate((dotDir - angleLimit) * _StrengthScalor);

                // Apagar si la linterna está apagada
                strength *= saturate(_Intensity);

                half4 texColor = tex2D(_BaseMap, IN.uv) * _BaseColor;
                texColor.a *= strength;

                // Si la transparencia es muy baja, no dibujar
                clip(texColor.a - 0.01);

                return texColor;
            }
            ENDHLSL
        }
    }

    FallBack Off
}