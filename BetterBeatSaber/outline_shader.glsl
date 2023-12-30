Shader "BetterBeatSaber/OutlineFill" {
    Properties {
        
        [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest("ZTest", Float) = 0
        
        // My Stuff
        _ColorMask("Color Mask", Float) = 15
        
        // Visibility: 0 = all; 1 = vr; 2 = desktop
        _Visibility("Visibility", Range(0, 2)) = 0
        
        _OutlineColor("Outline Color", Color) = (1, 1, 1, 1)
        _OutlineColor1("Outline Color 1", Color) = (1, 1, 1, 0)
        _OutlineWidth("Outline Width", Range(0, 50)) = 2
        
    }
    SubShader {
        Tags {
            "Queue" = "Transparent+110"
            "RenderType" = "Transparent"
            "DisableBatching" = "True"
        }
        Pass {
            
            Name "Fill"
            Cull Off
            //ZTest [_ZTest]
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            ColorMask [_ColorMask]

            Stencil {
                Ref 1
                Comp NotEqual
            }

            CGPROGRAM
            
            #include "UnityCG.cginc"
            #include "UnityInstancing.cginc"
          
            #pragma vertex vert
            #pragma fragment frag

            struct Input {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float3 smoothNormal : TEXCOORD3;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Output {
                float4 position : SV_POSITION;
                fixed4 color : COLOR;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            uniform int _Visibility;

            uniform fixed4 _OutlineColor, _OutlineColor1;
            uniform float _OutlineWidth;

            Output vert(Input input) {
            
                Output output;

                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_OUTPUT(Output, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
            
                float3 normal = any(input.smoothNormal) ? input.smoothNormal : input.normal;
                float3 viewPosition = UnityObjectToViewPos(input.vertex);
                float3 viewNormal = normalize(mul((float3x3)UNITY_MATRIX_IT_MV, normal));

                output.position = UnityViewToClipPos(viewPosition + viewNormal * -viewPosition.z * _OutlineWidth / 1000.0);

                output.color = lerp(_OutlineColor, _OutlineColor1, output.position.x);
                output.color[3] = (_Visibility == 0 || (_Visibility == 1 && unity_CameraProjection[0][2] != 0) || (_Visibility == 2 && unity_CameraProjection[0][2] == 0)) ? 1 : 0;

                return output;
            }

            fixed4 frag(Output input) : SV_Target {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
                return input.color;
            }
            
            ENDCG
            
        }
    }
}