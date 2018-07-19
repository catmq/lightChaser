﻿Shader "MQShader/UnlitTransparent"
{
	Properties
	{
		_Color ("Color",Color) = (0,0,0,0)
	}
	SubShader
	{
		Tags {"RenderType" = "Transparent" "Queue" = "Transparent"}
		LOD 100

		Pass
		{
			BLEND SrcAlpha OneMinusSrcAlpha		
			CULL Back
			ZWrite OFF

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog

			#include "UNITYCG.cginc"

			float4 _Color;

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				UNITY_FOG_COORDS(1)
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			fixed4 frag(v2f i):SV_TARGET
			{
				fixed4 col = _Color;
				UNITY_APPLY_FOG(i.fogCoord, col);
				return col;

			}

			ENDCG
		}
	}
}
