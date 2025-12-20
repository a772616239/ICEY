Shader "AVProVideo/VR/InsideSphere Unlit (stereo) - Android OES ONLY"
{
  Properties
  {
    _MainTex ("Base (RGB)", 2D) = "white" {}
    [KeywordEnum(None, Top_Bottom, Left_Right)] Stereo ("Stereo Mode", float) = 0
    [Toggle(STEREO_DEBUG)] _StereoDebug ("Stereo Debug Tinting", float) = 0
    [Toggle(APPLY_GAMMA)] _ApplyGamma ("Apply Gamma", float) = 0
  }
  SubShader
  {
    Tags
    { 
      "QUEUE" = "Geometry"
    }
    Pass // ind: 1, name: 
    {
      Tags
      { 
        "QUEUE" = "Geometry"
      }
      ZClip Off
      ZWrite Off
      Cull Front
      // m_ProgramMask = 6
      CGPROGRAM
      #pragma multi_compile MONOSCOPIC
      //#pragma target 4.0
      
      #pragma vertex vert
      #pragma fragment frag
      
      #include "UnityCG.cginc"
      #define conv_mxt4x4_0(mat4x4) float4(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x,mat4x4[3].x)
      #define conv_mxt4x4_1(mat4x4) float4(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y,mat4x4[3].y)
      #define conv_mxt4x4_2(mat4x4) float4(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z,mat4x4[3].z)
      #define conv_mxt4x4_3(mat4x4) float4(mat4x4[0].w,mat4x4[1].w,mat4x4[2].w,mat4x4[3].w)
      #define conv_mxt3x3_0(mat4x4) float3(mat4x4[0].x,mat4x4[1].x,mat4x4[2].x)
      #define conv_mxt3x3_1(mat4x4) float3(mat4x4[0].y,mat4x4[1].y,mat4x4[2].y)
      #define conv_mxt3x3_2(mat4x4) float3(mat4x4[0].z,mat4x4[1].z,mat4x4[2].z)
      
      
      //uniform float4x4 unity_ObjectToWorld;
      //uniform float4x4 unity_WorldToObject;
      //uniform float4x4 unity_MatrixVP;
      //uniform float4x4 unity_MatrixV;
      //uniform float4x4 unity_MatrixInvV;
      //uniform float4x4 UNITY_MATRIX_P;
      //uniform float4 _Time;
      //uniform float4 _SinTime;
      //uniform float4 _CosTime;
      //uniform float4 _ProjectionParams;
      //uniform float4 _ScreenParams;
      //uniform float3 _WorldSpaceCameraPos;
      //uniform float4 _WorldSpaceLightPos0;
      //uniform float4 _LightPositionRange;
      uniform float3 _cameraPosition;
      uniform float4x4 _ViewMatrix;
      uniform sampler2D _MainTex;
      struct appdata_t
      {
          float4 vertex :POSITION;
          float3 normal :NORMAL;
          float4 texcoord :TEXCOORD0;
      };
      
      struct OUT_Data_Vert
      {
          float4 vertex :SV_POSITION;
          //return TEXCOORD[1];
      };
      
      struct v2f
      {
      };
      
      struct OUT_Data_Frag
      {
          float4 color :SV_Target0;
      };
      
      float3x3 transpose(float3x3 mtx)
      {
          float3 c0 = conv_mxt3x3_0(mtx);
          float3 c1 = conv_mxt3x3_1(mtx);
          float3 c2 = conv_mxt3x3_2(mtx);
          return float3x3(float3(c0.x, c1.x, c2.x), float3(c0.y, c1.y, c2.y), float3(c0.z, c1.z, c2.z));
      }
      
      float4x4 transpose(float4x4 mtx)
      {
          float4 c0 = conv_mxt4x4_0(mtx);
          float4 c1 = conv_mxt4x4_1(mtx);
          float4 c2 = conv_mxt4x4_2(mtx);
          float4 c3 = conv_mxt4x4_3(mtx);
          return float4x4(float4(c0.x, c1.x, c2.x, c3.x), float4(c0.y, c1.y, c2.y, c3.y), float4(c0.z, c1.z, c2.z, c3.z), float4(c0.w, c1.w, c2.w, c3.w));
      }
      
      #define CODE_BLOCK_VERTEX
      float saturate(float x)
      {
          return max(0, min(1, x));
      }
      
      #define CODE_BLOCK_VERTEX
      float3 WorldSpaceLightDir(float4 v )
      {
          float3 worldPos = mul(unity_ObjectToWorld, v).xyz;
          #ifndef USING_LIGHT_MULTI_COMPILE
          return (_WorldSpaceLightPos0.xyz - (worldPos * _WorldSpaceLightPos0.w));
          #else
          #ifndef USING_DIRECTIONAL_LIGHT
          return (_WorldSpaceLightPos0.xyz - worldPos);
          #else
          return _WorldSpaceLightPos0.xyz;
          #endif
          #endif
      }
      
      float3 ObjSpaceLightDir(float4 v )
      {
          float3 objSpaceLightPos = mul(unity_WorldToObject, _WorldSpaceLightPos0).xyz;
          #ifndef USING_LIGHT_MULTI_COMPILE
          return (objSpaceLightPos.xyz - (v.xyz * _WorldSpaceLightPos0.w));
          #else
          #ifndef USING_DIRECTIONAL_LIGHT
          return (objSpaceLightPos.xyz - v.xyz);
          #else
          return objSpaceLightPos.xyz;
          #endif
          #endif
      }
      
      float3 WorldSpaceViewDir(float4 v )
      {
          return (_WorldSpaceCameraPos.xyz - mul(unity_ObjectToWorld, v).xyz);
      }
      
      float3 ObjSpaceViewDir(float4 v )
      {
          float3 objSpaceCameraPos = mul(unity_WorldToObject, float4(_WorldSpaceCameraPos.xyz, 1)).xyz;
          return (objSpaceCameraPos - v.xyz);
      }
      
      float2 ParallaxOffset(float h, float height, float3 viewDir )
      {
          h = ((h * height) - (height / 2));
          float3 v = normalize(viewDir);
          v.z = (v.z + 0.42);
          return (h * (v.xy / v.z));
      }
      
      float Luminance(float3 c )
      {
          return dot(c, float3(0.22, 0.707, 0.071));
      }
      
      int IsStereoEyeLeft(float3 worldNosePosition, float3 worldCameraRight)
      {
          #if defined( UNITY_SINGLE_PASS_STEREO)
          {
              return {(unity_StereoEyeIndex==0)};
              #else
              float dRight = distance((worldNosePosition + worldCameraRight), _WorldSpaceCameraPos);
              float dLeft = distance((worldNosePosition - worldCameraRight), _WorldSpaceCameraPos);
              return {(dRight>dLeft)};
              #endif
          }
      
          float4 GetStereoScaleOffset(int isLeftEye)
          {
              float2 scale = float2(1, 1);
              float2 offset = float2(0, 0);
              #if defined( STEREO_TOP_BOTTOM)
          {
              scale.y = 0.5;
              offset.y = 0;
              if(!isLeftEye)
              {
                  offset.y = 0.5;
              }
              #if !defined( SHADERLAB_GLSL)
          {
              #if !defined( UNITY_UV_STARTS_AT_TOP)
          {
              offset.y = (0.5 - offset.y);
              #endif
              #endif
              #elif defined( STEREO_LEFT_RIGHT)
          {
              scale.x = 0.5;
              offset.x = 0;
              if(!isLeftEye)
              {
                  offset.x = 0.5;
              }
              #endif
              return float4(scale, offset);
          }
      
              float4 GetStereoDebugTint(int isLeftEye)
          {
              float4 tint = float4(1, 1, 1, 1);
              #if defined( STEREO_TOP_BOTTOM)
          {
              | defined( STEREO_LEFT_RIGHT)
          {
              float4 leftEyeColor = float4(0, 1, 0, 1);
              float4 rightEyeColor = float4(1, 0, 0, 1);
              if(isLeftEye)
              {
                  tint = leftEyeColor;
              }
              else
              {
                  tint = rightEyeColor;
              }
              #endif
              #if defined( UNITY_UV_STARTS_AT_TOP)
          {
              tint.b = 0.5;
              #endif
              return tint;
          }
      
              float2 ScaleZoomToFit(float targetWidth, float targetHeight, float sourceWidth, float sourceHeight)
          {
              #if defined( ALPHAPACK_TOP_BOTTOM)
          {
              sourceHeight = (sourceHeight * 0.5);
              #elif defined( ALPHAPACK_LEFT_RIGHT)
          {
              sourceWidth = (sourceWidth * 0.5);
              #endif
              float targetAspect = (targetHeight / targetWidth);
              float sourceAspect = (sourceHeight / sourceWidth);
              float2 scale = float2(1, (sourceAspect / targetAspect));
              if((targetAspect<sourceAspect))
              {
                  scale = float2((targetAspect / sourceAspect), 1);
              }
              return scale;
          }
      
              float4 OffsetAlphaPackingUV(float2 texelSize, float2 uv, int flipVertical)
          {
              float4 result = uv.xyxy;
              #if defined( ALPHAPACK_TOP_BOTTOM)
          {
              float offset = (texelSize.y * 1.5);
              result.y = lerp((0 + offset), (0.5 - offset), uv.y);
              result.w = (result.y + 0.5);
              if(flipVertical)
              {
                  result.yw = (1 - result.yw);
                  result.yw = result.wy;
              }
              else
              {
                  #if !defined( UNITY_UV_STARTS_AT_TOP)
          {
              result.yw = result.wy;
              #endif
          }
      
                  #elif defined( ALPHAPACK_LEFT_RIGHT)
          {
              float offset = (texelSize.x * 1.5);
              result.x = lerp((0 + offset), (0.5 - offset), uv.x);
              result.z = (result.x + 0.5);
              if(flipVertical)
              {
                  result.yw = (1 - result.yw);
              }
              #else
              if(flipVertical)
              {
                  result.yw = (1 - result.yw);
              }
              #endif
              return result;
          }
      
                  float3 GammaToLinear(float3 col)
          {
              #if defined( CHEAP_GAMMATOLINEAR)
          {
              col = pow(col, 2.2);
              #else
              if((col.r<=0.03928))
              {
                  col.r = (col.r / 12.92);
              }
              else
              {
                  col.r = pow(((col.r + 0.055) / 1.055), 2.4);
              }
              if((col.g<=0.03928))
              {
                  col.g = (col.g / 12.92);
              }
              else
              {
                  col.g = pow(((col.g + 0.055) / 1.055), 2.4);
              }
              if((col.b<=0.03928))
              {
                  col.b = (col.b / 12.92);
              }
              else
              {
                  col.b = pow(((col.b + 0.055) / 1.055), 2.4);
              }
              #endif
              return col;
          }
      
              OUT_Data_Vert vert(appdata_t in_v)
          {
              out_v.vertex = mul(mul(unity_MatrixVP, unity_ObjectToWorld), in_v.vertex);
              out_v.texVal = in_v.texcoord.xy;
              out_v.texVal.x = (1 - out_v.texVal.x);
              out_v.texVal.y = (1 - out_v.texVal.y);
              #if defined( STEREO_TOP_BOTTOM)
          {
              #define __IL_START_FUN_defined.defined__|____STEREO_LEFT_RIGHT_
          }
      
          }
      
          }
      
              }
          }
      
          }
      
          }
      
          }
      
          }
      
          }
      
          }
      
          }
      
          }
      
          }
      
          }
      
      }
      
      OUT_Data_Frag frag(v2f in_f)
          {
              #if defined( SHADER_API_GLES)
          {
              | defined( SHADER_API_GLES3)
          {
              out_f.color = tex2D(_MainTex, in_f.texVal.xy);
              #else
              out_f.color = float4(1, 1, 0, 1);
              #endif
              #if defined( STEREO_DEBUG)
          {
              out_f.color = (out_f.color * in_f.tint);
              #endif
          }
      
          }
      
          }
      
          }
      
      ENDCG
      
    } // end phase
  }
  FallBack "AVProVideo/VR/InsideSphere Unlit (stereo+fog)"
}
