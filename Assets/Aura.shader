Shader "Unlit/Aura"
{
    Properties
    {
        _MainTex ("Sprite Texture", 2D) = "white" {}
        _OverlayTex ("Overlay Texture", 2D) = "Arrows" {}
        _OverlaySpeed ("Overlay Speed", float) = 10
        _OverlayDirection ("Overlay Direction", Vector) = (0,0,0,0)
        _WobbleFreq ("Wobble Frequency", float) = 16
        _WobbleAmp ("Wobble Amplitude", float) = .05
        _WobbleSpeed ("Wobble Speed", float) = 5
        _WobbleVarySpeed ("Wobble Variation Rate", float) = 12
        _InnerRad ("Inner Radius", float) = .45
        _PPU ("Pixels per Unit", int) = 16
        _PixelsOnScreenX ("Pixels on Screen X", float) = 320
        _PixelsOnScreenY ("Pixels on Screen Y", float) = 180
        _DistortionFreqX ("Distortion Frequency X", float) = 30
        _DistortionFreqY ("Distortion Frequency Y", float) = 30
        _DistortionAmpX ("Distortion Amplitude X", float) = 100
        _DistortionAmpY ("Distortion Amplitude Y", float) = 100
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        ZWrite off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull off

        GrabPass {
            "_BackgroundTexture"
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 color : TEXCOORD1;
                half pixelsInQuad : TEXCOORD2;
                float4 grabPos : TEXCOORD3;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _OverlayTex;
            float4 _OverlayTex_ST;
            float _OverlaySpeed;
            float2 _OverlayDirection;
            float _WobbleFreq;
            float _WobbleAmp;
            float _WobbleSpeed;
            float _WobbleVarySpeed;
            float _InnerRad;
            int _PPU;
            float _PixelsOnScreenX;
            float _PixelsOnScreenY;

            float _DistortionFreqX;
            float _DistortionFreqY;
            float _DistortionAmpX;
            float _DistortionAmpY;
            
            sampler2D _BackgroundTexture;

            half3 ObjectScale() {
                return half3(
                    length(unity_ObjectToWorld._m00_m10_m20),
                    length(unity_ObjectToWorld._m01_m11_m21),
                    length(unity_ObjectToWorld._m02_m12_m22)
                );
            }

            float4 AlphaBlend(float4 a, float4 b) {
                float4 o;
                o.a = a.a + b.a*(1-a.a);
                o.rgb = (a.rgb*a.a+b.rgb*b.a*(1-a.a))/o.a;

                // o.rgb = a.rgb + b.rgb * (1 - a.a);
                // o.a = a.a + b.a * (1 - a.a);

                return o;
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.grabPos = ComputeGrabScreenPos(o.vertex);
                
                // o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv = v.uv;
                o.color = v.color;
                half scale = ObjectScale().x;
                o.pixelsInQuad = scale * _PPU;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 pixelUv = i.uv;

                pixelUv = (floor(pixelUv * i.pixelsInQuad) + .5) / i.pixelsInQuad;
                
                // sample the texture
                // fixed4 col = tex2D(_MainTex, i.uv);
                float2 rad = pixelUv - float2(.5, .5);
                float angle = atan2(rad.y, rad.x);
                float wobble = sin(angle * _WobbleFreq + _Time.y * _WobbleSpeed) * sin(_Time.y * _WobbleVarySpeed) * (_WobbleAmp / 2) + _WobbleAmp / 2;
                float wobbleLen = length(rad) + wobble;
                fixed4 col;
                // if (wobbleLen > .5) return 0;
                if (wobbleLen < .5 && wobbleLen > _InnerRad) col = i.color;
                else if (wobbleLen < _InnerRad) {
                    
                    float4 pixelGrabPos = float4((floor(i.grabPos.x * _PixelsOnScreenX) + .5) / _PixelsOnScreenX, (floor(i.grabPos.y * _PixelsOnScreenY) + .5) / _PixelsOnScreenY, i.grabPos.zw);
                    col = tex2Dproj(_BackgroundTexture, float4(pixelGrabPos.x + sin(_Time.z * 5 + pixelGrabPos.y * _DistortionFreqY) / _DistortionAmpX, pixelGrabPos.y + sin(_Time.z * 5+ pixelGrabPos.x * _DistortionFreqX) / _DistortionAmpY,pixelGrabPos.zw));
                    
                    float a = atan2(_OverlayDirection.x, _OverlayDirection.y);

                    float2x2 overlayRotMat = float2x2(cos(a), -sin(a), sin(a), cos(a) );

                    // float4 overlayCol = float4(mul(overlayRotMat, (pixelUv*2)),0,0);

                    float2 overlaySample = float2(0, _Time.x * -_OverlaySpeed);

                    float2 rotPixelUv = mul(overlayRotMat, pixelUv*2 - float2(1, 1)) + float2(1, 1);

                    overlaySample += rotPixelUv;

                    float4 overlayCol = tex2D(_OverlayTex, float4(overlaySample, 0,0));
                    // col = float4((col.rgb*col.a+overlayCol.rgb*overlayCol.a),col.a+overlayCol.a);
                    
                    overlayCol = float4(i.color.rgb, overlayCol.r);

                    col = AlphaBlend(overlayCol, col);

                } else return 0;
                col.a = i.color.a;
                return col;
            }
            ENDCG
        }
    }
}
