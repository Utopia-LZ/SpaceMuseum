Shader "Custom/EarthCloud" {
    Properties {
        _CloudTex("Cloud Texture", 2D) = "white" {}
        _CloudSpeed("Speed", Range(0,5)) = 1.0
        _CloudAlpha("Transparency", Range(0,1)) = 0.7
    }
    SubShader {
        Tags { "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off // 双面渲染[7](@ref)
        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _CloudTex;
            float _CloudSpeed, _CloudAlpha;

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (appdata v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                // UV偏移模拟运动
                float2 cloudUV = i.uv + float2(_Time.y * _CloudSpeed * 0.1, 0);
                fixed4 cloud = tex2D(_CloudTex, cloudUV);
                cloud.a *= _CloudAlpha; // 控制透明度
                return cloud;
            }
            ENDCG
        }
    }
}