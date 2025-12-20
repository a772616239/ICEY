Shader "Xffect/displacement/additive"
{
  Properties
  {
    _TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
    _MainTex ("Main Texture", 2D) = "white" {}
    _DispMap ("Displacement Map (RG)", 2D) = "white" {}
    _MaskTex ("Mask(R)", 2D) = "white" {}
    _DispScrollSpeedX ("Map Scroll Speed X", float) = 0
    _DispScrollSpeedY ("Map Scroll Speed Y", float) = 0
    _DispX ("Displacement Strength X", float) = 0
    _DispY ("Displacement Strength Y", float) = 0.2
  }
  SubShader
  {
    Tags
    { 
      "IGNOREPROJECTOR" = "true"
      "QUEUE" = "Transparent"
      "RenderType" = "Transparent"
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      ZClip Off
      ZWrite Off
      Cull Off
      Blend SrcAlpha One
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      //uniform float4 _Time;
      uniform sampler2D _MainTex;
      uniform sampler2D _DispMap;
      uniform sampler2D _MaskTex;
      uniform float _DispScrollSpeedX;
      uniform float _DispScrollSpeedY;
      uniform float _DispX;
      uniform float _DispY;
      uniform float4 _TintColor;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 color :COLOR;
          float4 texcoord :TEXCOORD0;
          float4 texcoord1 :TEXCOORD1;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_COLOR :COLOR;
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float2 xlv_TEXCOORD1 :TEXCOORD1;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 xlv_COLOR :COLOR;
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float2 xlv_TEXCOORD1 :TEXCOORD1;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float4 tmpvar_1;
          tmpvar_1.w = 1;
          tmpvar_1.xyz = in_v.vertex.xyz;
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_1));
          out_v.xlv_COLOR = in_v.color;
          out_v.xlv_TEXCOORD0 = in_v.texcoord.xy;
          out_v.xlv_TEXCOORD1 = in_v.texcoord1.xy;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float2 uvoft_1;
          float4 dispColor_2;
          float2 mapoft_3;
          float2 tmpvar_4;
          tmpvar_4.x = (_Time.y * _DispScrollSpeedX);
          tmpvar_4.y = (_Time.y * _DispScrollSpeedY);
          mapoft_3 = tmpvar_4;
          float4 tmpvar_5;
          float2 P_6;
          P_6 = (in_f.xlv_TEXCOORD0 + mapoft_3);
          tmpvar_5 = tex2D(_DispMap, P_6);
          dispColor_2 = tmpvar_5;
          uvoft_1 = in_f.xlv_TEXCOORD0;
          uvoft_1.x = (uvoft_1.x + ((dispColor_2.x * _DispX) * in_f.xlv_TEXCOORD1.x));
          uvoft_1.y = (uvoft_1.y + ((dispColor_2.y * _DispY) * in_f.xlv_TEXCOORD1.x));
          float4 tmpvar_7;
          tmpvar_7 = ((2 * in_f.xlv_COLOR) * ((_TintColor * tex2D(_MainTex, uvoft_1)) * tex2D(_MaskTex, in_f.xlv_TEXCOORD0).x));
          out_f.color = tmpvar_7;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
