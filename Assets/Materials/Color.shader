Shader "Custom/Color"
{
	Properties
	{
		_Color("Color", Color) = (1.0, 1.0, 1.0, 1.0)
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

			uniform float4 _Color;

			fixed4 frag(Varyings varyings) : SV_Target
			{
				return _Color;
			}

			ENDCG
		}
	}
}
