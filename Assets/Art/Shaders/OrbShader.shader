Shader "Custom/OrbShader"
{
    Properties
    {
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf StandardSpecular fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        /* discontinuous pseudorandom uniformly distributed in [-0.5, +0.5]^3 */
		float3 random3(float3 c) {
			float j = 4096.0*sin(dot(c,float3(17.0, 59.4, 15.0)));
			float3 r;
			r.z = frac(512.0*j);
			j *= .125;
			r.x = frac(512.0*j);
			j *= .125;
			r.y = frac(512.0*j);
			return r-0.5;
		}

		/* skew constants for 3d simplex functions */
		const float F3 =  0.3333333;
		const float G3 =  0.1666667;

		/* 3d simplex noise */
		float simplex3d(float3 p) {
			 /* 1. find current tetrahedron T and it's four vertices */
			 /* s, s+i1, s+i2, s+1.0 - absolute skewed (integer) coordinates of T vertices */
			 /* x, x1, x2, x3 - unskewed coordinates of p relative to each of T vertices*/
	 
			 /* calculate s and x */
			 float3 s = floor(p + dot(p, float3(F3, F3, F3)));
			 float3 x = p - s + dot(s, float3(G3, G3, G3));
	 
			 /* calculate i1 and i2 */
			 float3 e = step(float3(0.0, 0.0, 0.0), x - x.yzx);
			 e.z = min(e.z, 3.0 - dot(3, float3(1.0, 1.0, 1.0)));
			 float3 i1 = e*(1.0 - e.zxy);
			 float3 i2 = 1.0 - e.zxy*(1.0 - e);
	 	
			 /* x1, x2, x3 */
			 float3 x1 = x - i1 + G3;
			 float3 x2 = x - i2 + 2.0*G3;
			 float3 x3 = x - 1.0 + 3.0*G3;
	 
			 /* 2. find four surflets and store them in d */
			 float4 w, d;
	 
			 /* calculate surflet weights */
			 w.x = dot(x, x);
			 w.y = dot(x1, x1);
			 w.z = dot(x2, x2);
			 w.w = dot(x3, x3);
	 
			 /* w fades from 0.6 at the center of the surflet to 0.0 at the margin */
			 w = max(0.6 - w, 0.0);
	 
			 /* calculate surflet components */
			 d.x = dot(random3(s), x);
			 d.y = dot(random3(s + i1), x1);
			 d.z = dot(random3(s + i2), x2);
			 d.w = dot(random3(s + 1.0), x3);
	 
			 /* multiply d by w^4 */
			 w *= w;
			 w *= w;
			 d *= w;
	 
			 /* 3. return the sum of the four surflets */
			 return dot(d, float4(52.0, 52.0, 52.0, 52.0));
		}

		/* const matrices for 3d rotation */
		const float3x3 rot1 = float3x3(-0.37, 0.36, 0.85,-0.14,-0.93, 0.34,0.92, 0.01,0.4);
		const float3x3 rot2 = float3x3(-0.55,-0.39, 0.74, 0.33,-0.91,-0.24,0.77, 0.12,0.63);
		const float3x3 rot3 = float3x3(-0.71, 0.52,-0.47,-0.08,-0.72,-0.68,-0.7,-0.45,0.56);

		/* directional artifacts can be reduced by rotating each octave */
		float simplex3d_fractal(float3 m) {
			return   0.5333333*simplex3d(mul(m, rot1))
					+0.2666667*simplex3d(2.0*mul(m, rot2))
					+0.1333333*simplex3d(4.0*mul(m, rot3))
					+0.0666667*simplex3d(8.0*m);
		}

        float blend_colorburn(float base, float blend)
        {
            if (base >= 1.0)
                return 1.0;
            else if (blend <= 0.0)
                return 0.0;
            else    
    	        return 1.0 - min(1.0, (1.0-base) / blend);
        }

        struct Input
        {
            float3 worldNormal;
        };

        half _Glossiness;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandardSpecular o)
        {
			float3 normal = IN.worldNormal;
            float noise = 0.5 + 0.5 * simplex3d(normal * 2.5);
            float noise2 = 0.5 + 0.5 * simplex3d_fractal(normal * 2.0);
            float finalNoise = noise2;
            float4 c = float4(finalNoise,finalNoise,finalNoise,1.0);
            o.Albedo = c.rgb;
            o.Smoothness = _Glossiness;
            o.Alpha = 1.0;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
