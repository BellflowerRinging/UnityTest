Shader "Master/GradientColor"
{
	Properties
	{
		_MainTex ("Texture",2D) = "white" {}

		_StartColor("StartColor",Color) = (0,0,0,0)
		_EndColor("EndColor",Color) = (0,0,0,0)
		_Offset("Min Offset",Range(-0.5,0.5))=1
		_AllhaMax("AllhaMax",Range(0,1))=1
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100
		Cull off
		Pass
		{
			Blend SrcAlpha OneMinusSrcAlpha

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed4 _StartColor;
			fixed4 _EndColor;
			float _Offset;
			float _AllhaMax;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed locUv=distance(i.uv.y,0.5);
				fixed locUvFcolor=locUv+_Offset;

				col.rgb=lerp(_StartColor.rgb,_EndColor.rgb,locUvFcolor/0.5);
				col.a=clamp(lerp(1,0,locUv/_AllhaMax),0,1);
				// apply fog
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;
			}
			ENDCG
		}


	}
}
