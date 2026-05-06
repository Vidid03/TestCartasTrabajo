Shader "Hidden/Custom/AdvancedLensDistortion"
{
    HLSLINCLUDE
        #include "Packages/com.unity.postprocessing/PostProcessing/Shaders/StdLib.hlsl"

        TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
        
        float _Intensity;
        float _Zoom;
        float _Aspect; // Recibimos el aspect ratio
        float2 _Center;
        float2 _AxisFactors;

        float4 Frag(VaryingsDefault i) : SV_Target
        {
            float2 uv = i.texcoord;
            
            // 1. Corregimos la dirección según el Aspect Ratio
            // Esto hace que el cálculo de distancia sea circular, no ovalado
            float2 dir = uv - _Center;
            float2 correctedDir = dir;
            correctedDir.x *= _Aspect; 

            // 2. La distancia ahora es "equilibrada"
            float distSq = dot(correctedDir, correctedDir);
            
            // 3. Calculamos la distorsión usando la distancia corregida
            float distortion = _Intensity * distSq;

            // 4. Aplicamos la deformación a los ejes originales
            float2 axisOffset = float2(
                dir.x * (1.0 + distortion * _AxisFactors.x),
                dir.y * (1.0 + distortion * _AxisFactors.y)
            );

            float2 distortedUV = _Center + axisOffset * _Zoom;

            // Bordes negros
            if (distortedUV.x < 0 || distortedUV.x > 1 || distortedUV.y < 0 || distortedUV.y > 1)
            {
                return float4(0, 0, 0, 1);
            }

            return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, distortedUV);
        }
    ENDHLSL

    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            HLSLPROGRAM
                #pragma vertex VertDefault
                #pragma fragment Frag
            ENDHLSL
        }
    }
}