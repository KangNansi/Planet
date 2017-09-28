Shader "Custom/TriangleTerrain" {
	Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_Rock ("Rock", Color) = (1,1,1)
		_Sand ("Sand", Color) = (1,1,1)
		_Grass ("Second", Color) = (1,1,1)
		_Dirt ("Second", Color) = (1,1,1)
		_Slope ("Slope", Float) = 50
		_HeightFactor ("Height Factor", Float) = 1000
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		
		CGPROGRAM
		#include "Assets/HLSL/SimplexNoise3D.hlsl"
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting


		struct Input {
			float2 uv_MainTex : TEXCOORD0;
			float3 worldPos;
			float4 color : COLOR;
			float3 normal;
		};

		fixed4 _Rock;
		fixed4 _Sand;
		fixed4 _Dirt;
		fixed4 _Grass;
		fixed _Slope;
		fixed _HeightFactor;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_CBUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_CBUFFER_END

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed f = 100;
			fixed camToPixel = distance(_WorldSpaceCameraPos, IN.worldPos)/3000.;
			if(camToPixel > 0.7)
				camToPixel = 0.7+camToPixel/10.;
			fixed d = clamp(1-camToPixel, 0, 1);
			fixed4 n1 = snoise_grad(IN.color.rgb*f*2);
			fixed4 n2 = snoise_grad((IN.color.rgb)*f*8);
			fixed4 n3 = snoise_grad((IN.color.rgb)*f*32);
			float n6 = snoise((IN.color.rgb)*f*64);
			float s = (n6);
			fixed4 rock = _Rock*((n2.z*0.6+n2.y*0.3+n1.z*0.1)*0.2+0.8);
			fixed4 sand = _Sand*(0.8+s*0.2);
			//n2 = 0.7*n2 + 0.3* snoise((IN.color.rgb)*1000);
			//n2 = 0.9*n2 + 0.1* snoise((IN.color.rgb)*10000);
			fixed4 dirt = _Grass*0.8 + (n1.x*0.5+n2.x*0.25+n3.x*0.125)*_Grass*0.2*d;
			fixed4 second = _Dirt*0.7 +(n1.y*0.5+n2.y*0.25+n3.y*0.125)*_Dirt*0.3*d;
			float sand_height = clamp(IN.uv_MainTex.x*_HeightFactor, 0, 1) ;
			float slope = clamp(((IN.color.a-45)*3+45)/_Slope, 0, 1);
			fixed4 col = clamp(sand_height, 0, 1)*sand + ((1-slope)*second + slope*dirt)*(1-sand_height);
			o.Albedo = (sand_height)*(slope*rock + (1-slope)*sand) + (1-sand_height)*(slope*second + (1-slope)*dirt);
			// Metallic and smoothness come from slider variables
			o.Metallic = 0;
			o.Smoothness = 0;
			o.Alpha = 1;
			//o.Normal = IN.color.r * UnpackNormal( tex2D( _FlatNormal, IN.uv_FlatTex ) ) + IN.color.g*UnpackNormal( tex2D( _SlopeNormal, IN.uv_FlatTex ) );
		}
		ENDCG
	}
	FallBack "Diffuse"
}
