Shader "MQShader/ambientSky" {
    Properties {
        _skyColor ("Sky Color", Color) = (0,0,0,1)
        _horizonColor ("Horizon Color", Color) = (0.1,0.1,0.1,1)
        _horizonThickness_1 ("Horizon Sky Thickness", Range(0, 1.0)) = 0.05
        _horizonThickness_2 ("Horizon Ground Thickness", Range(0, 1.0)) = 0.01
        _horizonNoise("Horizon Noise",Range(0,1.0)) = 0
        [KeywordEnum(Off,On)] _variance("Horizon Variance", Float) = 0
        _varianceFrequency("Variance Frequency",Range(0.01,1)) = 1
        _varianceStrength("Variance Strength",Range(0,1)) = 0
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
            #pragma shader_feature _VARIANCE_OFF _VARIANCE_ON
            uniform float4 _skyColor;
            uniform float4 _horizonColor;
            uniform float _horizonThickness_1;
            uniform float _horizonThickness_2;
            uniform float _horizonNoise;
            uniform float _varianceFrequency;
            uniform float _varianceStrength;
            
            static const int NUM = 32;
            static const int MAXBLOCKNUM = 256;
            static const float randV[NUM] = {
                0.481398, 0.061536, 0.876773, 0.651351, 0.024812, 0.041826, 0.578250, 0.050811, 0.229682, 0.183251, 0.629462, 0.770048, 0.564906, 0.446879, 0.376282, 0.362360, 0.479490, 0.938886, 0.964369, 0.718695, 0.294638, 0.081306, 0.716721, 0.378063, 0.104969, 0.483435, 0.826935, 0.320512, 0.325629, 0.713444, 0.241332, 0.647386
            };
            static const float PI = 3.14159265f;
            
            struct VertexInput {
                float4 vertex : POSITION;
            };
            
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float4 posWorld : TEXCOORD0;
            };
            
            float fade(float t){
                return t*t*t*(t*(t*6-15)+10);
            }
            
            float random (float2 p){
                return frac(sin(dot(p.xy,float2(21.4927,97.3724)))*1742.0839);
            }
            
            int mod (int a, int b){
                return a - floor(a/b) * b;
            }
            
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            
            float4 frag(VertexOutput i) : COLOR {
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);            
////// Emissive:
                float skyValue = max(0,dot(viewDirection,float3(0,-1,0)));
                float groundValue = max(0,dot(viewDirection,float3(0,1,0)));
                //float xValue = i.posWorld.x;
                //float3 emissive = (_SkyColor.rgb + xValue * _HorizonColor.rgb *_HorizonColor.a);
                //float noise = sin( 10 * atan2(i.posWorld.x,i.posWorld.z)) * _horizonNoise;
                
                float vari = 0;
                float a = 0;
                float b = 0;
                #if _VARIANCE_ON
                    float angle = clamp((atan2(i.posWorld.z,i.posWorld.x) / PI +1) / 2,0,1);
                    int blockNum = _varianceFrequency * MAXBLOCKNUM;
                    int startIndex = floor(random(float2(_varianceFrequency * (float)MAXBLOCKNUM - blockNum,0)) * NUM);
                    int index = floor(angle * blockNum);
                    float u = angle * blockNum - index;
                    u = fade(u);
                    a = randV[mod(index+ startIndex,NUM)];
                    b = index + 1 == blockNum? randV[startIndex]:randV[mod(index+ startIndex +1,NUM)];
                    vari = (1-u) * a + u* b;
                    //return fixed4(vari,vari,vari,1);
                    //return((float)index/blockNum,(float)index/blockNum,(float)index/blockNum,1);
                #endif
                float noise = (random(float2(i.posWorld.x,i.posWorld.z)) - 0.5) * 2 * _horizonNoise;
                float skyHorizon = (1 + noise) * _horizonThickness_1;
                float groundHorizon = (1+ noise) * _horizonThickness_2;
                skyHorizon = skyHorizon * (1- ((1-vari) * _varianceStrength));
                groundHorizon = groundHorizon * (1- ((1-vari) * _varianceStrength));
                
                float horizonValue = abs(pow(1.0 - skyValue-ceil(groundValue),1.0/skyHorizon))+abs(pow(1.0 - groundValue - (1.0 - ceil(groundValue)),1.0/groundHorizon));
                float3 emissive = (1-horizonValue) * _skyColor.rgb + horizonValue * _horizonColor.rgb * _horizonColor.a;

                //return fixed4((float)mod(index,blockNum)/blockNum,(float)mod(index,blockNum)/blockNum,(float)mod(index,blockNum)/blockNum,1);
                return fixed4(emissive,1);
            }
            ENDCG
        }
    }
}
