// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/LightBeam"
{
	Properties {
		_Mask("Mask", 2D) = "white" {}
		_MaskStrength("Mask strength", Range(0,1)) = 1
		[Toggle] _UseWorldPos("Use world position as uv", Float) = 0
		_Ramp("Ramp", 2D) = "white" {}
		_Strength ("Depth blend strength", Float) = 1
		_Color("Color", Color) = (0,0,0)
		_MaxFogValue("Max fog thickness", Range(0,1)) = 1.0
		_ScrollSpeed("Scroll Speed", Vector) = (0,0,0,0)
	}

	Category {
		Tags { "Queue"="Transparent+99" "RenderType"="Transparent" "IgnoreProjector"="True" "PreviewType"="Plane"  "ForceNoShadowCasting"="True"}
		Blend SrcAlpha OneMinusSrcAlpha
	
		SubShader {
			LOD 200

			Pass {
				Cull Back
				ZWrite Off
			
				CGPROGRAM
				#pragma target 3.0
				#pragma vertex vert
				#pragma fragment frag
				#pragma fragmentoption ARB_precision_hint_fastest
				#pragma shader_feature _USEWORLDPOS_ON
				#include "UnityCG.cginc"

				struct appdata_t {
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float2 texcoord: TEXCOORD0;
					float3 normal : NORMAL;
				};

				struct v2f {
					float4 vertex : POSITION;
					float4 projPos : TEXCOORD0;
					float eyeDepth : TEXCOORD1;
					float2 uv : TEXCOORD2;
					float edge : TEXCOORD3;
				};

				uniform half _Strength;
				uniform half _MaxFogValue;

				half2 _ScrollSpeed;
				sampler2D _Mask;
				sampler2D _Ramp;
				float4 _Mask_ST;

				v2f vert (appdata_t v)
				{
					v2f o;
					float4 posWorld = mul(unity_ObjectToWorld, v.vertex);
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.projPos = ComputeScreenPos(o.vertex);

					#ifndef _USEWORLDPOS_ON
						o.uv = TRANSFORM_TEX(v.texcoord, _Mask) + fmod(_ScrollSpeed.xy * _Time.x, half2(1, 1));
					#else
						o.uv = TRANSFORM_TEX((posWorld.xy + posWorld.xz) * half2(0.5,1), _Mask) + fmod(_ScrollSpeed.xy * _Time.x, half2(1, 1));
					#endif
					
					COMPUTE_EYEDEPTH(o.eyeDepth);

					o.edge = dot(normalize(_WorldSpaceCameraPos - posWorld.xyz), UnityObjectToWorldNormal(v.normal));

					return o;
				}

				uniform sampler2D _CameraDepthTexture; //Depth Texture
				fixed4 _Color;
				half _MaskStrength;

				half4 frag( v2f i ) : COLOR
				{
					half4 col = _Color;
					col.a *= lerp(1, tex2D(_Mask, i.uv).r, _MaskStrength);

					clip(col.a - 0.01);

					float rawZ = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos));
					float sceneZ = LinearEyeDepth(rawZ);
					
					float partZ = i.eyeDepth;
 
					float fade = 1.0;
					if ( rawZ > 0.0 ) // Make sure the depth texture exists
						fade = saturate(_Strength * (sceneZ - partZ));

					col.a *= min(fade,_MaxFogValue) * tex2D(_Ramp, i.edge);
					return col;
				}
				ENDCG
			}
		}
	}
}
