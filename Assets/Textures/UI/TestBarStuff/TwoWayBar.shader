Shader "Custom/Two Way Progress Bar" {

	Properties {
		_BorderTexture ("Border Texture (RGBA)", 2D) = "white" {}
		_BarTexture ("Bar Texture(RGBA)", 2D) = "white" {}
		_BorderColor ("Border Color", Color) = (1,1,1,1)
		_BarColor ("Bar Color", Color) = (1,1,1,1)
		_BarBGColor ("Bar Background Color", Color) = (1,1,1,1)
		_ProgressH ("Progress H", Range(-1,1)) = 0.0
		_ProgressV ("Progress V", Range(-1,1)) = 0.0
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
			
			uniform sampler2D _BorderTexture, _BarTexture;
			uniform float4 _BarColor, _BorderColor, _BarBGColor;
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
				half4 b = tex2D( _BorderTexture, i.uv);
				half4 f = tex2D( _BarTexture, i.uv);
				half4 col = b.a * _BorderColor;
				
				if(_ProgressH > 0){
					if( i.uv.x > 0.5){
						if( (i.uv.x*2 - 1) < _ProgressH){
							col += f.a * _BarColor;
						}else{
							col += f.a * _BarBGColor;
						}
					}else{
						col += f.a * _BarBGColor;
					}
				}else{
					if( i.uv.x < 0.5){
						if( (i.uv.x*2 - 1) > _ProgressH){
							col += f.a * _BarColor;
						}else{
							col += f.a * _BarBGColor;
						}
					}else{
						col += f.a * _BarBGColor;
					}
				}
				
				if(_ProgressV > 0){
					if( i.uv.y > 0.5){
						if( (i.uv.y*2 - 1) < _ProgressV){
							col += f.a * _BarColor;
						}else{
							col += f.a * _BarBGColor;
						}
					}else{
						col += f.a * _BarBGColor;
					}
				}else{
					if( i.uv.y < 0.5){
						if( (i.uv.y*2 - 1) > _ProgressV){
							col += f.a * _BarColor;
						}else{
							col += f.a * _BarBGColor;
						}
					}else{
						col += f.a * _BarBGColor;
					}
				}

			    return col;
			}
		ENDCG
	    }
	}
}