Shader "Custom/Upscale"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_LUT("Texture", 2D) = "white" {}
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

			static const uint scale = 4;
			sampler2D _MainTex;
			uniform float4 _MainTex_TexelSize;
			sampler2D _LUT;

			fixed4 frag(v2f i) : SV_Target
			{
				float x_offset = _MainTex_TexelSize.x;
				float y_offset = _MainTex_TexelSize.y;
				float width = _MainTex_TexelSize.z;
				float height = _MainTex_TexelSize.w;

				float top_left = tex2D(_MainTex, i.uv + float2(-x_offset, y_offset) / 2).r;
				float top_right = tex2D(_MainTex, i.uv + float2(x_offset, y_offset) / 2).r;
				float bottom_left = tex2D(_MainTex, i.uv + float2(-x_offset, -y_offset) / 2).r;
				float bottom_right = tex2D(_MainTex, i.uv + float2(x_offset, -y_offset) / 2).r;

				int index = (top_left * 1 + top_right * 2 + bottom_left * 4 + bottom_right * 8);

				float2 LUTuvs = frac((frac(i.uv * float2(width, height) + float2(1 / 2.0 , -1 / 2.0)) + float2(index % scale, index / scale)) / scale);
				float final = tex2D(_LUT, LUTuvs).r;

				return fixed4(final, final, final, 1);
			}
			ENDCG
		}
	}
}