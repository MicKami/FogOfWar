Shader "Custom/Blur"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
	SubShader
	{
		Cull Off
		ZWrite Off
		ZTest Always

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
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;
			float4  _MainTex_TexelSize;

			static const float offset[3] = { 0.0, 1.3846153846, 3.2307692308 };
			static const float weight[3] = { 0.2270270270, 0.3162162162, 0.0702702703 };

			fixed4 frag(v2f v) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, v.uv) * weight[0];
				for (int i = 1; i < 3; i++)
				{
					col += tex2D(_MainTex, v.uv + float2(offset[i] / _MainTex_TexelSize.z, 0)) * weight[i];
					col += tex2D(_MainTex ,v.uv - float2(offset[i] / _MainTex_TexelSize.z, 0)) * weight[i];
				}
				return col;
			}
			ENDCG
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
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;
			float4  _MainTex_TexelSize;

			static const float offset[3] = { 0.0, 1.3846153846, 3.2307692308 };
			static const float weight[3] = { 0.2270270270, 0.3162162162, 0.0702702703 };

			fixed4 frag(v2f v) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, v.uv) * weight[0];
				for (int i = 1; i < 3; i++)
				{
					col += tex2D(_MainTex, v.uv + float2(0, offset[i] / _MainTex_TexelSize.z)) * weight[i];
					col += tex2D(_MainTex, v.uv - float2(0, offset[i] / _MainTex_TexelSize.z)) * weight[i];
				}
				return col;
			}
			ENDCG
		}
	}
}
