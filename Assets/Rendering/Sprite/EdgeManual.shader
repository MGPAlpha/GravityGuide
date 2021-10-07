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
        _Shininess("Shininess", Range(0.1, 10)) = 1.0 // please refer to Phong
        _Brightness("Brightness", Range(0, 10)) = 1.0 // brightness modifier of the highlight
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
                half _Shininess;
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


                half4 CombinedShapeLightFragment(Varyings i) : SV_Target
                {
                    half4 main = i.color * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                    half4 mask = SAMPLE_TEXTURE2D(_MaskTex, sampler_MaskTex, i.uv);
                    //half4 main = i.color * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                    //half4 mask = SAMPLE_TEXTURE2D(_MaskTex, sampler_MaskTex, i.uv);

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
                    half4 luminosity = CombinedShapeLightShared(half4(1, 1, 1, main.a), mask, i.lightingUV);
                    luminosity.a = (luminosity.r + luminosity.g + luminosity.b) / 3.0 * alpha * _Brightness;
                    // adjust light
                    luminosity.a = clamp(luminosity.a, 0, 1);
                    luminosity.a = pow(luminosity.a, _Shininess);
                    luminosity.a = floor(luminosity.a * 4) / 4;
                    // apply light as coloring
                    luminosity.rgb = luminosity.rgb * luminosity.a;

                    half4 main_lighting = CombinedShapeLightShared(main, mask, i.lightingUV);

                    // overlay coloring to main sprite
                    main.rgb = main_lighting.rgb + luminosity.rgb;

                    return main;
                }
                ENDHLSL
            }

            Pass
            {
                Tags { "LightMode" = "NormalsRendering"}
                HLSLPROGRAM
                #pragma vertex NormalsRenderingVertex
                #pragma fragment NormalsRenderingFragment

                struct Attributes
                {
                    float3 positionOS   : POSITION;
                    float4 color		: COLOR;
                    float2 uv			: TEXCOORD0;
                    float4 tangent      : TANGENT;
                    UNITY_VERTEX_INPUT_INSTANCE_ID
                };

                struct Varyings
                {
                    float4  positionCS		: SV_POSITION;
                    half4   color			: COLOR;
                    float2	uv				: TEXCOORD0;
                    half3   normalWS		: TEXCOORD1;
                    half3   tangentWS		: TEXCOORD2;
                    half3   bitangentWS		: TEXCOORD3;
                    UNITY_VERTEX_OUTPUT_STEREO
                };

                TEXTURE2D(_MainTex);
                SAMPLER(sampler_MainTex);
                TEXTURE2D(_NormalMap);
                SAMPLER(sampler_NormalMap);
                half4 _NormalMap_ST;  // Is this the right way to do this?

                Varyings NormalsRenderingVertex(Attributes attributes)
                {
                    Varyings o = (Varyings)0;
                    UNITY_SETUP_INSTANCE_ID(attributes);
                    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                    o.positionCS = TransformObjectToHClip(attributes.positionOS);
                    o.uv = TRANSFORM_TEX(attributes.uv, _NormalMap);
                    o.uv = attributes.uv;
                    o.color = attributes.color;
                    o.normalWS = TransformObjectToWorldDir(float3(0, 0, -1));
                    o.tangentWS = TransformObjectToWorldDir(attributes.tangent.xyz);
                    o.bitangentWS = cross(o.normalWS, o.tangentWS) * attributes.tangent.w;
                    return o;
                }

                #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/NormalsRenderingShared.hlsl"

                half4 NormalsRenderingFragment(Varyings i) : SV_Target
                {
                    half4 mainTex = i.color * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                    half3 normalTS = UnpackNormal(SAMPLE_TEXTURE2D(_NormalMap, sampler_NormalMap, i.uv));
                    return NormalsRenderingShared(mainTex, normalTS, i.tangentWS.xyz, i.bitangentWS.xyz, i.normalWS.xyz);
                }
                ENDHLSL
            }
            Pass
            {
                Tags { "LightMode" = "UniversalForward" "Queue" = "Transparent" "RenderType" = "Transparent"}

                HLSLPROGRAM
                #pragma vertex UnlitVertex
                #pragma fragment UnlitFragment

                struct Attributes
                {
                    float3 positionOS   : POSITION;
                    float4 color		: COLOR;
                    float2 uv			: TEXCOORD0;
                    UNITY_VERTEX_INPUT_INSTANCE_ID
                };

                struct Varyings
                {
                    float4  positionCS		: SV_POSITION;
                    float4  color			: COLOR;
                    float2	uv				: TEXCOORD0;
                    UNITY_VERTEX_OUTPUT_STEREO
                };

                TEXTURE2D(_MainTex);
                SAMPLER(sampler_MainTex);
                float4 _MainTex_ST;

                Varyings UnlitVertex(Attributes attributes)
                {
                    Varyings o = (Varyings)0;
                    UNITY_SETUP_INSTANCE_ID(attributes);
                    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

                    o.positionCS = TransformObjectToHClip(attributes.positionOS);
                    o.uv = TRANSFORM_TEX(attributes.uv, _MainTex);
                    o.uv = attributes.uv;
                    o.color = attributes.color;
                    return o;
                }

                float4 UnlitFragment(Varyings i) : SV_Target
                {
                    float4 mainTex = i.color * SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);
                    return mainTex;
                }
                ENDHLSL
            }
        }

            Fallback "Sprites/Default"
}
