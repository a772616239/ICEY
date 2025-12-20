Shader "Xffect/Object/spherical_billboard_add"
{
  Properties
  {
    _TintColor ("Tint Color", Color) = (0.5,0.5,0.5,0.5)
    _MainTex ("Main Texture", 2D) = "white" {}
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
      ZTest Always
      ZWrite Off
      Cull Off
      Blend SrcAlpha One
      // m_ProgramMask = 6
      CGPROGRAM
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      #define conv_mxt4x4_1(mat4x4) float4(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y,mat4x4[3].y)
      #define conv_mxt4x4_2(mat4x4) float4(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z,mat4x4[3].z)
      #define conv_mxt4x4_3(mat4x4) float4(mat4x4[0].w,mat4x4[1].w,mat4x4[2].w,mat4x4[3].w)
      
      
      #define CODE_BLOCK_VERTEX
      //uniform float4 _ProjectionParams;
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 UNITY_MATRIX_P;
      //uniform float4x4 unity_MatrixV;
      //uniform float4 _ZBufferParams;
      uniform sampler2D _MainTex;
      uniform float4 _TintColor;
      uniform sampler2D _CameraDepthTexture;
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
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float3 xlv_TEXCOORD2 :TEXCOORD2;
          float3 xlv_TEXCOORD3 :TEXCOORD3;
          float2 xlv_TEXCOORD4 :TEXCOORD4;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 xlv_COLOR :COLOR;
          float2 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float3 xlv_TEXCOORD2 :TEXCOORD2;
          float3 xlv_TEXCOORD3 :TEXCOORD3;
          float2 xlv_TEXCOORD4 :TEXCOORD4;
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
          float3 Q_2;
          float4 tmpvar_3;
          float2 tmpvar_4;
          tmpvar_4 = tmpvar_1;
          float4x4 tmpvar_5;
          float tmpvar_6;
          tmpvar_6 = cos(in_v.texcoord1.y);
          conv_mxt4x4_0(tmpvar_5).x = tmpvar_6;
          float tmpvar_7;
          tmpvar_7 = sin(in_v.texcoord1.y);
          conv_mxt4x4_0(tmpvar_5).y = tmpvar_7;
          conv_mxt4x4_0(tmpvar_5).z = 0;
          conv_mxt4x4_0(tmpvar_5).w = 0;
          conv_mxt4x4_1(tmpvar_5).x = (-tmpvar_7);
          conv_mxt4x4_1(tmpvar_5).y = tmpvar_6;
          conv_mxt4x4_1(tmpvar_5).z = 0;
          conv_mxt4x4_1(tmpvar_5).w = 0;
          conv_mxt4x4_2(tmpvar_5).x = 0;
          conv_mxt4x4_2(tmpvar_5).y = 0;
          conv_mxt4x4_2(tmpvar_5).z = 1;
          conv_mxt4x4_2(tmpvar_5).w = 0;
          conv_mxt4x4_3(tmpvar_5).x = 0;
          conv_mxt4x4_3(tmpvar_5).y = 0;
          conv_mxt4x4_3(tmpvar_5).z = 0;
          conv_mxt4x4_3(tmpvar_5).w = 1;
          float3 tmpvar_8;
          tmpvar_8 = mul(mul(unity_MatrixV, unity_ObjectToWorld), in_v.vertex).xyz;
          float3 tmpvar_9;
          tmpvar_9 = normalize(tmpvar_8);
          float3 tmpvar_10;
          tmpvar_10 = mul(tmpvar_5, float4(0, 1, 0, 1)).xyz;
          float3 tmpvar_11;
          tmpvar_11 = normalize(((tmpvar_10.yzx * tmpvar_9.zxy) - (tmpvar_10.zxy * tmpvar_9.yzx)));
          Q_2 = (tmpvar_8 + ((((in_v.texcoord.x - 0.5) * tmpvar_11) * in_v.texcoord1.x) + (((in_v.texcoord.y - 0.5) * normalize(((tmpvar_9.yzx * tmpvar_11.zxy) - (tmpvar_9.zxy * tmpvar_11.yzx)))) * in_v.texcoord1.x)));
          float4 tmpvar_12;
          tmpvar_12.w = 1;
          tmpvar_12.xyz = float3(Q_2);
          tmpvar_3 = mul(UNITY_MATRIX_P, tmpvar_12);
          tmpvar_3.z = (tmpvar_3.z * float(((tmpvar_3.z / tmpvar_3.w)>0)));
          float4 o_13;
          float4 tmpvar_14;
          tmpvar_14 = (tmpvar_3 * 0.5);
          float2 tmpvar_15;
          tmpvar_15.x = tmpvar_14.x;
          tmpvar_15.y = (tmpvar_14.y * _ProjectionParams.x);
          o_13.xy = (tmpvar_15 + tmpvar_14.w);
          o_13.zw = tmpvar_3.zw;
          out_v.vertex = tmpvar_3;
          out_v.xlv_COLOR = in_v.color;
          out_v.xlv_TEXCOORD0 = tmpvar_4;
          out_v.xlv_TEXCOORD1 = o_13;
          out_v.xlv_TEXCOORD2 = tmpvar_8;
          out_v.xlv_TEXCOORD3 = Q_2;
          out_v.xlv_TEXCOORD4 = in_v.texcoord1.xy;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 tmpvar_1;
          float tmpvar_2;
          tmpvar_2 = (in_f.xlv_COLOR.w * sqrt(dot(in_f.xlv_COLOR.xyz, in_f.xlv_COLOR.xyz)));
          float tau_3;
          tau_3 = tmpvar_2;
          float alpha_4;
          alpha_4 = 0;
          float tmpvar_5;
          tmpvar_5 = _ProjectionParams.z;
          float tmpvar_6;
          float3 x_7;
          x_7 = (in_f.xlv_TEXCOORD3 - in_f.xlv_TEXCOORD2);
          tmpvar_6 = sqrt(dot(x_7, x_7));
          float tmpvar_8;
          tmpvar_8 = sqrt(dot(in_f.xlv_TEXCOORD3, in_f.xlv_TEXCOORD3));
          float tmpvar_9;
          tmpvar_9 = ((_ProjectionParams.y * tmpvar_8) / in_f.xlv_TEXCOORD3.z);
          if((tmpvar_6<in_f.xlv_TEXCOORD4.x))
          {
              float ds_10;
              float Ds_11;
              float tmpvar_12;
              tmpvar_12 = sqrt(((in_f.xlv_TEXCOORD4.x * in_f.xlv_TEXCOORD4.x) - (tmpvar_6 * tmpvar_6)));
              float4 tmpvar_13;
              tmpvar_13 = tex2D(_CameraDepthTexture, in_f.xlv_TEXCOORD1);
              float tmpvar_14;
              float z_15;
              z_15 = tmpvar_13.x;
              tmpvar_14 = (1 / ((_ZBufferParams.z * z_15) + _ZBufferParams.w));
              Ds_11 = tmpvar_14;
              if((tmpvar_14==0))
              {
                  Ds_11 = tmpvar_5;
              }
              float tmpvar_16;
              tmpvar_16 = (min((tmpvar_8 + tmpvar_12), Ds_11) - max((tmpvar_8 - tmpvar_12), tmpvar_9));
              ds_10 = tmpvar_16;
              if((tmpvar_16<0))
              {
                  ds_10 = 0;
              }
              float tmpvar_17;
              tmpvar_17 = (1 - (tmpvar_6 / in_f.xlv_TEXCOORD4.x));
              alpha_4 = (1 - exp((((-tau_3) * (tmpvar_17 * tmpvar_17)) * ds_10)));
          }
          float4 tmpvar_18;
          tmpvar_18 = tex2D(_MainTex, in_f.xlv_TEXCOORD0);
          tmpvar_1 = ((2 * in_f.xlv_COLOR) * ((_TintColor * tmpvar_18) * alpha_4));
          out_f.color = tmpvar_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "VertexLit"
}
