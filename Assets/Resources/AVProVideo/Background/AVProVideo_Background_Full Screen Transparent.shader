Shader "AVProVideo/Background/Full Screen Transparent"
{
  Properties
  {
    _MainTex ("Texture", 2D) = "black" {}
    _Color ("Main Color", Color) = (1,1,1,1)
    [KeywordEnum(None, Top_Bottom, Left_Right)] AlphaPack ("Alpha Pack", float) = 0
    [Toggle(APPLY_GAMMA)] _ApplyGamma ("Apply Gamma", float) = 0
  }
  SubShader
  {
    Tags
    { 
      "QUEUE" = "Background+1"
      "RenderType" = "Transparent"
    }
    LOD 100
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "QUEUE" = "Background+1"
        "RenderType" = "Transparent"
      }
      LOD 100
      ZClip Off
      ZTest Always
      ZWrite Off
      Cull Off
      Blend SrcAlpha OneMinusSrcAlpha
      // m_ProgramMask = 6
      CGPROGRAM
      #pragma multi_compile ALPHAPACK_NONE
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4 _ProjectionParams;
      //uniform float4 _ScreenParams;
      uniform float4 _MainTex_ST;
      uniform float4 _MainTex_TexelSize;
      uniform sampler2D _MainTex;
      uniform float4 _Color;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float2 pos_1;
          float4 tmpvar_2;
          float2 scale_3;
          float tmpvar_4;
          tmpvar_4 = (_ScreenParams.y / _ScreenParams.x);
          float tmpvar_5;
          tmpvar_5 = (_MainTex_TexelSize.w / _MainTex_TexelSize.z);
          float2 tmpvar_6;
          tmpvar_6.x = 1;
          tmpvar_6.y = (tmpvar_5 / tmpvar_4);
          scale_3 = tmpvar_6;
          if((tmpvar_4<tmpvar_5))
          {
              float2 tmpvar_7;
              tmpvar_7.y = 1;
              tmpvar_7.x = (tmpvar_4 / tmpvar_5);
              scale_3 = tmpvar_7;
          }
          float2 tmpvar_8;
          tmpvar_8 = ((in_v.vertex.xy * scale_3) * 2);
          pos_1 = tmpvar_8;
          if((_ProjectionParams.x<0))
          {
              pos_1.y = ((1 - tmpvar_8.y) - 1);
          }
          float4 tmpvar_9;
          tmpvar_9.zw = float2(-1, 1);
          tmpvar_9.xy = float2(pos_1);
          tmpvar_2.xy = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          if((_MainTex_ST.y<0))
          {
              tmpvar_2.y = (1 - tmpvar_2.y);
          }
          float4 result_10;
          result_10 = tmpvar_2.xyxy;
          if((_MainTex_ST.y<0))
          {
              result_10.yw = (1 - tmpvar_2.yy);
          }
          tmpvar_2 = result_10;
          out_v.vertex = tmpvar_9;
          out_v.xlv_TEXCOORD0 = result_10;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 tmpvar_1;
          tmpvar_1 = (tex2D(_MainTex, in_f.xlv_TEXCOORD0.xy) * _Color);
          out_f.color = tmpvar_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
