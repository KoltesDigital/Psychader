Shader "Custom/Depth"
{
	Properties
	{
	}
	SubShader
	{
		Tags
		{
			"RenderType" = "Opaque"
		}

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct Attributes
			{
				float4 position : POSITION;
			};

			struct Varyings
			{
				float4 position : SV_POSITION;
				float depth : TEXCOORD0;
			};

			Varyings vert(Attributes attributes)
			{
				Varyings varyings;
				varyings.position = mul(UNITY_MATRIX_MVP, attributes.position);
				varyings.depth = varyings.position.z;
				return varyings;
			}

			fixed4 frag(Varyings varyings) : SV_Target
			{
				fixed4 col = fixed4(varyings.depth, varyings.depth, varyings.depth, 1.0);
				return col;
			}

			ENDCG
		}
	}
}
