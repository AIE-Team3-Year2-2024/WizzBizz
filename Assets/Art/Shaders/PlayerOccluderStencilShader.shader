Shader "Custom/PlayerOccluderStencil"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" "PerformanceChecks"="False" }
        LOD 100

        Stencil
        {
            Ref 1
            ReadMask 255
            WriteMask 255
            Comp Always
            Pass Replace
        }

        ZWrite On
        ZTest LEqual
        Cull Off

        CGINCLUDE
            #define UNITY_SETUP_BRDF_INPUT MetallicSetup
        ENDCG

        Pass
        {
            Tags { "LightMode"="ForwardBase" }

            CGPROGRAM
            
            #pragma target 3.0
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma multi_compile_instancing
            #pragma vertex vertBase
            #pragma fragment fragBase
            #include "UnityStandardCoreForward.cginc"

            ENDCG
        }
        Pass
        {
            Tags { "LightMode"="ForwardDelta" }
            Blend SrcAlpha One
            Fog { Color(0,0,0,0) }
            ZWrite Off
            ZTest LEqual

            CGPROGRAM
            
            #pragma target 3.0
            #pragma multi_compile_fwdadd_fullshadows
            #pragma multi_compile_fog
            #pragma vertex vertAdd
            #pragma fragment fragAdd
            #include "UnityStandardCoreForward.cginc"

            ENDCG
        }
        Pass
        {
            Tags { "LightMode"="ShadowCaster" }
            ZWrite On
            ZTest LEqual

            CGPROGRAM
            
            #pragma target 3.0
            #pragma multi_compile_shadowcaster
            #pragma multi_compile_instancing
            #pragma vertex vertShadowCaster
            #pragma fragment fragShadowCaster
            #include "UnityStandardShadow.cginc"

            ENDCG
        }
        Pass
        {
            Tags { "LightMode"="Deferred" }

            CGPROGRAM
            
            #pragma target 3.0
            #pragma exclude_renderers nomrt
            #pragma multi_compile_prepassfinal
            #pragma multi_compile_instancing
            #pragma vertex vertDeferred
            #pragma fragment fragDeferred
            #include "UnityStandardCore.cginc"

            ENDCG
        }
        Pass
        {
            Tags { "LightMode"="Meta" }
            Cull Off

            CGPROGRAM
            
            #pragma shader_feature EDITOR_VISUALIZATION
            #pragma vertex vert_meta
            #pragma fragment frag_meta
            #include "UnityStandardMeta.cginc"

            ENDCG
        }
    }
    FallBack "Diffuse"
}
