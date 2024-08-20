Shader "Custom/ProtoOrbShader"
{
    Properties
    {
        [HDR] _InnerColor ("Inner Color", Color) = (0,0,0)
        [HDR] _OuterColor ("Outer Color", Color) = (1,1,1)
        [PowerSlider(4)] _FresnelExponent ("Fresnel Exponent", Range(0.25, 4)) = 0.25
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM

        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        struct Input
        {
            float3 worldNormal;
            float3 viewDir;
        };

        half3 _InnerColor;
        half3 _OuterColor;
        float _FresnelExponent;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float fresnel = dot(IN.worldNormal, IN.viewDir);
            fresnel = saturate(1 - fresnel);
            fresnel = pow(fresnel, _FresnelExponent);
            half3 c = lerp(_InnerColor, _OuterColor, fresnel);
            o.Emission = c.rgb;
        }
        ENDCG
    }
    FallBack "Standard"
}
