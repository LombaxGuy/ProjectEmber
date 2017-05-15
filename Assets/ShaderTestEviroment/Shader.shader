Shader "Custom/Water"
{
Properties
	{
		_MainTex ("MainTexture", 2D) = "white"{}
		_Color("Color", Color) = (1,1,1,1)

		_Value1("Height", Range(0,1)) = 1
		_Value2("Wavyness", Range(0,1)) = 1
		_Value3("Speed", Range(0,1)) = 1

	}
	SubShader{

				pass{

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

				//Vertex
			sampler2D _MainTex;
			float _Value1;
			float _Value2;
			float _Value3;

			v2f vertexFunction(appdata IN) {
			v2f OUT;

			IN.vertex.xyz += IN.normal.xyz * (sin((IN.vertex.x + _Time * _Value3) * _Value2) * 0 + cos((IN.vertex.z + _Time * _Value3) * _Value2)) * _Value1;

			OUT.position = mul(UNITY_MATRIX_MVP, IN.vertex);
			OUT.uv = IN.uv;
			

			return OUT;

			}

			fixed4 fragmentFunction(v2f IN) : sv_Target{

			float4 textureColor1 = tex2D(_MainTex, IN.uv);
			

			return textureColor1;

			}

			ENDCG

		}
}
}

