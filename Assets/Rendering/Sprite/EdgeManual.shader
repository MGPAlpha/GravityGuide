/***************************************************************************************************
* THIS SHADER IS MAINLY BASED ON URP'S SPRITE_LIT_DEFAULT SHADER
* CUSTOM CODE ARE CONCENTRATED AT THE VARIABLE DECLARATIONS AND COMBINEDSHAPELIGHTFRAGMENT FUNCTION
* EVERYTHING ELSE IS CREATED BY UNITY

* CITATION:
* Title: Universal Render Pipeline/2D/Sprite-Lit-Default
* Author: Unity Technologies
* Date: June 29, 2021
* Code Version: 10.5.1
* Availability: https://docs.unity3d.com/Packages/com.unity.render-pipelines.universal@10.5/manual/index.html
****************************************************************************************************/

Shader "Alan Tao/Edge Manual"
{
    Properties
    {
        _MainTex("Diffuse", 2D) = "white" {}
        _MaskTex("Mask", 2D) = "white" {}
        _NormalMap("Normal Map", 2D) = "bump" {}
        _Metallic("Metallic", Range(0.1, 10)) = 1.0
        _Brightness("Brightness", Range(-1, 10)) = 1.0 // brightness modifier of the highlight
        _Border_Width("Border Width (texel)", Range(1, 20)) = 1.0 // width of the highlight border

        // Legacy properties. They're here so that materials using this shader can gracefully fallback to the legacy sprite shader.
        [HideInInspector] _Color("Tint", Color) = (1,1,1,1)
        [HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
        [HideInInspector] _AlphaTex("External Alpha", 2D) = "white" {}
        [HideInInspector] _EnableExternalAlpha("Enable External Alpha", Float) = 0
    }

        HLSLINCLUDE
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            ENDHLSL

            SubShader
        {
            Tags {"Queue" = "Transparent" "RenderType" = "Transparent" "RenderPipeline" = "UniversalPipeline" }

            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off

            Pass
            {
                Tags { "LightMode" = "Universal2D" }
                HLSLPROGRAM
                #pragma vertex CombinedShapeLightVertex
                #pragma fragment CombinedShapeLightFragment
                #pragma multi_compile USE_SHAPE_LIGHT_TYPE_0 __
                #pragma multi_compile USE_SHAPE_LIGHT_TYPE_1 __
                #pragma multi_compile USE_SHAPE_LIGHT_TYPE_2 __
                #pragma multi_compile USE_SHAPE_LIGHT_TYPE_3 __

                struct Attributes
                {
                    float3 positionOS   : POSITION;
                    float4 color        : COLOR;
                    float2  uv           : TEXCOORD0;
                    UNITY_VERTEX_INPUT_INSTANCE_ID
                };

                struct Varyings
                {
                    float4  positionCS  : SV_POSITION;
                    half4   color       : COLOR;
                    float2	uv          : TEXCOORD0;
                    half2	lightingUV  : TEXCOORD1;
                    UNITY_VERTEX_OUTPUT_STEREO
                };

                #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/LightingUtility.hlsl"

                TEXTURE2D(_MainTex);
                SAMPLER(sampler_MainTex);
                TEXTURE2D(_MaskTex);
                SAMPLER(sampler_MaskTex);
                TEXTURE2D(_NormalMap);
                SAMPLER(sampler_NormalMap);

                half _Metallic;
                half _Brightness;
                half _Border_Width;
                half4 _MainTex_ST;
                half4 _NormalMap_ST;
                half2 _MainTex_TexelSize;

                #if USE_SHAPE_LIGHT_TYPE_0
                SHAPE_LIGHT(0)
                #endif

                #if USE_SHAPE_LIGHT_TYPE_1
                SHAPE_LIGHT(1)
                #endif

                #if USE_SHAPE_LIGHT_TYPE_2
                SHAPE_LIGHT(2)
                #endif

                #if USE_SHAPE_LIGHT_TYPE_3
                SHAPE_LIGHT(3)
                #endif

                Varyings CombinedShapeLightVertex(Attributes v)
                {
                    Varyings o = (Varyings)0;
                    UNITY_SETUP_INSTANCE_ID(v);
                    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                    o.positionCS = TransformObjectToHClip(v.positionOS);
                    o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                    float4 clipVertex = o.positionCS / o.positionCS.w;
                    o.lightingUV = ComputeScreenPos(clipVertex).xy;
                    o.color = v.color;

                    return o;
                }

                #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/CombinedShapeLightShared.hlsl"

                half cull(half x) {
                    return clamp(abs(x - 0.5)-0.3, 0, 1);
                }


                half4 CombinedShapeLightFragment(Varyings i) : SV_Target
                {
                    half4 main = i.color * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                    half4 mask = SAMPLE_TEXTURE2D(_MaskTex, sampler_MaskTex, i.uv);

                    if (main.a == 0) { return half4(0, 0, 0, 0); }

                    // sample adjacent texels. if any of them are transparent, then the current texel is at border
                    half2 w_step = half2(_MainTex_TexelSize.x * _Border_Width, 0);
                    half2 h_step = half2(0, _MainTex_TexelSize.y * _Border_Width);

                    half left = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv - w_step).a;
                    half right = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv + w_step).a;
                    half top = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv - h_step).a;
                    half bottom = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv + h_step).a;

                    // trace out border
                    half alpha = 4 * main.a - top - right - bottom - left;
                    alpha = clamp(alpha, 0, 1);

                    // calculate raw light
                    half4 l_col = CombinedShapeLightShared(half4(1, 1, 1, 1), mask, i.lightingUV);
                    l_col *= (1 + _Brightness);

                    // add light to normal coloring
                    main += l_col;
                    main.a = alpha;

                    // adjust metallic (more metallic = more spot light; less metallic = more averaged out)
                    main = pow(main, _Metallic);
                    main = clamp(main, 0, 1);

                    return main;
                }
                ENDHLSL
            }
        }

            Fallback "Sprites/Default"
}
