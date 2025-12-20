Shader "Xffect/Object/volume_fog"
{
  Properties
  {
    _FogColor ("Fog Color", Color) = (1,1,1,1)
  }
  SubShader
  {
    Tags
    { 
      "IGNOREPROJECTOR" = "true"
      "QUEUE" = "Transparent+99"
      "RenderType" = "Transparent"
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "IGNOREPROJECTOR" = "true"
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
      //uniform float3 _WorldSpaceCameraPos;
      //uniform float4 _ProjectionParams;
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      //uniform float4 _ZBufferParams;
      uniform float4 _FogColor;
      uniform sampler2D _CameraDepthTexture;
      uniform float4 FogParam;
      struct appdata_t
      {
          float4 vertex :POSITION;
      };
      
      struct OUT_Data_Vert
      {
          float3 xlv_TEXCOORD0 :TEXCOORD0;
          float4 xlv_TEXCOORD1 :TEXCOORD1;
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float3 xlv_TEXCOORD0 :TEXCOORD0;
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
          float4 tmpvar_2;
          float4 tmpvar_3;
          tmpvar_3.w = 1;
          tmpvar_3.xyz = in_v.vertex.xyz;
          tmpvar_2 = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_3));
          tmpvar_1.xyw = tmpvar_2.xyw;
          float4 o_4;
          float4 tmpvar_5;
          tmpvar_5 = (tmpvar_2 * 0.5);
          float2 tmpvar_6;
          tmpvar_6.x = tmpvar_5.x;
          tmpvar_6.y = (tmpvar_5.y * _ProjectionParams.x);
          o_4.xy = (tmpvar_6 + tmpvar_5.w);
          o_4.zw = tmpvar_2.zw;
          tmpvar_1.z = (tmpvar_2.z * float(((tmpvar_2.z / tmpvar_2.w)>0)));
          out_v.vertex = tmpvar_1;
          out_v.xlv_TEXCOORD0 = (mul(unity_ObjectToWorld, in_v.vertex).xyz - _WorldSpaceCameraPos);
          out_v.xlv_TEXCOORD1 = o_4;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 color_1;
          color_1 = float4(1, 1, 1, 1);
          float4 tmpvar_2;
          tmpvar_2 = tex2D(_CameraDepthTexture, in_f.xlv_TEXCOORD1);
          float z_3;
          z_3 = tmpvar_2.x;
          float3 sphereCenter_4;
          sphereCenter_4 = FogParam.xyz;
          float sphereRadius_5;
          sphereRadius_5 = FogParam.w;
          float3 cameraPosition_6;
          cameraPosition_6 = _WorldSpaceCameraPos;
          float3 viewDirection_7;
          viewDirection_7 = normalize(in_f.xlv_TEXCOORD0);
          float backDepth_8;
          backDepth_8 = sqrt(dot(in_f.xlv_TEXCOORD0, in_f.xlv_TEXCOORD0));
          float maxDistance_9;
          maxDistance_9 = (1 / ((_ZBufferParams.z * z_3) + _ZBufferParams.w));
          float density_10;
          density_10 = _FogColor.w;
          float tmpvar_11;
          int seg_12;
          float step_distance_13;
          float fog_14;
          float xlat_varsample_15;
          float dist_16;
          float3 tmpvar_17;
          tmpvar_17 = (_WorldSpaceCameraPos - FogParam.xyz);
          float tmpvar_18;
          tmpvar_18 = dot(viewDirection_7, viewDirection_7);
          float tmpvar_19;
          tmpvar_19 = (2 * dot(viewDirection_7, tmpvar_17));
          float tmpvar_20;
          tmpvar_20 = ((tmpvar_19 * tmpvar_19) - ((4 * tmpvar_18) * (dot(tmpvar_17, tmpvar_17) - (FogParam.w * FogParam.w))));
          if((tmpvar_20<0))
          {
              tmpvar_11 = 0;
          }
          else
          {
              float tmpvar_21;
              tmpvar_21 = (0.5 / tmpvar_18);
              if((tmpvar_20==0))
              {
                  dist_16 = backDepth_8;
              }
              else
              {
                  dist_16 = (((-tmpvar_19) - sqrt(tmpvar_20)) * tmpvar_21);
              }
              float tmpvar_22;
              tmpvar_22 = min(dist_16, maxDistance_9);
              dist_16 = tmpvar_22;
              float tmpvar_23;
              tmpvar_23 = min(backDepth_8, maxDistance_9);
              backDepth_8 = tmpvar_23;
              xlat_varsample_15 = tmpvar_22;
              fog_14 = 0;
              step_distance_13 = ((tmpvar_23 - tmpvar_22) / 10);
              seg_12 = 0;
              while((seg_12<10))
              {
                  float3 x_24;
                  x_24 = (sphereCenter_4 - (cameraPosition_6 + (viewDirection_7 * xlat_varsample_15)));
                  fog_14 = (fog_14 + (1 - clamp((sqrt(dot(x_24, x_24)) / sphereRadius_5), 0, 1)));
                  xlat_varsample_15 = (xlat_varsample_15 + step_distance_13);
              }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack "VertexLit"
}
