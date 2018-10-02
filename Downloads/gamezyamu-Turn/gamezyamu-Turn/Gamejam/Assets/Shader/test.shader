Shader "Custom/test" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_MainTex2("Albedo2 (RGB)", 2D) = "white" {}
		_MainTex3("Albedo3 (RGB)", 2D) = "white"{}
		_MainTex4("Albedo3 (RGB)", 2D) = "white"{}
		_MainTex5("Albedo3 (RGB)", 2D) = "white"{}
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		_Metallic ("Metallic", Range(0,1)) = 0.0
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
#pragma surface surf Standard fullforwardshadows vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 4.0

		sampler2D _MainTex;
		sampler2D _MainTex2;
		sampler2D _MainTex3;
		sampler2D _MainTex4;
		sampler2D _MainTex5;

		struct Input {
			float2 uv_MainTex;
			fixed4 vertexColor;
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

		void vert(inout appdata_full v, out Input o) {
			//UNITY_INITIALIZE_OUTPUT(Input,o);
			
			o.uv_MainTex = v.texcoord;
			float4 c = tex2Dlod(_MainTex4, float4(v.texcoord.xy,0,1));
			v.vertex.y +=c.r*2;
			v.vertex.y -= c.b *2;
			o.vertexColor = v.color;
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {

			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) *tex2D(_MainTex4, IN.uv_MainTex).r;
			fixed4 c2= tex2D(_MainTex2, IN.uv_MainTex) *tex2D(_MainTex4, IN.uv_MainTex).g;
			fixed4 c3 = tex2D(_MainTex3, IN.uv_MainTex) *tex2D(_MainTex4, IN.uv_MainTex).b;

			o.Albedo = c.rgb+c2.rgb+c3.rgb;



		}
		ENDCG
	}
	FallBack "Diffuse"
}
