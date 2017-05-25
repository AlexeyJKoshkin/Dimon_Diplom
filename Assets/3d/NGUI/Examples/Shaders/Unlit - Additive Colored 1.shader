Shader "Unlit/Bats Shader"
{
	Properties
	{
		_MainTex ("Base (RGB), Alpha (A)", 2D) = "black" {}
	}
	
	SubShader
	{

		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		
		LOD 100
		Cull Off
		Lighting Off
		ZWrite On
		Fog { Mode Off }
		//ColorMask RGB
		//AlphaTest Greater .01
		Blend SrcAlpha OneMinusSrcAlpha

		Pass
		{
			ColorMaterial AmbientAndDiffuse
			
			SetTexture [_MainTex]
			{
				Combine Texture * Primary
			}
		}
	}
}
