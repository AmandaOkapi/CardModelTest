Shader "Custom/Glass"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _StreakColor ("Streak Color", Color) = (1,1,1,1)
        _StreakWidth ("Streak Width", Range(0.0, 1.0)) = 0.01 // Further reduced streak width
        _CycleDuration ("Cycle Duration (Seconds)", Range(0.1, 10.0)) = 1.0 // Faster travel
        _BreakDuration ("Break Duration (Seconds)", Range(0.0, 10.0)) = 5.0
        _Transparency ("Transparency", Range(0.0, 1.0)) = 0.5
    }
    SubShader
    {
        Tags {"Queue"="Transparent" "RenderType"="Transparent"}
        LOD 200

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            fixed4 _StreakColor;
            float _StreakWidth;
            float _CycleDuration;
            float _BreakDuration;
            float _Transparency;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                // Calculate time-based animation loop, normalized between 0 and 1
                float totalCycleTime = _Time.y;
                float cycleTime = fmod(totalCycleTime, _CycleDuration + _BreakDuration);
                
                // Adjust cycleTime to account for the break period
                if (cycleTime >= _CycleDuration)
                {
                    cycleTime = _CycleDuration; // During the break, no streak should be visible
                }
                else
                {
                    cycleTime /= _CycleDuration; // Normalize for streak effect
                }

                // Increase streak speed by multiplying cycleTime by a factor
                float streakPos = cycleTime * 2.0 + (( -1.0 - i.uv.x) + i.uv.y) * 0.5;

                // Create the streak effect using smoothstep for gradual edges
                float streak = smoothstep(0.0, _StreakWidth, streakPos) - smoothstep(_StreakWidth, _StreakWidth * 2.0, streakPos);
                col.rgb += _StreakColor.rgb * streak;

                // Apply transparency
                col.a *= _Transparency;
                return col;
            }
            ENDCG
        }
    }

    FallBack "Transparent/Cutout/VertexLit"

}
