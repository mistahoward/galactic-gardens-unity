Shader"Custom/TerrainShader" {
    Properties {
        _Splat ("Splat Map", 2D) = "white" {}
        _Texture1 ("Texture1", 2D) = "white" {}
        _Texture2 ("Texture2", 2D) = "white" {}
        _Texture3 ("Texture3", 2D) = "white" {}
        _Texture4 ("Texture4", 2D) = "white" {}
    }
    SubShader {
        Pass {
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

sampler2D _Splat;
sampler2D _Texture1;
sampler2D _Texture2;
sampler2D _Texture3;
sampler2D _Texture4;

v2f vert(appdata v)
{
    v2f o;
    o.vertex = UnityObjectToClipPos(v.vertex);
    o.uv = v.uv;
    return o;
}

fixed4 frag(v2f i) : SV_Target
{
                // sample the splat map
    float4 splat = tex2D(_Splat, i.uv);

                // sample each of the textures
    float4 tex1 = tex2D(_Texture1, i.uv);
    float4 tex2 = tex2D(_Texture2, i.uv);
    float4 tex3 = tex2D(_Texture3, i.uv);
    float4 tex4 = tex2D(_Texture4, i.uv);

                // return a blend of the textures based on the splat map
    return tex1 * splat.r + tex2 * splat.g + tex3 * splat.b + tex4 * splat.a;
}
            ENDCG
        }
    }
}
