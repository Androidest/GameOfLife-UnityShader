Shader "Custom/CurGenerationShader"
{
    Properties
    {
        _MainTex("Cur Generation Texture", 2D) = "white" {}
        _CellColor("Cell Color", Color) = (0,0,0,0)
        _GridColor("Grid Color", Color) = (0,0,0,0)
        
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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            fixed4 _CellColor;
            fixed4 _GridColor;

            fixed4 frag(v2f i) : SV_Target
            {
                float2 pos = i.uv / _MainTex_TexelSize.xy;
                float2 mod = fmod(pos, 1);
                if (mod.x < 0.1 || mod.y < 0.1)
                {
                    return _GridColor;
                }
                
                fixed4 col = tex2D(_MainTex, i.uv);
                return col.r * _CellColor;
            }
            ENDCG
        }
    }
}
