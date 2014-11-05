Shader "ForceX/Progress Bar Lte" {

	Properties {
		_BarBGColor ("Bar Background Color", Color) = (1,1,1,1)
		_BarColor ("Bar Color", Color) = (1,1,1,1)
		_ProgressH ("Progress H", Range(0.0,1)) = 0.0
		_ProgressV ("Progress V", Range(0.0,1)) = 0.0
	}
	
	SubShader {
		Tags { "Queue"="Overlay+1" }
		ZTest Always
		Blend SrcAlpha OneMinusSrcAlpha
        Pass {
			
			CGPROGRAM
			#include "UnityCG.cginc"
			#pragma vertex vert
			#pragma fragment frag
			
			
			uniform float4 _BarBGColor, _BarColor;
			uniform float _ProgressH, _ProgressV;
			
			struct v2f {
				float4 pos : POSITION;
				float2 uv : TEXCOORD0;
			};
			
			v2f vert (appdata_base v){
			    v2f o;
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
			    o.uv = TRANSFORM_UV(0);
			    return o;
			}
			
			half4 frag( v2f i ) : COLOR{
				half4 col = _BarBGColor;
			
				if( i.uv.x < _ProgressH){
					col =  _BarColor;
				}
				if( i.uv.y < _ProgressV){
					col =  _BarColor;
				}				
			    return col;
			}
			ENDCG
	    }
	}
}
