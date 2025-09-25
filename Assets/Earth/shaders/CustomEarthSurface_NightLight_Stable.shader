Shader "Custom/EarthSurface_Final"
{
    Properties
    {
        // 基础地表贴图
        [MainTexture] _MainTex ("Day Texture", 2D) = "white" {}
        [Normal] _BumpMap ("Terrain Normal", 2D) = "bump" {}
        [NoScaleOffset] _NightTex ("City Lights", 2D) = "black" {}
        
        // 参数控制
        _NightIntensity ("Night Brightness", Range(1, 20)) = 8.0
        _SpecularPower ("Ocean Gloss", Range(0, 1)) = 0.8  // 海洋高光强度[10](@ref)
    }

    SubShader
    {
        Tags { 
            "RenderType"="Opaque" 
            "RenderPipeline"="UniversalPipeline"
            "Queue"="Geometry"
        }

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        
        // 纹理声明（避免重复定义）
        TEXTURE2D(_MainTex);     SAMPLER(sampler_MainTex);
        TEXTURE2D(_BumpMap);     SAMPLER(sampler_BumpMap);
        TEXTURE2D(_NightTex);    SAMPLER(sampler_NightTex);
        ENDHLSL

        Pass
        {
            Name "EarthPass"
            Tags { "LightMode"="UniversalForward" }
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            
            // 材质属性（移除了_SpecColor）[7,9](@ref)
            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float _NightIntensity;
                float _SpecularPower;
            CBUFFER_END

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float4 positionCS : SV_POSITION;
                float3 worldNormal : TEXCOORD1;
                float3 viewDir : TEXCOORD2;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            Varyings vert(Attributes IN)
            {
                Varyings OUT;
                UNITY_SETUP_INSTANCE_ID(IN);
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = TRANSFORM_TEX(IN.uv, _MainTex);
                
                // 计算世界空间法线和视角方向[10](@ref)
                VertexNormalInputs normalInput = GetVertexNormalInputs(IN.normal);
                OUT.worldNormal = normalInput.normalWS;
                OUT.viewDir = GetWorldSpaceNormalizeViewDir(TransformObjectToWorld(IN.positionOS.xyz));
                
                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {
                // 1. 采样地表贴图
                half4 baseColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, IN.uv);
                
                // 2. 采样夜景贴图（直接叠加发光效果）
                half4 nightData = SAMPLE_TEXTURE2D(_NightTex, sampler_NightTex, IN.uv);
                baseColor.rgb += nightData.rgb * _NightIntensity * nightData.a;
                
                // 3. 采样地形法线
                half3 normalData = UnpackNormal(SAMPLE_TEXTURE2D(_BumpMap, sampler_BumpMap, IN.uv));
                
                // 4. 海洋高光计算（使用URP内置光照）[10](@ref)
                Light mainLight = GetMainLight();
                half3 halfDir = normalize(normalize(mainLight.direction) + IN.viewDir);
                half specular = pow(saturate(dot(normalData, halfDir)), _SpecularPower * 128);
                
                // 5. 最终合成（高光仅影响海洋区域）
                half oceanMask = step(0.7, baseColor.b); // 蓝色通道识别海洋
                half3 finalColor = baseColor.rgb + specular * oceanMask * mainLight.color;
                
                return half4(finalColor, 1.0);
            }
            ENDHLSL
        }
    }

    FallBack "Universal Render Pipeline/Lit"
}