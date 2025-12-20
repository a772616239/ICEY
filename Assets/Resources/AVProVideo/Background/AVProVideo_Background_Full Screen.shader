Shader "AVProVideo/Background/Full Screen"
{
  Properties
  {
    _MainTex ("Texture", 2D) = "black" {}
    _Color ("Main Color", Color) = (1,1,1,1)
    [Toggle(APPLY_GAMMA)] _ApplyGamma ("Apply Gamma", float) = 0
  }
  SubShader
  {
    Tags
    { 
      "QUEUE" = "Background"
      "RenderType" = "Opaque"
    }
    LOD 100
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "QUEUE" = "Background"
        "RenderType" = "Opaque"
      }
      LOD 100
      ZClip Off
      ZTest Always
      ZWrite Off
      Cull Off
      // m_ProgramMask = 6
      CGPROGRAM
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
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float2 pos_1;
          float2 scale_2;
          float tmpvar_3;
          tmpvar_3 = (_ScreenParams.y / _ScreenParams.x);
          float tmpvar_4;
          tmpvar_4 = (_MainTex_TexelSize.w / _MainTex_TexelSize.z);
          float2 tmpvar_5;
          tmpvar_5.x = 1;
          tmpvar_5.y = (tmpvar_4 / tmpvar_3);
          scale_2 = tmpvar_5;
          if((tmpvar_3<tmpvar_4))
          {
              float2 tmpvar_6;
              tmpvar_6.y = 1;
              tmpvar_6.x = (tmpvar_3 / tmpvar_4);
              scale_2 = tmpvar_6;
          }
          float2 tmpvar_7;
          tmpvar_7 = ((in_v.vertex.xy * scale_2) * 2);
          pos_1 = tmpvar_7;
          if((_ProjectionParams.x<0))
          {
              pos_1.y = ((1 - tmpvar_7.y) - 1);
          }
          float4 tmpvar_8;
          tmpvar_8.zw = float2(-1, 1);
          tmpvar_8.xy = float2(pos_1);
          out_v.vertex = tmpvar_8;
          out_v.xlv_TEXCOORD0 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 tmpvar_1;
          tmpvar_1.w = 1;
          tmpvar_1.xyz = (tex2D(_MainTex, in_f.xlv_TEXCOORD0) * _Color).xyz;
          out_f.color = tmpvar_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
