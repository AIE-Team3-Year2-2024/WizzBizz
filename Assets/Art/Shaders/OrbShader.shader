Shader "Custom/OrbShader"
{
    Properties
    {
        _InnerColor ("Inner Color", Color) = (0.0, 0.555, 0.031)
        _OuterColor ("Outer Color", Color) = (0.0, 0.058, 0.036)
        [HDR] _InnerGlow ("Inner Glow", Color) = (0.164, 1.0, 0.589)
        _Glossiness ("Smoothness", Range(0,1)) = 1.0
        _AnimationSpeed ("Animation Speed", Range(0,10)) = 1.0
        _HeightScale ("Height Scale", Range(0,2)) = 0.5
        _Noise1Scale ("Noise1 Scale", Range(0,16)) = 2.5
        _Noise2Scale ("Noise2 Scale", Range(0,16)) = 4.0
    }
    SubShader
    {
        Tags { "RenderPipeline"="UniversalPipeline" }
        LOD 100

        HLSLINCLUDE
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

        CBUFFER_START(UnityPerMaterial)
            half3 _InnerColor;
            half3 _OuterColor;
            half3 _InnerGlow;
            half _Glossiness;
            half _AnimationSpeed;
            half _HeightScale;
            half _Noise1Scale;
            half _Noise2Scale;
        CBUFFER_END

        ENDHLSL

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }
            //Blend SrcAlpha OneMinusSrcAlpha
            Cull Back
            ZWrite On

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #define _SPECULAR_COLOR
            #pragma shader_feature_local _RECEIVE_SHADOWS_OFF
            //#pragma shader_feature_local_fragment _ _SPECGLOSSMAP _SPECULAR_COLOR
            //#pragma shader_feature_local_fragment _GLOSSINESS_FROM_BASE_ALPHA
            #pragma shader_feature_local_fragment _SPECULAR_SETUP
            #pragma shader_feature_local_fragment _EMISSION
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _ALPHAPREMULTIPLY_ON

            #pragma multi_compile_fwdbasealpha
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/SurfaceInput.hlsl"

            #include "Includes/Noise.cginc"

            struct Attributes
            {
                float4 vertex : POSITION;
                float4 normal : NORMAL;
                float2 uv : TEXCOORD0;
                float4 lightmapUV : TEXCOORD1;
                //UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD2;
                half3 worldNormal : TEXCOORD3;
                half3 viewDir : TEXCOORD4;
                #ifdef _ADDITIONAL_LIGHTS_VERTEX
                    half4 fogFactorAndVertexLight : TEXCOORD5;
                #else
                    half fogFactor : TEXCOORD5;
                #endif
                #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
                    float4 shadowCoord : TEXCOORD6;
                #endif
                DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 1);
                //UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            inline float3 colorburn_blend(in float3 a, in float3 b, in float mix = 1.0)
            {
                float f = 1.0 - mix;
                float3 result = a;

                float tmp = f + mix * b.r;
                if (tmp <= 0.0) 
                    result.r = 0.0;
                else if ((tmp = (1.0 - (1.0 - (a.r)) / tmp)) < 0.0)
                    result.r = 0.0;
                else if (tmp > 1.0)
                    result.r = 1.0;
                else
                    result.r = tmp;

                tmp = f + mix * b.g;
                if (tmp <= 0.0) 
                    result.g = 0.0;
                else if ((tmp = (1.0 - (1.0 - (a.g)) / tmp)) < 0.0)
                    result.g = 0.0;
                else if (tmp > 1.0)
                    result.g = 1.0;
                else
                    result.g = tmp;

                tmp = f + mix * b.b;
                if (tmp <= 0.0) 
                    result.b = 0.0;
                else if ((tmp = (1.0 - (1.0 - (a.b)) / tmp)) < 0.0)
                    result.b = 0.0;
                else if (tmp > 1.0)
                    result.b = 1.0;
                else
                    result.b = tmp;

                return result;
            }

            float orbNoise(float3 st)
            {
                float noise = fbm(st * _Noise1Scale, 2, 2.0, 0.5);
                float noise2 = fbm(st * _Noise2Scale, 2, 4.0, 1.0);
                float finalNoise = colorburn_blend(noise, noise2, 0.333333);
                return finalNoise;
            }

            float3 pom(float3 st, float3 viewDir)
            {
                const float scale = _HeightScale;
                const int minLayers = 8;
                const int maxLayers = 32;
                float numLayers = lerp(maxLayers, minLayers, abs(dot(float3(0, 0, 1), viewDir)));
                float layerDepth = 1.0 / numLayers;
                float currentLayerDepth = 0.0;
                float3 P = viewDir * scale;
                float3 deltaTexCoords = P / numLayers;
                float3 currentTexCoords = st;
                float currentDepthValue = orbNoise(currentTexCoords);
                while (currentLayerDepth < currentDepthValue)
                {
                    currentTexCoords -= deltaTexCoords;
                    currentDepthValue = orbNoise(currentTexCoords);
                    currentLayerDepth += layerDepth;
                }
                float3 prevTexCoords = currentTexCoords + deltaTexCoords;
                float afterDepth = currentDepthValue - currentLayerDepth;
                float beforeDepth = orbNoise(prevTexCoords) - currentLayerDepth + layerDepth;
                float weight = afterDepth / (afterDepth - beforeDepth);
                float3 finalTexCoords = prevTexCoords * weight + currentTexCoords * (1.0 - weight);
                return finalTexCoords;
            }

            Varyings vert(Attributes IN)
            {
                Varyings OUT;

                //UNITY_SETUP_INSTANCE_ID(IN);
                //UNITY_TRANSFER_INSTANCE_ID(IN, OUT);

                VertexPositionInputs positionInputs = GetVertexPositionInputs(IN.vertex.xyz);
                VertexNormalInputs normalInputs = GetVertexNormalInputs(IN.normal.xyz);

                OUT.vertex = positionInputs.positionCS;
                OUT.worldPos = positionInputs.positionWS;
                OUT.worldNormal = NormalizeNormalPerVertex(normalInputs.normalWS);
                OUT.viewDir = GetWorldSpaceNormalizeViewDir(positionInputs.positionWS);

                #if 0
                half3 vertexLight = VertexLighting(positionInputs.positionWS, normalInputs.normalWS);
                half fogFactor = ComputeFogFactor(positionInputs.positionCS.z);

                OUTPUT_LIGHTMAP_UV(IN.lightmapUV, unity_LightmapST, OUT.lightmapUV);
                OUTPUT_SH(OUT.worldNormal.xyz, OUT.vertexSH);

                #ifdef _ADDITIONAL_LIGHTS_VERTEX
                    OUT.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
                #else
                    OUT.fogFactor = fogFactor;
                #endif

                #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
                    OUT.shadowCoord = GetShadowCoord(positionInputs);
                #endif
                #endif

                return OUT;
            }

            half4 frag(Varyings IN) : SV_Target
            {  
                //UNITY_SETUP_INSTANCE_ID(IN);

                // pom & animation
                float animTime = _Time.x * _AnimationSpeed;
                float3 offset = float3(animTime,animTime,animTime/20.0);
                float3 texCoords = pom(IN.worldNormal + offset, IN.viewDir);

                // noise
                float n = orbNoise(texCoords);

                // color
                half4 c = half4(0.0,0.0,0.0,1.0);
                c.rgb += lerp(_InnerColor.rgb, _OuterColor.rgb, pow(n, 6));

                // emission
                half4 e = half4(0.0,0.0,0.0,1.0);
                e.rgb += lerp(float3(0.0, 0.0, 0.0), _InnerGlow.rgb, pow(n, 16));

                #if 0
                SurfaceData surfaceData = (SurfaceData)0;
                surfaceData.occlusion = 1.0;
                surfaceData.albedo = c.rgb;
                surfaceData.emission = e.rgb;
                surfaceData.specular = 0.5;
                surfaceData.smoothness = _Glossiness;
                surfaceData.alpha = c.a;

                InputData inputData = (InputData)0;
                inputData.positionWS = IN.worldPos;
                inputData.normalWS = NormalizeNormalPerPixel(IN.worldNormal);
                inputData.viewDirectionWS = SafeNormalize(IN.viewDir);

                #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
                    inputData.shadowCoord = IN.shadowCoord;
                #elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
                    inputData.shadowCoord = TransformWorldToShadowCoord(IN.worldPos);
                #else
                    inputData.shadowCoord = float4(0,0,0,0);
                #endif

                #ifdef _ADDITIONAL_LIGHTS_VERTEX
                    inputData.fogCoord = IN.fogFactorAndVertexLight.x;
                    inputData.vertexLighting = IN.fogFactorAndVertexLight.yzw;
                #else
                    inputData.fogCoord = IN.fogFactor;
                    inputData.vertexLighting = half3(0,0,0);
                #endif

                inputData.bakedGI = SAMPLE_GI(IN.lightmapUV, IN.vertexSH, inputData.normalWS);
                inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(IN.vertex);
                inputData.shadowMask = SAMPLE_SHADOWMASK(IN.lightmapUV);

                half4 result = UniversalFragmentPBR(inputData, surfaceData);
                result.rgb = MixFog(result.rgb, inputData.fogCoord);
                #endif

                //return result;
                return c;
            }
            ENDHLSL
        }
    }
}
