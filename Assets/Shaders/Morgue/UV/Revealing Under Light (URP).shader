Shader "Revealing Under Light (URP)"
{
    Properties
    {
        _BaseColor("Color", Color) = (1,1,1,1)
        _BaseMap("Albedo (RGB)", 2D) = "white" {}
        _Smoothness("Smoothness", Range(0,1)) = 0.5
        _Metallic("Metallic", Range(0,1)) = 0.0
        _LightDirection("Light Direction", Vector) = (0,0,1,0)
        _LightPosition("Light Position", Vector) = (0,0,0,0)
        _LightAngle("Light Angle", Range(0,180)) = 45
        _StrengthScalor("Strength", Float) = 50
        _Intensity("Light Intensity", Float) = 0

    }

    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "Queue"="Transparent"
            "RenderPipeline"="UniversalPipeline"
        }

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Name "ForwardLit"
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
                float _Smoothness;
                float _Metallic;
                float3 _LightPosition;
                float3 _LightDirection;
                float _LightAngle;
                float _StrengthScalor;
                float _Intensity;
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
                // -------------------------------
                // 1) DIRECCIÓN DE LUZ
                // -------------------------------
                float3 lightDir = normalize(_LightPosition - IN.positionWS);

                // Dot con el forward de la linterna
                float scale = dot(lightDir, normalize(_LightDirection));

                // -------------------------------
                // 2) Ángulo del haz (spotlight)
                // -------------------------------
                float angleLimit = cos(_LightAngle * (3.14159 / 180.0));

                float strength = scale - angleLimit;

                // -------------------------------
                // 3) Aplicar fuerza
                // -------------------------------
                strength = saturate(strength * _StrengthScalor);

                // -------------------------------
                // 4) Aplicar la intensidad real
                // Si la linterna está apagada → intensidad = 0 → NO se muestra
                // -------------------------------
                strength *= saturate(_Intensity);

                // -------------------------------
                // 5) Color final
                // -------------------------------
                half4 color = tex2D(_BaseMap, IN.uv) * _BaseColor;
                color.a *= strength;

                // Si la alpha queda muy baja → descartar pixel
                clip(color.a - 0.01);
                // float3 lightDir = normalize(_LightPosition - IN.positionWS);
                // float scale = dot(lightDir, normalize(_LightDirection));

                // float strength = scale - cos(_LightAngle * (3.14159 / 180.0));
                // strength = saturate(strength * _StrengthScalor);

                // half4 color = tex2D(_BaseMap, IN.uv) * _BaseColor;
                // color.a *= strength;

                // // ❗ ESTA LÍNEA ES LA MAGIA
                // clip(color.a - 0.01);

                return color;
            }
            ENDHLSL
        }
    }

    FallBack Off
}
