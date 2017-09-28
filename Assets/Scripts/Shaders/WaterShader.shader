Shader "Unlit/WaterShader"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1)
		_Frequency ("Frequency", Float) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" }
		LOD 100

		Pass
		{
Blend SrcAlpha OneMinusSrcAlpha
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
			float _Frequency;
			fixed4 _Color;

			float get(fixed3 pos){
				float n =snoise(pos);
				return n;
			}
			float wave(float value){
				return (sin(snoise(value/1000)*_Time*_Frequency)+1)/2.;
			}
			v2f vert (appdata v)
			{
				v2f o;
				o.worldPos = mul(unity_ObjectToWorld, v.vertex) ;
				o.vertex.y += wave(o.worldPos.x);
				o.vertex = mul(unity_WorldToObject,v.vertex);
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.vertex += fixed4(0,1,0,0)*get(normalize(o.worldPos))*5.;
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			

			fixed4 frag (v2f i) : SV_Target
			{
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				fixed3 n = normalize(i.worldPos);

				return _Color;//*0.8+(wave(n.x)+wave(n.y*4)+wave(n.z*8))/3.*0.2*_Color,0.5);
			}
			ENDCG
		}
	}
}
