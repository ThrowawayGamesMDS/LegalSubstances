Shader "Custom/3dprint"
{
	Properties{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB), Alpha (A)", 2D) = "white" {}
		_MainTex2("Albedo (RGB), Alpha (A)", 2D) = "white" {}
		_Glossiness("Smoothness", Range(0,1)) = 0.5
		_Metallic("Metallic", Range(0,1)) = 0.0
		_ConstructY("ConstructionFinishedness", Range(0,20)) = 0.0
	}
		SubShader{
			Tags {"Queue" = "Transparent" "RenderType" = "Transparent" }
			LOD 200

			CGPROGRAM
			// Physically based Standard lighting model, and enable shadows on all light types
			//#pragma surface surf Standard fullforwardshadows
			#pragma surface surf Custom fullforwardshadows alpha:fade
			#pragma surface surf Unlit fullforwardshadows alpha:fade
			#include "UnityPBSLighting.cginc"
			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0
			int building;
		float4 _MainTex_ST;
			float _ConstructY;
			fixed4 _ConstructColor;

			//        inline half4 LightingUnlit (SurfaceOutputStandard s, half3 lightDir, UnityGI gi)        {            return _ConstructColor;        }
			//        inline void LightingUnlit_GI(SurfaceOutputStandard s, UnityGIInput data, inout UnityGI gi)        {            LightingStandard_GI(s, data, gi);                }
					inline half4 LightingCustom(SurfaceOutputStandard s, half3 lightDir, UnityGI gi)
					{
						if (!building)
							return LightingStandard(s, lightDir, gi); // Unity5 PBR
						return LightingStandard(s, lightDir, gi); // Unity5 PBR
						//return _ConstructColor; // Unlit
					}
					inline void LightingCustom_GI(SurfaceOutputStandard s, UnityGIInput data, inout UnityGI gi)
					{
						LightingStandard_GI(s, data, gi);
					}
					sampler2D _MainTex;
					sampler2D _MainTex2;

					struct Input {
						float2 uv_MainTex;
						float2 uv_MainTex2;
						float3 weight : TEXCOORD0;
						float3 worldPos;
					};

					half _Glossiness;
					half _Metallic;
					fixed4 _Color;

					// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
					// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
					// #pragma instancing_options assumeuniformscaling
					UNITY_INSTANCING_BUFFER_START(Props)
						// put more per-instance properties here
					UNITY_INSTANCING_BUFFER_END(Props)
					void surf(Input IN, inout SurfaceOutputStandard o) {
					 if (IN.worldPos.y < _ConstructY)
					 {
					 fixed4 c = tex2D(_MainTex2, IN.uv_MainTex2) * _Color;
					o.Albedo = c.rgb;
					 o.Alpha = c.a;
					 building = 0;
					 }
					 else
					 {
					float3 oPos = mul(unity_WorldToObject, fixed4(IN.worldPos, 1.0)).xyz;
					fixed2 uvx = (oPos.yz - _MainTex_ST.zw) * _MainTex_ST.xy;
					fixed2 uvy = (oPos.xz - _MainTex_ST.zw) * _MainTex_ST.xy;
					fixed2 uvz = (oPos.xy - _MainTex_ST.zw) * _MainTex_ST.xy;
					fixed4 cz = tex2D(_MainTex, uvx) * IN.weight.xxxx;
					fixed4 cy = tex2D(_MainTex, uvy) * IN.weight.yyyy;
					fixed4 cx = tex2D(_MainTex, uvz) * IN.weight.zzzz;
					fixed4 col = (cz + cy + cx);
					o.Albedo = col.rgb * _Color.rgb;
					o.Alpha = col.a * _Color.a;



					// fixed4 ca = tex2D(_MainTex2, IN.uv_MainTex2) * _Color;
					//o.Specular = _Shininess;
					//o.Albedo = ca.rgb;
					// o.Alpha  = ca.a;
					// o.Albedo = _ConstructColor.rgb;
					// o.Alpha  = _ConstructColor.a;
					 building = 1;
					 }
					 o.Metallic = _Metallic;
					 o.Smoothness = _Glossiness;
					}
					ENDCG
		}
			FallBack "Diffuse"
}

