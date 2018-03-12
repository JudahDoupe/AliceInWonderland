Shader "Custom/Displace" 
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (1, 1, 1, 0)
	}

	SubShader
	{
		ZWrite On
		Tags{ "RenderType" = "Transparent" "Queue" = "Geometry+1" }
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha

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
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			half4 _Color;

			v2f vert(appdata v)
			{
				float2 newUV = TRANSFORM_TEX(v.uv, _MainTex);
				float4 offset = tex2Dlod(_MainTex, float4(newUV,0,0));
				offset = offset - 0.5;
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex + offset);
				o.uv = v.uv;
				return o;
			}

			float4 frag(v2f i) : SV_Target
			{
				return float4(_Color.r * i.uv.r, _Color.g * i.uv.g,_Color.b * i.uv.r,1);
			}
			ENDCG
		}
	}
}