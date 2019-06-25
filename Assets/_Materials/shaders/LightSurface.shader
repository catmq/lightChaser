Shader "MQShader/LightSurface"
{
	Properties
	{
        _Color ("EmissiveColor",Color) = (0,0,0,0)
        _Fresnel("Edge Fade", Range(0.01,5)) = 0
        _Halo("Halo Size",Range(0,0.8))= 0
        _HaloInner("Inner Halo Size",Range(0.01,0.8)) = 0
        _HaloPow("Halo Power",Range(2,10)) = 0
	}
	SubShader
	{
		Tags {"RenderType" = "Transparent" "Queue" = "Transparent"}
		LOD 100

		Pass
		{
			BLEND SrcAlpha OneMinusSrcAlpha		
			CULL Front
			ZWrite OFF

			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_fog

			#include "UNITYCG.cginc"

			float4 _Color;
            float _Fresnel;
            float _Halo;
            float _HaloInner;
            float _HaloPow;

			struct appdata
			{
				float4 vertex : POSITION;
                float3 normal: NORMAL;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
                float4 posWorld : TEXCOORD0;
                float3 normal: TEXCOORD1;
				UNITY_FOG_COORDS(1)
			};

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
                o.posWorld = mul(unity_ObjectToWorld,v.vertex);
                o.normal = UnityObjectToWorldNormal(v.normal);
				UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			fixed4 frag(v2f i):SV_TARGET
			{
				i.normal = normalize(-i.normal);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float fresnelVal = max(0,dot(i.normal,viewDirection));
                float haloVal = 0;
                if (_Halo > 0){
                    haloVal = clamp(1 - max(0,_Halo - fresnelVal)/_Halo,0,1);
                }
                fresnelVal = clamp(max(0, fresnelVal - _Halo) / (1-_Halo),0,1);
                haloVal = clamp(haloVal - (fresnelVal > 0),0,1);
                haloVal = clamp(haloVal + max(0, _HaloInner - fresnelVal) / _HaloInner - (fresnelVal == 0),0,1);
                fixed4 col = clamp(fixed4(_Color.rgb,_Color.a*(pow(fresnelVal,_Fresnel)+pow(haloVal,_HaloPow))),0,1);
				//fixed4 col = fixed4(haloVal,haloVal,haloVal,1);
                UNITY_APPLY_FOG(i.fogCoord, col);
				return col;

			}

			ENDCG
		}
	}
}
