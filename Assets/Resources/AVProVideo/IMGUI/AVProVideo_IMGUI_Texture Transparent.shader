Shader "AVProVideo/IMGUI/Texture Transparent"
{
  Properties
  {
    _MainTex ("Texture", any) = "" {}
    _VertScale ("Vertical Scale", Range(-1, 1)) = 1
    [KeywordEnum(None, Top_Bottom, Left_Right)] AlphaPack ("Alpha Pack", float) = 0
    [Toggle(APPLY_GAMMA)] _ApplyGamma ("Apply Gamma", float) = 0
  }
  SubShader
  {
    Tags
    { 
      "ForceSupported" = "true"
      "RenderType" = "Overlay"
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "ForceSupported" = "true"
        "RenderType" = "Overlay"
      }
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
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_MatrixVP;
      uniform float4 _MainTex_ST;
      uniform float _VertScale;
      uniform sampler2D _MainTex;
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
          float4 vertex :SV_POSITION;
      };
      
      struct v2f
      {
          float4 xlv_COLOR :COLOR;
          float4 xlv_TEXCOORD0 :TEXCOORD0;
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      OUT_Data_Vert vert(appdata_t in_v)
      {
          OUT_Data_Vert out_v;
          float4 tmpvar_1;
          tmpvar_1 = in_v.color;
          float4 tmpvar_2;
          float4 tmpvar_3;
          tmpvar_3.w = 1;
          tmpvar_3.xyz = in_v.vertex.xyz;
          tmpvar_2 = mul(unity_MatrixVP, mul(unity_ObjectToWorld, tmpvar_3));
          float2 uv_4;
          uv_4 = TRANSFORM_TEX(in_v.texcoord.xy, _MainTex);
          float4 result_5;
          result_5 = uv_4.xyxy;
          if((_VertScale<0))
          {
              result_5.yw = (1 - uv_4.yy);
          }
          out_v.vertex = tmpvar_2;
          out_v.xlv_COLOR = tmpvar_1;
          out_v.xlv_TEXCOORD0 = result_5;
          return out_v;
      }
      
      #define CODE_BLOCK_FRAGMENT
      OUT_Data_Frag frag(v2f in_f)
      {
          OUT_Data_Frag out_f;
          float4 tmpvar_1;
          tmpvar_1 = (tex2D(_MainTex, in_f.xlv_TEXCOORD0.xy) * in_f.xlv_COLOR);
          out_f.color = tmpvar_1;
          return out_f;
      }
      
      
      ENDCG
      
    } // end phase
  }
  FallBack Off
}
