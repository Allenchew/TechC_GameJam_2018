// Upgrade NOTE: replaced '_World2Object' with 'unity_WorldToObject'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Grass" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Normal("NormalMap",2D) = "bump"{}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_WindDirection ("WindDirection",Vector) = (0,0,0,0)
	}
	SubShader {
		Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
		LOD 200
		Cull off

		CGPROGRAM
		
		#pragma surface surf Standard alpha:fade vertex:vert

		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _Normal;

		struct Input {
			float2 uv_MainTex;
			float2 uv_BumpMap;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		float4 _WindDirection;

		void vert(inout appdata_full v)
		{
			float3 pos = v.vertex.xyz;
			float effect = (sin(_Time.y * 1.0) * 0.05 * (v.texcoord.y * v.texcoord.y));
			float3 wPos = mul(pos, unity_WorldToObject);
			float3 fPos = wPos + (_WindDirection.xyz * effect);
			float3 direction = fPos - wPos;
			direction.y = 0;
			pos = pos + (direction * effect);
			v.vertex.xyz = pos;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) 
		{	
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
			o.Normal = UnpackNormal(tex2D(_Normal, IN.uv_BumpMap));
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
