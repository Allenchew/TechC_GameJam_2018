// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Environment/Grass3"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_NormalTex ("Normal", 2D) = "bump" {}
	}
	SubShader
	{
		Tags { "RenderType" = "Transparent" "Queue" = "Transparent" }
		LOD 100
		Cull Off

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
				float4 tangent : TANGENT;
				float2 uv : TEXCOORD0;
				float3 normal : NORMAL;

			};

			struct v2f
			{
				float3 worldPos : TEXCOORD0;
				float2 uv : TEXCOORD4;
				float4 pos : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _NormalTex;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				o.uv = v.uv;
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				return col;
			}
			ENDCG
		}
	}
}
