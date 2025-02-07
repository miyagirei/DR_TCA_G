Shader "Custom/SpriteFillOutline"
{
    Properties
    {
        _FillColor( "Fill Color", Color ) = ( 0,0,0,1 )   // 中身の色（黒）
        _OutlineColor( "Outline Color", Color ) = ( 1,1,1,1 ) // 枠の色（白）
        _OutlineWidth( "Outline Width", Range( 0, 10 ) ) = 1.0 // 枠の太さ
        _MainTex( "Sprite Texture", 2D ) = "white" {}
    }
        SubShader
    {
        Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
        Pass
        {
            Name "FILL & OUTLINE"
            Tags {"LightMode" = "Always"}

            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            ZTest Always

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _FillColor;
            float4 _OutlineColor;
            float _OutlineWidth;

            v2f vert( appdata_t v ) {
                v2f o;
                o.vertex = UnityObjectToClipPos( v.vertex );
                o.uv = v.uv;
                return o;
            }

            // 近傍ピクセルを調べてアウトラインを描画
            fixed4 frag( v2f i ) : SV_Target
            {
                float alpha = tex2D( _MainTex, i.uv ).a;

                if ( alpha < 0.1 ) {
                    discard; // 透明部分はそのまま
                }

                // 近傍のピクセルをチェック
                float2 texSize = float2( 1.0 / _ScreenParams.x, 1.0 / _ScreenParams.y );
                float outline = 0.0;

                outline += tex2D( _MainTex, i.uv + float2( texSize.x * _OutlineWidth, 0 ) ).a;
                outline += tex2D( _MainTex, i.uv - float2( texSize.x * _OutlineWidth, 0 ) ).a;
                outline += tex2D( _MainTex, i.uv + float2( 0, texSize.y * _OutlineWidth ) ).a;
                outline += tex2D( _MainTex, i.uv - float2( 0, texSize.y * _OutlineWidth ) ).a;

                if ( outline > 0.1 && alpha < 0.9 ) {
                    return _OutlineColor; // 枠の色（白）
                }

                return fixed4( _FillColor.rgb, alpha ); // 中身の色（黒）
            }
            ENDCG
        }
    }
}
