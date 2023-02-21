Shader "Custom/GOLComputingShader"
{
    Properties
    {
        _MainTex("Cur Generation Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth  
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex); 
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;

            fixed4 frag(v2f i) : SV_Target
            {
                float sx = _MainTex_TexelSize.x;
                float sy = _MainTex_TexelSize.y;
                float2 uv = i.uv;
                int center = int(tex2D(_MainTex, uv).r);

                int count = int(tex2D(_MainTex, uv + float2(-sx, -sy)).r);
                count += int(tex2D(_MainTex, uv + float2(0, -sy)).r);
                count += int(tex2D(_MainTex, uv + float2(sx, -sy)).r);

                count += int(tex2D(_MainTex, uv + float2(-sx, 0)).r);
                count += int(tex2D(_MainTex, uv + float2(sx, 0)).r);

                count += int(tex2D(_MainTex, uv + float2(-sx, sy)).r);
                count += int(tex2D(_MainTex, uv + float2(0, sy)).r);
                count += int(tex2D(_MainTex, uv + float2(sx, sy)).r);

                if (count == 3)
                    return fixed4(1, 0, 0, 0);

                if (count == 2)
                    return fixed4(center, 0, 0, 0);

                return fixed4(0, 0, 0, 0);
            }
            ENDCG
        }
    }
}
