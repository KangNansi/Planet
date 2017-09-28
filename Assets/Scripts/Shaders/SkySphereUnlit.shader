Shader "Unlit/SkySphereUnlit"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Frequency ("Frequency", Float) = 1
		_Sun ("Sun", Vector) = (0, 2, 0)
		_SkyState ("SkyState", Float) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Cull Off 

		Pass
		{

			CGPROGRAM
			#include "Assets/HLSL/SimplexNoise3D.hlsl"
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
				float3 worldPos : TEXCOORD0;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed _Frequency;
			float3 _Sun;
			float _SkyState;
			


			v2f vert (appdata v)
			{
				v2f o;
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}

			float3 getSkyColor(float scalar){
				fixed3 color = clamp((_SkyState+1)/2., 0, 1) * fixed3(0.7,0.7,1);
				fixed sunset = 0;
				fixed3 sunsetColor = fixed3(0.8,0.6,0.5);
					sunset = 1-abs(_SkyState);
				return (1-sunset)*color + sunset*sunsetColor;
			}
			
			fixed4 frag (v2f i, out float depth:DEPTH) : SV_Target
			{			
				depth = 0;
				fixed4 col;
				fixed3 normal = normalize(i.worldPos);
				fixed n = snoise(normal*_Frequency);
				fixed n3 = snoise(fixed3(normal.x, normal.y, normal.z/3))/3+0.3;
				if(n<0.8) n = 0;
				else n = (n-0.8)/0.2;
				fixed scalar = dot(_Sun, normal);
				fixed worldDot = dot(_Sun,normal);
				fixed3 sky = getSkyColor(worldDot);
				scalar = scalar-0.99;
				scalar *= 100;
				scalar = clamp(scalar, 0, 1);
				col.xyz = (n*(1-clamp(_SkyState, 0, 1)))*fixed3(1,1,1)+scalar*scalar*scalar + 0*n3*fixed3(0.1,0.05,0.3) + sky;
				col.a = 1;
				return col;
			}
			ENDCG
		}
	}
}
