Shader "Custom/first/myshader"
{

	//Variables
	Properties
	{
		_MainTex ("MainTexture", 2D) = "white"{}
		_Color("Color", Color) = (1,1,1,1)

		_DissolveTex("SecondTexture", 2D) = "white" {}
		_DissolveAmount("BlendAmound", float) =1

		_Extrude("Extrude Amount", Range(-0.1, 0.1)) = 0.3
	}
	SubShader{

		Pass{
			CGPROGRAM
			//#pragma enable_d3d11_debug_symbols
			#pragma vertex vertexFunction
			#pragma fragment fragmentFunction
			#include "UnityCG.cginc"

			//Verticies
			//Normal
			//Color
			//UV
			struct appdata {
				float4 vertex : POSITION;
				float2 uv :TEXCOORD0;
				float3 normal : NORMAL;
			};
			
			struct v2f {
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
			};


			float4 _Color;
			sampler2D _MainTex;
			float _Extrude;

			//Build the object
			v2f vertexFunction(appdata IN) {
			v2f OUT;

			IN.vertex.xyz += IN.normal.xyz  * _Extrude;

			OUT.position = mul(UNITY_MATRIX_MVP, IN.vertex);
			OUT.uv = IN.uv;
			

			return OUT;

			}

			

			//Vertex

			

			//Fragment
			//Color it
			fixed4 fragmentFunction(v2f IN) : sv_Target{

				float4 textureColor = tex2D(_MainTex, IN.uv);
				float4 dissolveColor = tex2D(_DisolveTed, In.uv)

				return textureColor * _Color;
			}

			ENDCG
		}
	}


}