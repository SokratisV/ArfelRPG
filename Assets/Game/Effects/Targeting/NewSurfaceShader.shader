Shader "Custom/TextureOverlay" {
     Properties {
         _MainTex ("Main Texture (RGBA)", 2D) = "white" {}
         _OverlayTex ("Secondary Texture (RGBA)", 2D) = "white" {}
         _Transparency ("Transparency", Range(0,1)) = 0.85
     }
     SubShader {
         CGPROGRAM
         #pragma surface surf Standard fullforwardshadows 
         #pragma target 3.0
 
         struct Input {
             float2 uv_MainTex;
         };
 
         sampler2D _MainTex;
 
         void surf (Input IN, inout SurfaceOutputStandard o) {
             fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
             o.Albedo = c.rgb;
             o.Alpha = c.a;
         }
         ENDCG
         Blend SrcAlpha OneMinusSrcAlpha
         ZWrite Off
         CGPROGRAM
         #pragma surface surf Standard fullforwardshadows alpha
         #pragma target 3.0
 
         sampler2D _OverlayTex;
         float _Transparency;
 
         struct Input {
             float2 uv_OverlayTex;
         };
 
         void surf (Input IN, inout SurfaceOutputStandard o) {
             fixed4 c = tex2D (_OverlayTex, IN.uv_OverlayTex);
             o.Albedo = c.rgb;
             o.Alpha = _Transparency * c.a;
         }
         ENDCG
     } 
     FallBack "Diffuse"
 }