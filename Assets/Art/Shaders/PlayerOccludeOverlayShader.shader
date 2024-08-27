Shader "Custom/PlayerOccludeOverlay"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Overlay" "PerformanceChecks"="False" }
        LOD 100
     
        Stencil
        {
            Ref 2
            ReadMask 255
            WriteMask 255
            Comp LEqual
            Pass Zero
        }

        Pass
        {
            ZWrite On
            ZTest GEqual
            Blend SrcAlpha OneMinusSrcAlpha
        
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct v2f
            {
                float4 pos : SV_POSITION;
                float4 screenPos : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            fixed4 _Color;

            v2f vert(appdata_base v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.screenPos = ComputeScreenPos(o.pos);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 texCoords = i.screenPos.xy / i.screenPos.w;
                float aspect = _ScreenParams.x / _ScreenParams.y;
                texCoords.x = texCoords.x * aspect;
                texCoords = TRANSFORM_TEX(texCoords, _MainTex);
                fixed4 c = tex2D(_MainTex, texCoords);
                c *= _Color;
                return c;
            }

            ENDCG
        }
    }

    FallBack "VertexLit"
}
