Shader "Xffect/PP/radial_blur"
{
  Properties
  {
    _MainTex ("Base (RGB)", 2D) = "white" {}
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
      uniform float _SampleDist;
      uniform float _SampleStrength;
      uniform float4 _Center;
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
          float samples_2[10];
          float4 sum_3;
          float2 dir_4;
          float2 texCoord_5;
          texCoord_5 = in_f.xlv_TEXCOORD0;
          float2 tmpvar_6;
          tmpvar_6 = (_Center.xy - texCoord_5);
          float tmpvar_7;
          tmpvar_7 = sqrt(dot(tmpvar_6, tmpvar_6));
          dir_4 = (tmpvar_6 / tmpvar_7);
          float4 tmpvar_8;
          tmpvar_8 = tex2D(_MainTex, texCoord_5);
          sum_3 = tmpvar_8;
          samples_2[0] = (-0.08);
          samples_2[1] = (-0.05);
          samples_2[2] = (-0.03);
          samples_2[3] = (-0.02);
          samples_2[4] = (-0.01);
          samples_2[5] = 0.01;
          samples_2[6] = 0.02;
          samples_2[7] = 0.03;
          samples_2[8] = 0.05;
          samples_2[9] = 0.08;
          int i_1_1 = 0;
          while((i_1_1<10))
          {
              float2 P_9;
              P_9 = (texCoord_5 + ((dir_4 * samples_2[i_1_1]) * _SampleDist));
              sum_3 = (sum_3 + tex2D(_MainTex, P_9));
              i_1_1 = (i_1_1 + 1);
          }
          sum_3 = (sum_3 / 11);
          float tmpvar_10;
          float tmpvar_11;
          tmpvar_11 = clamp((tmpvar_7 * _SampleStrength), 0, 1);
          tmpvar_10 = tmpvar_11;
          float4 tmpvar_12;
          tmpvar_12 = lerp(tmpvar_8, sum_3, float4(tmpvar_10, tmpvar_10, tmpvar_10, tmpvar_10));
          out_f.color = tmpvar_12;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
