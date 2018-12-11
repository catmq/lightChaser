Shader "MQShader/ambientSky" {
    Properties {
        _SkyColor ("Sky Color", Color) = (0,0,0,1)
        _HorizonColor ("Horizon Color", Color) = (0.1,0.1,0.1,1)
        _horizonThickness_1 ("horizonThickness_1", Range(0.01, 1)) = 0.05
        _horizonThickness_2 ("horizonThickness_2", Range(0.01, 1)) = 0.01
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Background"
            "RenderType"="Opaque"
            "PreviewType"="Skybox"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            //#pragma only_renderers d3d9 d3d11 glcore gles 
            //#pragma target 3.0
            uniform float4 _SkyColor;
            uniform float4 _HorizonColor;
            uniform float _horizonThickness_1;
            uniform float _horizonThickness_2;
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
////// Lighting:
////// Emissive:
                float skyValue = max(0,dot(viewDirection,float3(0,-1,0)));
                float groundValue = max(0,dot(viewDirection,float3(0,1,0)));
                float3 emissive = (_SkyColor.rgb+(((abs((pow((1.0 - skyValue),(1.0/_horizonThickness_1))-((1.0 - ceil(skyValue))*1.0)))+abs((pow((1.0 - groundValue),(1.0/_horizonThickness_2))-((1.0 - ceil(groundValue))*1.0))))*_HorizonColor.rgb)*_HorizonColor.a));
                float3 finalColor = emissive;
                return fixed4(finalColor,1);
            }
            ENDCG
        }
    }
}
