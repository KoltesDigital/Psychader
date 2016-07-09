Shader "Custom/Fullscreen"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
	}
	SubShader
	{
		ZWrite Off
		ZTest Always

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
				float2 uv : TEXCOORD0;
			};

			struct Varyings
			{
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			Varyings vert(Attributes attributes)
			{
				Varyings varyings;
				varyings.position = float4(attributes.uv * 2.0 - 1.0, 0.0, 1.0);
				varyings.uv = attributes.uv;
				return varyings;
			}

			uniform sampler2D _MainTex;
			
			float rand(float2 co) { return frac(sin(dot(co.xy, float2(12.9898, 78.233))) * 43758.5453); }
			float rand(float2 co, float l) { return rand(float2(rand(co), l)); }

			fixed4 frag(Varyings varyings) : SV_Target
			{
				float4 color = tex2D(_MainTex, varyings.uv);

				float noise = rand(varyings.uv, _Time);
				color *= 0.8 + 0.2 * noise;

				// Vignette IQ
				float2 position = varyings.uv * 2.0 - 1.0;
				color *= 0.5 + 0.5 * pow(
					(position.x + 1.0) *
					(position.y + 1.0) *
					(position.x - 1.0) *
					(position.y - 1.0),
					0.1);

				return color;
			}

			ENDCG
		}
	}
}
