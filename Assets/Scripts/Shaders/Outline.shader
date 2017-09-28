// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Outline"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color ("Color", Color) = ( 1, 1, 1)
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Off

		 Pass {
            Tags {"RenderType"="Opaque"}
            ZWrite On ZTest Always Fog { Mode Off }
        
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            fixed4 _Color;
            float4 vert(float4 v:POSITION) : POSITION {
                return UnityObjectToClipPos (v);
            }
            fixed4 frag() : SV_Target {
                return fixed4(_Color.rgb * _Color.a, 1.0);
            }
            ENDCG
        }    

		Pass {
                CGPROGRAM
                #pragma vertex vert_img
                #pragma fragment frag
                #pragma fragmentoption ARB_precision_hint_fastest
                #include "UnityCG.cginc"
            
                sampler2D _MainTex;
                sampler2D _OccludeMap;
            
                half4 frag(v2f_img IN) : COLOR {
                    return tex2D (_MainTex, IN.uv) - tex2D(_OccludeMap, IN.uv);
                }
                ENDCG
        }

		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;
			sampler2D _OccludeMap;

			fixed4 frag (v2f_img i) : SV_Target
			{
				fixed4 added = tex2D(_OccludeMap, i.uv);
				if (added.r != 0)
					return added*0.5 + 0.5*tex2D(_MainTex, i.uv);
				return tex2D (_MainTex, i.uv);
			}
			ENDCG
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#include "UnityCG.cginc"
			
			sampler2D _MainTex;

			fixed4 frag (v2f_img i) : SV_Target
			{
				fixed4 c = tex2D (_MainTex, i.uv);
				for(int j = 0; j < 5; j++)
					c+= tex2D (_MainTex, i.uv+fixed2(0.05+(j*0.1), 0));
				return c/5;
			}
			ENDCG
		}
	}
}
