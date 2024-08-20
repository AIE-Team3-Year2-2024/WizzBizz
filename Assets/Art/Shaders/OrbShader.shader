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
        Tags { "Queue"="Transparent" "RenderType"="Transparent" "IgnoreProjector"="True" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Back
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf StandardSpecular fullforwardshadows alpha

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        #include "Includes/Noise.cginc"

        struct Input
        {
            float3 viewDir;
            float3 worldPos;
            float3 worldNormal; 
        };
        half3 _InnerColor;
        half3 _OuterColor;
        half3 _InnerGlow;
        half _Glossiness;
        half _AnimationSpeed;
        half _HeightScale;
        half _Noise1Scale;
        half _Noise2Scale;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

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
            const int minLayers = 16;
            const int maxLayers = 64;
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

        void surf (Input IN, inout SurfaceOutputStandardSpecular o)
        {  
            float t = _Time.x * _AnimationSpeed;
            float3 offset = float3(t,t,t/20.0);
            float3 texCoords = pom(IN.worldNormal + offset, IN.viewDir);

            float n = orbNoise(texCoords);

            float4 c = float4(0.0,0.0,0.0,1.0);
            c.rgb += lerp(_InnerColor.rgb, _OuterColor.rgb, pow(n, 6));

            float4 e = float4(0.0,0.0,0.0,1.0);
            e.rgb += lerp(float3(0.0, 0.0, 0.0), _InnerGlow.rgb, pow(n, 16));

            o.Albedo = c.rgb;
            o.Emission = e.rgb;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
