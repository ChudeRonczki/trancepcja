Shader "Custom/Fire Additive"
{
	Properties
	{
		_Color("Color", Color) = (0,0,0)
		[Gamma] _MainTex("Texture", 2D) = "white" {}
		_Strength ("Strength", Range(0.01,10.0)) = 1.0
		
		[NoScaleOffset]_Ramp("Ramp", 2D) = "white" {}

		[Space]
		_Mask("Mask (R)", 2D) = "black" {}
		_MaskStrength ("Strength", Range(0.0,1.0)) = 1.0
		_Mask2("Mask2 (R)", 2D) = "black" {}
		_Mask2Strength ("Strength", Range(0.0,1.0)) = 1.0
		
		[Space]
		_ScrollSpeed("Scroll Speed (XY - Mask, ZW - Mask 2)", Vector) = (0,0,0,0)
		_Wave("Wave Mask - (XY - speed, ZW - amount)", Vector) = (0,0,0,0)
		_Wave2("Wave Mask 2 - (XY - speed, ZW - amount)", Vector) = (0,0,0,0)
		_WaveUVScale("Wave scale - (XY - Mask, ZW - Mask 2)", Vector) = (0,0,0,0)
		
		[Space]
		[Toggle] _WaveMesh("Use waving", Float) = 0
		_WaveStrength("Waving mesh Strength", Float) = 1.0
		_WaveSpeed("Waving mesh Speed", Float) = 1.0
		_WaveScale("Waving mesh Scale", Float) = 1.0
		
		[Space]
		[Toggle] _UseSoft("Use soft fading", Float) = 0
		_InvFade ("Soft Particles Factor", Range(0.01,10.0)) = 1.0
		_Offset ("Offset", Float) = 0
	}

	SubShader
	{
		Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"  "PreviewType"="Plane"}
		LOD 100
		//Offset [_Offset], [_Offset]
			
		ZWrite Off
		Blend One One
		//ZTest Off

		Pass
		{
			CGPROGRAM
				#pragma target 3.0
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"
				#include "Waving.cginc"
				
				#pragma shader_feature _WAVEMESH_ON
				#pragma shader_feature _USESOFT_ON

				struct vIn
				{
					float4 vertex : POSITION;
					fixed4 color : COLOR;
					float2 uv : TEXCOORD0;
				};


				struct v2f
				{
					float4 pos : SV_POSITION;
					float4 uv : TEXCOORD0;
					float4 uv2 : TEXCOORD1;
					float4 projPos : TEXCOORD2;
					float4 color : TEXCOORD3;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;

				sampler2D _Mask;
				float4 _Mask_ST;
				sampler2D _Mask2;
				float4 _Mask2_ST;
				float4 _Wave;
				float4 _Wave2;
				float4 _ScrollSpeed;
				float4 _WaveUVScale;
				fixed4 _Color;
				half _Offset;

				static const float PI = 3.1415;
				static const float PI2 = PI * 2;

				v2f vert(vIn v)
				{
					v2f o;
					
					#ifdef _WAVEMESH_ON
						v.vertex.xyz += mul(unity_WorldToObject, WaveVertex(mul(unity_ObjectToWorld, v.vertex), v.color.r));
					#endif

					o.pos = UnityObjectToClipPos(v.vertex);
					//o.pos.z = UnityViewToClipPos(UnityObjectToViewPos( v.vertex ) + half4(0, 0, _Offset, 0)).z;// ;
					//float w = 1 / o.pos.w;
					//o.pos.w -= _Offset;// / o.pos.w;
					//o.pos.w = max(o.pos.w, 0.1);
					//w *= o.pos.w;
					//w = max(w, 0.1);
					//o.pos.xy *= w;

					o.pos.z *= o.pos.w / (max(o.pos.w - _Offset, 0.01));
					o.pos.z = saturate(o.pos.z);

					o.uv.xy = TRANSFORM_TEX(v.uv, _MainTex);
					
					float4 posWorld = mul(unity_ObjectToWorld, v.vertex);
					float2 uv = (posWorld.xy + posWorld.xz) * half2(0.5,1);

					o.uv2.xy = TRANSFORM_TEX(uv, _Mask) + sin(fmod(_Wave.xy * _Time.x + uv * _WaveUVScale.xy, PI2)) * _Wave.zw +
								fmod(_ScrollSpeed.xy * _Time.x, 1);

					o.uv2.zw = TRANSFORM_TEX(uv, _Mask2) + sin(fmod(_Wave2.xy * _Time.x + uv * _WaveUVScale.zw, PI2)) * _Wave2.zw +
								fmod(_ScrollSpeed.zw * _Time.x, 1);
		
					o.projPos = ComputeScreenPos (o.pos);

					#ifdef _USESOFT_ON
						COMPUTE_EYEDEPTH(o.projPos.z);
						o.projPos.z -= _Offset;
					#endif
					
					#ifdef _WAVEMESH_ON
						o.color = _Color;
						o.color.a *= v.color.a;
					#else
						o.color = _Color;
					#endif

					return o;
				}
				
				sampler2D_float _CameraDepthTexture;
				float _InvFade;

				half _MaskStrength;
				half _Mask2Strength;
				
				sampler2D _Ramp;
				half _Strength;

				fixed4 frag(v2f i) : SV_Target
				{
					float value = tex2D(_MainTex, i.uv.xy).r * _Strength;

					value -= (1 - tex2D(_Mask, i.uv2.xy).r) * _MaskStrength + (1 - tex2D(_Mask2, i.uv2.zw).r) * _Mask2Strength;
					
					#ifdef _USESOFT_ON
						float sceneZ = LinearEyeDepth (SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
						float partZ = i.projPos.z;
						value *= saturate (_InvFade * (sceneZ-partZ));
					#endif

					float4 col = tex2D(_Ramp, saturate(value * i.color.a));

					col.rgb *= i.color.rgb;

					return col;
				}
			
			ENDCG
		}
	}
}
