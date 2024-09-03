Shader "Custom/NewSurfaceShader"
{
    Properties
    {
        _Color1("Stripe Color 1", Color) = (1, 1, 1, 1)
        _Color2("Stripe Color 2", Color) = (0, 0, 0, 1)
        _GradientColor("Gradient Color", Color) = (1, 1, 1, 1)
        _StripeCount("Stripe Count", Range(1, 100)) = 10
        _Rotation("Rotation (Degrees)", Range(0, 360)) = 0
        _GradientRadius("Gradient Radius", Range(0, 1)) = 0.5
        _Smoothness("Stripe Smoothness", Range(0, 1)) = 0.1
    }
    SubShader
    {
        Tags { "Queue" = "Geometry-1" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            float4 _Color1;
            float4 _Color2;
            float4 _GradientColor;
            float _StripeCount;
            float _Rotation;
            float _GradientRadius;
            float _Smoothness;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 centeredUV = i.uv - 0.5;

                // Convert rotation from degrees to radians
                float rotationRadians = _Rotation * UNITY_PI / 180.0;

                // Rotate the UV coordinates
                float2 rotatedUV = float2(
                    centeredUV.x * cos(rotationRadians) - centeredUV.y * sin(rotationRadians),
                    centeredUV.x * sin(rotationRadians) + centeredUV.y * cos(rotationRadians)
                );

                float angle = atan2(rotatedUV.y, rotatedUV.x) / (2.0 * UNITY_PI) + 0.5;
                float stripes = frac(angle * _StripeCount);

                // Smooth the transition between stripes with better control
                float smoothStripes = smoothstep(0.0, _Smoothness, abs(stripes - 0.5));

                // Calculate gradient based on distance from the center
                float dist = length(centeredUV) / _GradientRadius;
                dist = saturate(dist);

                fixed4 stripeColor = lerp(_Color1, _Color2, smoothStripes);
                fixed4 gradientColor = lerp(stripeColor, _GradientColor, dist);

                return gradientColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
