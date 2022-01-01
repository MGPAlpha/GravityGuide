Shader "Unlit/Raymarch"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Main Color", Color) = (1,1,1,1)
        _CameraPos ("Camera Position", Vector) = (0,0,-6,0)
        _RotSpeeds ("Rotation Speeds", Vector) = (.15, .21, .35, 2)
        _RotStarts ("Rotation Speeds", Vector) = (0, 0, 0, 0)
    }
    SubShader
    {
        Tags {     
            "RenderType"="Transparent"
            "Queue"="Transparent"
        }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float4 _CameraPos;
            float4 _RotSpeeds;
            float4 _RotStarts;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float sdBox( float3 p, float3 b )
            {
                
                float3 q = abs(p) - b;
                return length(max(q,0.0)) + min(max(q.x,max(q.y,q.z)),0.0);
            }

            float sdOctahedron( float3 p, float s)
            {
                p = abs(p);
                return (p.x+p.y+p.z-s)*0.57735027;
            }

            float sdTorus( float3 p, float2 t )
            {
                float2 q = float2(length(p.xz)-t.x,p.y);
                return length(q)-t.y;
            }

            float sdCappedCylinder( float3 p, float h, float r )
            {
                float2 d = abs(float2(length(p.xz),p.y)) - float2(h,r);
                return min(max(d.x,d.y),0.0) + length(max(d,0.0));
            }

            float getDist(float3 p) {
                float time1 = _Time.y * _RotSpeeds.x + _RotStarts.x;
                float time2 = _Time.y * _RotSpeeds.y + _RotStarts.y;
                float time3 = _Time.y * _RotSpeeds.z + _RotStarts.z;
                float time4 = _Time.y * _RotSpeeds.a + _RotStarts.a;
                float3x3 rot90degMat = float3x3(
                    1,  0,  0,
                    0,  0,  1,
                    0,  -1, 0
                );
                float3x3 rot90degMat2 = float3x3(
                    0,  1,  0,
                    -1, 0,  0,
                    0,  0,  1
                );
                float3x3 torusRotMat1 = float3x3(
                    cos(time1),   0,  -sin(time1),
                    0,            1,  0,
                    sin(time1),   0,  cos(time1)
                );
                torusRotMat1 = mul(rot90degMat, torusRotMat1);
                float3x3 torusRotMat2 = float3x3(
                    1,  0,            0,
                    0,  cos(time2),   -sin(time2),
                    0,  sin(time2),   cos(time2)
                );
                float3x3 torusRotMat3 = float3x3(
                    cos(time3),   -sin(time3),  0,
                    sin(time3),   cos(time3),   0,
                    0,            0,            1
                );
                float3x3 boxRotMat1 = float3x3(
                    .577,   -.577,   .577,
                    .577,   .789,   .211,
                    -.577,  .211,   .789
                    
                );
                float3x3 boxRotMat2 = float3x3(
                    1,  0,            0,
                    0,  cos(time4),   -sin(time4),
                    0,  sin(time4),   cos(time4)
                );
                boxRotMat2 = mul(boxRotMat1, boxRotMat2);
                // float3x3 rotMatTotal = mul(rotMat1, rotMat2);
                float3 torusRot1 = mul(torusRotMat1, p);
                float3 torusRot2 = mul(torusRotMat2, torusRot1);
                float3 torusRot3 = mul(torusRotMat3, torusRot2);
                float3 axisRot = mul(rot90degMat2, torusRot3);
                float3 boxRot = mul(boxRotMat2, torusRot3);
                
                float dist = sdTorus(torusRot1, float2(2,.2));
                dist = min(dist, sdTorus(torusRot2, float2(1.5,.2)));
                dist = min(dist, sdTorus(torusRot3, float2(1,.2)));
                dist = min(dist, sdCappedCylinder(axisRot, .1, 1));
                dist = min(dist, sdBox(boxRot, float3(.4,.4,.4)));
                return dist;
            }

            #define MAX_STEPS 100
            #define SURFACE_DIST .01
            #define MAX_DIST 100

            float rayMarch(float3 ro, float3 rd) {
                float dO = 0;
                for (int i = 0; i < MAX_STEPS; i++) {
                    float3 p = ro+dO*rd;
                    float dS = getDist(p);
                    dO += dS;
                    if (dS < SURFACE_DIST || dO > MAX_DIST) break;
                }
                return dO;
            }

            float3 getNormal(float3 p) {
                float d = getDist(p);
                float2 e = float2(.01, 0);

                float3 n = d - float3(
                    getDist(p-e.xyy),
                    getDist(p-e.yxy),
                    getDist(p-e.yyx)
                );

                return normalize(n);
            }

            float getLight(float3 p) {
                float3 l = normalize(float3(.0,0,-1));
                float3 n = getNormal(p);

                float dif = dot(n, l);

                return dif;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float3 col = float3(0,0,0);

                float3 worldScale = float3(
                    length(float3(unity_ObjectToWorld[0].x, unity_ObjectToWorld[1].x, unity_ObjectToWorld[2].x)), // scale x axis
                    length(float3(unity_ObjectToWorld[0].y, unity_ObjectToWorld[1].y, unity_ObjectToWorld[2].y)), // scale y axis
                    length(float3(unity_ObjectToWorld[0].z, unity_ObjectToWorld[1].z, unity_ObjectToWorld[2].z))  // scale z axis
                );
                float2 posterizeScale = float2(16, 16) * worldScale.xy;
                float2 posterizeUV = floor(i.uv * posterizeScale) / posterizeScale;

                // camera
                float3 ro = _CameraPos.xyz + float3((posterizeUV - float2(.5, .5)) * 5, 0);
                float3 rd = normalize(float3(0, 0, 1));
                
                float d = rayMarch(ro, rd);

                float3 p = ro + rd * d;

                float dif = getLight(p);

                col = float3(d,d,d);
                
                float light = getLight(p);

                // return half4(max(light,0).xxx, 1);
                return half4(getNormal(p) * float3(.5, .5, -1) + float3(.5,.5,0), step(0, light));
                return half4(_Color.xyz, max(light,0));
            }
            ENDCG
        }
    }
}
