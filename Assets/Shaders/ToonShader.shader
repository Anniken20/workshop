Shader "Toon/NewUnlitShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "image" {}
        _Albedo("Albedo", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
               // float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldNormal: TEXTCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Albedo;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float cosineAngle = dot(normalize(i.worldNormal), normalize(_WorldSpaceLightPos0.xyz));
                cosineAngle = max(cosineAngle, 0.0);
               // return fixed4(cosineAngle,cosineAngle,cosineAngle,1.0);
               return _Albedo * cosineAngle;
            }
            ENDCG
        }
    }
}
