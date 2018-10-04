Shader "Hidden/PlayerView"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_FX ("FX", 2D) = "white" {}
		_Blend ("Black & White blend", Range (0, 1)) = 0
		_Brightness ("Brightness", Range (0, 2)) = 0
		_Contrast ("Contrast", Range (0, 2)) = 0
	}
	SubShader
	{
		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag

			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform sampler2D _FX;
			uniform float _Blend;
			uniform float _Brightness;
			uniform float _Contrast;

			float4 frag(v2f_img i) : COLOR
			{
				float4 c = tex2D(_MainTex, i.uv);
				float4 fx = tex2D(_FX, i.uv);

				// Get luminance
				float lum = dot(c.rgb, float3(.3, .59, .11));
				float3 bw = float3( lum, lum, lum );

				float vignette = 1 - length(i.uv - float2(.5, .5));

				vignette = pow(vignette, 4 * _Blend) * 2;
				vignette = saturate(vignette);

				bw *= lerp(1, vignette, 0.5);

				c.rgb = lerp(c.rgb, bw.rgb, _Blend);

				return c;
				
				// Get luminance
				//float lum = dot(c, float3(.3, .59, .11));
				//float3 bw = float3( lum, lum, lum );

				// Apply contrast.
				bw = ((bw - 0.5f) * max(_Contrast, 0)) + 0.5f;

				// Apply brightness.
				bw += _Brightness;

				// Blend
				bw = lerp(c.rgb, bw.rgb, _Blend);

				//float vignette = 1 - length(i.uv - float2(.5, .5));

				//vignette = pow(vignette, 4 * _Blend) * 2;
				//vignette = saturate(vignette);

				//bw *= vignette;

				// Blood vignette
				if (_Blend < 0.9999)
					bw *= lerp(float3(1, 1, 1), float3(1, 0, 0), 1 - vignette);

				return float4(bw, 1.0);
			}
			ENDCG
		}
	}
}