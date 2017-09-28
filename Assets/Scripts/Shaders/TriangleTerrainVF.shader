// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Custom/TriangleTerrainVF"
{
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_Frequency ("Frequency", Float) = 1
		_Frequency2 ("Frequency2", Float) = 1
		_Frequency3 ("Frequency3", Float) = 1
		_Frequency4 ("Frequency4", Float) = 1
	}
	SubShader
	{
				Tags { "RenderType"="Opaque" }

		Pass
		{
			CGPROGRAM


			#include "Assets/HLSL/SimplexNoise3D.hlsl"
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#pragma target 3.0

			struct appdata
			{
				float4 vertex : POSITION;
				//float2 uv : TEXCOORD0;
				float4 color : COLOR;
			};

			struct v2f
			{
				//float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
				float4 color : COLOR;
				float3 worldPos : TEXCOORD0;
			};

			v2f vert (appdata v)
			{
				v2f o;
				o.worldPos = mul(unity_ObjectToWorld, v.vertex) ;
				o.vertex = UnityObjectToClipPos(v.vertex);
				//o.uv = v.uv;
				o.color = v.color;
				return o;
			}
			
			fixed4 _Color;
			fixed _Frequency;
			fixed _Frequency2;
			fixed _Frequency3;
			fixed _Frequency4;

			fixed4 frag (v2f i) : SV_Target
			{
				float n1 = snoise(i.worldPos*_Frequency);
				float n2 = snoise(i.worldPos*_Frequency2);
				float n3 = snoise(i.worldPos*_Frequency3);
				float n4 = snoise(i.worldPos*_Frequency4);

				fixed4 col = (n1*0.5+n2*0.25+n3*0.125+n4*0.125)*_Color;
				// just invert the colors 
				
				return col;
			}
			ENDCG
		}
	}
}
