Shader "Custom/FogOfWar"
{
    Properties
    {
        _Persistent_FoW("Texture", 2D) = "white" {}
        _Dynamic_FoW("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
		Blend SrcAlpha OneMinusSrcAlpha
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

			sampler2D _Persistent_FoW;
			sampler2D _Dynamic_FoW;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			float _EdgeMin;
			float _EdgeMax;
			float _Opacity;
			float _UseStatic;
			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col1 = tex2D(_Dynamic_FoW, i.uv);
				fixed4 col2 = max(tex2D(_Persistent_FoW, i.uv), _UseStatic);
				float result = max(pow(_Opacity, 0.25) * smoothstep(_EdgeMin, _EdgeMax, 1 - col1), smoothstep(_EdgeMin, _EdgeMax, 1 - col2));
				return fixed4(0, 0, 0, result);
			}
			ENDCG
		}
    }
}
