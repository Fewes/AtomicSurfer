Shader "WaterShader"
{
    Properties
    {
        _MainTex ( "Base (RGB)", 2D ) = "white" {}
        _XScrollSpeed ( "X Scroll Speed", Float ) = 1
        _YScrollSpeed ( "Y Scroll Speed", Float ) = 1
        _VColIntensity ( "VCol Intensity", Float ) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
       
        CGPROGRAM
        #pragma surface surf Lambert
 
        sampler2D _MainTex;
        float _XScrollSpeed;
        float _YScrollSpeed;
        float _VColIntensity;
 
        struct Input
        {
            float2 uv_MainTex;
            float4 color : COLOR;
        };

        float3 Overlay (float3 cBase, float3 cBlend)
		{
		/*
			float4 cNew;
			if (cBase.r > .5) { cNew.r = 1 - (1 - 2 * (cBase.r - .5)) * (1 - cBlend.r); }
			else { cNew.r = (2 * cBase.r) * cBlend.r; }
			
			if (cBase.g > .5) { cNew.g = 1 - (1 - 2 * (cBase.g - .5)) * (1 - cBlend.g); }
			else { cNew.g = (2 * cBase.g) * cBlend.g; }
			
			if (cBase.b > .5) { cNew.b = 1 - (1 - 2 * (cBase.b - .5)) * (1 - cBlend.b); }
			else { cNew.b = (2 * cBase.b) * cBlend.b; }
			
			cNew.a = 1.0;
			return cNew;
		*/
			// Vectorized (easier for compiler)
			float3 cNew;
			
			// overlay has two output possbilities
			// which is taken is decided if pixel value
			// is below half or not

			cNew = step(0.5,cBase);
			
			// we pick either solution
			// depending on pixel
			
			// first is case of < 0.5
			// second is case for >= 0.5
			
			// interpolate between the two, 
			// using color as influence value
			cNew= lerp((cBase*cBlend*2),(1.0-(2.0*(1.0-cBase)*(1.0-cBlend))),cNew);

			return cNew;
		}
 
        void surf( Input IN, inout SurfaceOutput o )
        {
            fixed2 scrollUV = IN.uv_MainTex;
            fixed xScrollValue = _XScrollSpeed * _Time.x;
            fixed yScrollValue = _YScrollSpeed * _Time.x;
            scrollUV += fixed2( xScrollValue, yScrollValue );
            half4 c = tex2D( _MainTex, scrollUV );
            //o.Albedo = c.rgb;
            o.Emission = lerp(c, Overlay(IN.color.rgb, c.rgb), _VColIntensity);
            o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}