Shader "Xffect/displacement/screen"
{
  Properties
  {
    _DispMap ("Displacement Map (RG)", 2D) = "white" {}
    _MaskTex ("Mask (R)", 2D) = "white" {}
    _DispScrollSpeedX ("Map Scroll Speed X", float) = 0
    _DispScrollSpeedY ("Map Scroll Speed Y", float) = 0
    _StrengthX ("Displacement Strength X", float) = 1
    _StrengthY ("Displacement Strength Y", float) = -1
    _EdgeSmooth ("Edge smooth", float) = 0
  }
  SubShader
  {
    Tags
    { 
      "QUEUE" = "Transparent+99"
      "RenderType" = "Transparent"
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
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
      // m_ProgramMask = 0
      
    } // end phase
    Pass // ind: 2, name: BASE
    {
      Name "BASE"
      Tags
      { 
        "LIGHTMODE" = "ALWAYS"
        "QUEUE" = "Transparent+99"
        "RenderType" = "Transparent"
      }
      ZClip Off
      ZTest Always
      ZWrite Off
      Cull Off
      Blend SrcAlpha OneMinusSrcAlpha
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _DispMap_ST;
      //uniform float4 _Time;
      uniform float _StrengthX;
      uniform float _StrengthY;
      uniform sampler2D _DispMap;
      uniform sampler2D _MaskTex;
      uniform float _DispScrollSpeedY;
      uniform float _DispScrollSpeedX;
      uniform sampler2D _GrabTexture;
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
          float4 xlv_TEXCOORD2 :TEXCOORD2;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 xlv_COLOR :COLOR;
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float2 xlv_TEXCOORD1 :TEXCOORD1;
          float4 xlv_TEXCOORD2 :TEXCOORD2;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float4 tmpvar_1;
          float4 tmpvar_2;
          float4 tmpvar_3;
          tmpvar_3.w = 1;
          tmpvar_3.xyz = in_v.vertex.xyz;
          tmpvar_2 = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_3));
          tmpvar_1.xy = ((tmpvar_2.xy + tmpvar_2.w) * 0.5);
          tmpvar_1.zw = tmpvar_2.zw;
          out_v.vertex = tmpvar_2;
          out_v.xlv_COLOR = in_v.color;
          out_v.xlv_TEXCOORD0 = TRANSFORM_TEX(in_v.texcoord.xy, _DispMap);
          out_v.xlv_TEXCOORD1 = in_v.texcoord1.xy;
          out_v.xlv_TEXCOORD2 = tmpvar_1;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 tmpvar_1;
          tmpvar_1.zw = in_f.xlv_TEXCOORD2.zw;
          float4 col_2;
          float oftY_3;
          float oftX_4;
          float4 offsetColor_5;
          float2 mapoft_6;
          float2 tmpvar_7;
          tmpvar_7.x = (_Time.y * _DispScrollSpeedX);
          tmpvar_7.y = (_Time.y * _DispScrollSpeedY);
          mapoft_6 = tmpvar_7;
          float4 tmpvar_8;
          float2 P_9;
          P_9 = (in_f.xlv_TEXCOORD0 + mapoft_6);
          tmpvar_8 = tex2D(_DispMap, P_9);
          offsetColor_5 = tmpvar_8;
          float tmpvar_10;
          tmpvar_10 = ((offsetColor_5.x * _StrengthX) * in_f.xlv_TEXCOORD1.x);
          oftX_4 = tmpvar_10;
          float tmpvar_11;
          tmpvar_11 = ((offsetColor_5.y * _StrengthY) * in_f.xlv_TEXCOORD1.x);
          oftY_3 = tmpvar_11;
          tmpvar_1.x = (in_f.xlv_TEXCOORD2.x + oftX_4);
          tmpvar_1.y = (in_f.xlv_TEXCOORD2.y + oftY_3);
          float4 tmpvar_12;
          tmpvar_12 = tex2D(_GrabTexture, tmpvar_1);
          col_2.xyz = tmpvar_12.xyz;
          col_2.w = in_f.xlv_COLOR.w;
          float4 tmpvar_13;
          tmpvar_13 = tex2D(_MaskTex, in_f.xlv_TEXCOORD0);
          col_2.w = (col_2.w * tmpvar_13.x);
          out_f.color = col_2;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  SubShader
  {
    Tags
    { 
      "QUEUE" = "Transparent+99"
      "RenderType" = "Transparent"
    }
    Pass // ind: 1, name: BASE
    {
      Name "BASE"
      Tags
      { 
        "QUEUE" = "Transparent+99"
        "RenderType" = "Transparent"
      }
      ZClip Off
      ZTest Always
      ZWrite Off
      Cull Off
      Fog
      { 
        Mode  Off
      } 
      Blend SrcAlpha OneMinusSrcAlpha
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _MainTex_ST;
      uniform sampler2D _MainTex;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 color :COLOR;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_COLOR0 :COLOR0;
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 xlv_COLOR0 :COLOR0;
          float2 xlv_TEXCOORD0 :TEXCOORD0;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float4 tmpvar_1;
          float4 tmpvar_2;
          tmpvar_2 = clamp(in_v.color, 0, 1);
          tmpvar_1 = tmpvar_2;
          float4 tmpvar_3;
          tmpvar_3.w = 1;
          tmpvar_3.xyz = in_v.vertex.xyz;
          out_v.xlv_COLOR0 = tmpvar_1;
          out_v.xlv_TEXCOORD0 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_3));
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 col_1;
          float4 tmpvar_2;
          tmpvar_2 = tex2D(_MainTex, in_f.xlv_TEXCOORD0);
          col_1.xyz = (tmpvar_2 * in_f.xlv_COLOR0).xyz;
          col_1.xyz = (col_1 * 2).xyz;
          col_1.w = (tmpvar_2.w * in_f.xlv_COLOR0.w);
          if((col_1.w<=0.01))
          {
              discard;
          }
          out_f.color = col_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
