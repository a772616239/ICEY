Shader "AVProVideo/UI/Transparent Packed"
{
  Properties
  {
    [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
    _Color ("Tint", Color) = (1,1,1,1)
    _StencilComp ("Stencil Comparison", float) = 8
    _Stencil ("Stencil ID", float) = 0
    _StencilOp ("Stencil Operation", float) = 0
    _StencilWriteMask ("Stencil Write Mask", float) = 255
    _StencilReadMask ("Stencil Read Mask", float) = 255
    _ColorMask ("Color Mask", float) = 15
    _VertScale ("Vertical Scale", Range(-1, 1)) = 1
    [KeywordEnum(None, Top_Bottom, Left_Right)] AlphaPack ("Alpha Pack", float) = 0
    [Toggle(APPLY_GAMMA)] _ApplyGamma ("Apply Gamma", float) = 0
  }
  SubShader
  {
    Tags
    { 
      "CanUseSpriteAtlas" = "true"
      "IGNOREPROJECTOR" = "true"
      "PreviewType" = "Plane"
      "QUEUE" = "Transparent"
      "RenderType" = "Transparent"
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "CanUseSpriteAtlas" = "true"
        "IGNOREPROJECTOR" = "true"
        "PreviewType" = "Plane"
        "QUEUE" = "Transparent"
        "RenderType" = "Transparent"
      }
      ZClip Off
      ZWrite Off
      Cull Off
      Stencil
      { 
        Ref 0
        ReadMask 0
        WriteMask 0
        Pass Keep
        Fail Keep
        ZFail Keep
        PassFront Keep
        FailFront Keep
        ZFailFront Keep
        PassBack Keep
        FailBack Keep
        ZFailBack Keep
      } 
      Fog
      { 
        Mode  Off
      } 
      Blend SrcAlpha OneMinusSrcAlpha
      ColorMask 0
      // m_ProgramMask = 6
      CGPROGRAM
      #pragma multi_compile ALPHAPACK_NONE
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _Color;
      uniform float _VertScale;
      uniform sampler2D _MainTex;
      uniform float4 _ClipRect;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 color :COLOR;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_COLOR :COLOR;
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 xlv_COLOR :COLOR;
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float4 tmpvar_1;
          tmpvar_1 = in_v.vertex;
          float2 tmpvar_2;
          tmpvar_2 = in_v.texcoord.xy;
          float4 tmpvar_3;
          float4 tmpvar_4;
          float4 tmpvar_5;
          float4 tmpvar_6;
          tmpvar_6.w = 1;
          tmpvar_6.xyz = tmpvar_1.xyz;
          tmpvar_5 = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_6));
          tmpvar_4.xy = float2(tmpvar_2);
          if((_VertScale<0))
          {
              tmpvar_4.y = (1 - tmpvar_4.y);
          }
          float2 uv_7;
          uv_7 = tmpvar_4.xy;
          float4 result_8;
          result_8 = uv_7.xyxy;
          if((_VertScale<0))
          {
              result_8.yw = (1 - uv_7.yy);
          }
          tmpvar_4 = result_8;
          tmpvar_3 = (in_v.color * _Color);
          out_v.vertex = tmpvar_5;
          out_v.xlv_COLOR = tmpvar_3;
          out_v.xlv_TEXCOORD0 = tmpvar_4;
          out_v.xlv_TEXCOORD1 = tmpvar_1;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 tmpvar_1;
          float4 color_2;
          float4 tmpvar_3;
          tmpvar_3 = tex2D(_MainTex, in_f.xlv_TEXCOORD0.xy);
          color_2 = tmpvar_3;
          color_2 = (color_2 * in_f.xlv_COLOR);
          float tmpvar_4;
          float2 tmpvar_5;
          tmpvar_5.x = float((_ClipRect.z>=in_f.xlv_TEXCOORD1.x));
          tmpvar_5.y = float((_ClipRect.w>=in_f.xlv_TEXCOORD1.y));
          float2 tmpvar_6;
          tmpvar_6 = (float2(bool2(in_f.xlv_TEXCOORD1.xy >= _ClipRect.xy)) * tmpvar_5);
          tmpvar_4 = (tmpvar_6.x * tmpvar_6.y);
          color_2.w = (color_2.w * tmpvar_4);
          float x_7;
          x_7 = (color_2.w - 0.001);
          if((x_7<0))
          {
              discard;
          }
          tmpvar_1 = color_2;
          out_f.color = tmpvar_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
