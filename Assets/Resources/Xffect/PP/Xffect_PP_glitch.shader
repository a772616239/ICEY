Shader "Xffect/PP/glitch"
{
  Properties
  {
    _MainTex ("Base (RGB)", 2D) = "white" {}
    _Mask ("Mask Texture (RG)", 2D) = "white" {}
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
      uniform sampler2D _MainTex;
      uniform sampler2D _Mask;
      uniform float4 _displace;
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
          float2 tmpvar_1;
          tmpvar_1 = in_v.texcoord.xy;
          float2 tmpvar_2;
          float4 tmpvar_3;
          tmpvar_3.w = 1;
          tmpvar_3.xyz = in_v.vertex.xyz;
          tmpvar_2 = tmpvar_1;
          out_v.vertex = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_3));
          out_v.xlv_TEXCOORD0 = tmpvar_2;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float2 ouv_1;
          float4 c_2;
          float4 tmpvar_3;
          tmpvar_3 = tex2D(_Mask, in_f.xlv_TEXCOORD0);
          c_2.w = tex2D(_MainTex, in_f.xlv_TEXCOORD0).w;
          ouv_1.y = in_f.xlv_TEXCOORD0.y;
          ouv_1.x = (in_f.xlv_TEXCOORD0.x + (_displace.x * tmpvar_3.x));
          c_2.x = tex2D(_MainTex, ouv_1).x;
          ouv_1.x = (in_f.xlv_TEXCOORD0.x + (_displace.y * tmpvar_3.x));
          c_2.y = tex2D(_MainTex, ouv_1).y;
          ouv_1.x = (in_f.xlv_TEXCOORD0.x + (_displace.z * tmpvar_3.x));
          c_2.z = tex2D(_MainTex, ouv_1).z;
          out_f.color = c_2;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
