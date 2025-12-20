Shader "Xffect/PP/glow_downsample"
{
  Properties
  {
    _Color ("Color", Color) = (1,1,1,0)
    _MainTex ("", 2D) = "white" {}
  }
  SubShader
  {
    Tags
    { 
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
      }
      ZClip Off
      ZTest Always
      ZWrite Off
      Cull Off
      Fog
      { 
        Mode  Off
      } 
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
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
          float4 xlv_TEXCOORD0_1 :TEXCOORD0_1;
          float4 xlv_TEXCOORD0_2 :TEXCOORD0_2;
          float4 xlv_TEXCOORD0_3 :TEXCOORD0_3;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD0_1 :TEXCOORD0_1;
          float4 xlv_TEXCOORD0_2 :TEXCOORD0_2;
          float4 xlv_TEXCOORD0_3 :TEXCOORD0_3;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float2 tmpvar_1;
          tmpvar_1 = in_v.texcoord.xy;
          float4 uv_2;
          float4 tmpvar_3;
          tmpvar_3.w = 1;
          tmpvar_3.xyz = in_v.vertex.xyz;
          float2 inUV_4;
          inUV_4 = tmpvar_1;
          float4 tmpvar_5;
          tmpvar_5.zw = float2(0, 0);
          tmpvar_5.xy = float2(inUV_4);
          uv_2.xy = (float4x4(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1) * tmpvar_5).xy;
          uv_2.zw = float2(0, 0);
          float tmpvar_6;
          tmpvar_6 = _MainTex_TexelSize.x;
          float tmpvar_7;
          tmpvar_7 = _MainTex_TexelSize.y;
          float4 tmpvar_8;
          tmpvar_8.zw = float2(0, 1);
          float tmpvar_9;
          tmpvar_9 = (-_MainTex_TexelSize.x);
          tmpvar_8.x = tmpvar_9;
          float tmpvar_10;
          tmpvar_10 = (-_MainTex_TexelSize.y);
          tmpvar_8.y = tmpvar_10;
          float4 tmpvar_11;
          tmpvar_11.zw = float2(0, 1);
          tmpvar_11.x = tmpvar_6;
          tmpvar_11.y = tmpvar_10;
          float4 tmpvar_12;
          tmpvar_12.zw = float2(0, 1);
          tmpvar_12.x = tmpvar_6;
          tmpvar_12.y = tmpvar_7;
          float4 tmpvar_13;
          tmpvar_13.zw = float2(0, 1);
          tmpvar_13.x = tmpvar_9;
          tmpvar_13.y = tmpvar_7;
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_3));
          out_v.xlv_TEXCOORD0 = (uv_2 + tmpvar_8);
          out_v.xlv_TEXCOORD0_1 = (uv_2 + tmpvar_11);
          out_v.xlv_TEXCOORD0_2 = (uv_2 + tmpvar_12);
          out_v.xlv_TEXCOORD0_3 = (uv_2 + tmpvar_13);
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 c_1;
          c_1 = (tex2D(_MainTex, in_f.xlv_TEXCOORD0.xy) + tex2D(_MainTex, in_f.xlv_TEXCOORD0_1.xy));
          c_1 = (c_1 + tex2D(_MainTex, in_f.xlv_TEXCOORD0_2.xy));
          c_1 = (c_1 + tex2D(_MainTex, in_f.xlv_TEXCOORD0_3.xy));
          c_1 = (c_1 / 4);
          c_1.xyz = (c_1.xyz * _Color.xyz);
          c_1.xyz = (c_1.xyz * (c_1.w + _Color.w));
          c_1.w = 0;
          out_f.color = c_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  SubShader
  {
    Tags
    { 
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
      }
      ZClip Off
      ZTest Always
      ZWrite Off
      Cull Off
      Fog
      { 
        Mode  Off
      } 
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
      uniform float4 _Color;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float4 xlv_COLOR0 :COLOR0;
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float2 xlv_TEXCOORD1 :TEXCOORD1;
          float2 xlv_TEXCOORD2 :TEXCOORD2;
          float2 xlv_TEXCOORD3 :TEXCOORD3;
          float2 xlv_TEXCOORD4 :TEXCOORD4;
          float2 xlv_TEXCOORD5 :TEXCOORD5;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float2 xlv_TEXCOORD1 :TEXCOORD1;
          float2 xlv_TEXCOORD2 :TEXCOORD2;
          float2 xlv_TEXCOORD3 :TEXCOORD3;
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
          tmpvar_2 = clamp(float4(0, 0, 0, 1.1), 0, 1);
          tmpvar_1 = tmpvar_2;
          float4 tmpvar_3;
          tmpvar_3.w = 1;
          tmpvar_3.xyz = in_v.vertex.xyz;
          out_v.xlv_COLOR0 = tmpvar_1;
          float2 tmpvar_4;
          tmpvar_4 = (in_v.texcoord.xy * _MainTex_ST.xy);
          out_v.xlv_TEXCOORD0 = (tmpvar_4 + _MainTex_ST.zw);
          out_v.xlv_TEXCOORD1 = (tmpvar_4 + _MainTex_ST.zw);
          out_v.xlv_TEXCOORD2 = (tmpvar_4 + _MainTex_ST.zw);
          out_v.xlv_TEXCOORD3 = (tmpvar_4 + _MainTex_ST.zw);
          out_v.xlv_TEXCOORD4 = (tmpvar_4 + _MainTex_ST.zw);
          out_v.xlv_TEXCOORD5 = (tmpvar_4 + _MainTex_ST.zw);
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_3));
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 col_1;
          col_1 = (tex2D(_MainTex, in_f.xlv_TEXCOORD0) * 0.25);
          col_1 = ((tex2D(_MainTex, in_f.xlv_TEXCOORD1) * 0.25) + col_1);
          col_1 = ((tex2D(_MainTex, in_f.xlv_TEXCOORD2) * 0.25) + col_1);
          col_1 = ((tex2D(_MainTex, in_f.xlv_TEXCOORD3) * 0.25) + col_1);
          col_1.xyz = (col_1 * _Color).xyz;
          col_1.w = (col_1.w + _Color.w);
          col_1.xyz = (col_1 * col_1.w).xyz;
          col_1.w = 0;
          out_f.color = col_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
