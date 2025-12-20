Shader "Xffect/PP/radial_blur_mask"
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
      uniform float _SampleDist;
      uniform float _SampleStrength;
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
          float4 tmpvar_1;
          float4 sum_2;
          float2 texCoord_3;
          texCoord_3 = in_f.xlv_TEXCOORD0;
          float4 tmpvar_4;
          tmpvar_4 = tex2D(_MainTex, texCoord_3);
          float4 tmpvar_5;
          tmpvar_5 = tex2D(_Mask, texCoord_3);
          float2 P_6;
          P_6 = (texCoord_3 + ((((0.5 - texCoord_3) * tmpvar_5.x) * _SampleDist) * 0.01));
          sum_2 = (tmpvar_4 + tex2D(_MainTex, P_6));
          sum_2 = (sum_2 / 2);
          float4 tmpvar_7;
          tmpvar_7 = lerp(tmpvar_4, sum_2, float4(_SampleStrength, _SampleStrength, _SampleStrength, _SampleStrength));
          tmpvar_1 = tmpvar_7;
          out_f.color = tmpvar_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
