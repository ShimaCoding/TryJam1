Shader "Custom/Unlit Emissive" {
    Properties {
        _Color ("Main Color", Color) = (1,1,1,1)
        _MainTex ("Albedo", 2D) = "white" {}
		[HDR] _EmissionColor ("Emission Color", Color) = (0,0,0)
    }
    Category {
       Lighting Off
       ZWrite On
       Cull Back
       SubShader {
            Pass {
               SetTexture [_MainTex] {
                    constantColor [_Color]
                    Combine texture * constant, texture * constant
                 }
            }
        }
    }
}