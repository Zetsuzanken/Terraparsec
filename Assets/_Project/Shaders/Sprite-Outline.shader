// Sprite Outline Effect
// Based on an outline shader originally written by Nielson
// (formerly at http://nielson.io/2016/04/2d-sprite-outlines-in-unity/, now unavailable)
// Archived by Mandarinx: https://gist.github.com/mandarinx/f28931faa3c6a5378978a82c84d3dbcd
// Slightly modified by me for my thesis project (Terraparsec), primarily for compatibility with the latest Unity version.

Shader "Sprites/Outline"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
		[HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0

		[PerRendererData] _Outline("Outline", Float) = 0
		[PerRendererData] _OutlineColor("Outline Color", Color) = (1,1,1,1)
		[PerRendererData] _OutlineSize("Outline Size", int) = 1
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex SpriteVert
			#pragma fragment frag
			#pragma target 2.0
			#pragma multi_compile_instancing
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#include "UnitySprites.cginc"

			float _Outline;
			fixed4 _OutlineColor;
			int _OutlineSize;
			float4 _MainTex_TexelSize;

			inline bool IsOutOfRange(float2 uv)
            {
                return (uv.x < 0.0 || uv.x > 1.0 || uv.y < 0.0 || uv.y > 1.0);
            }

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;

				if (_Outline > 0 && c.a != 0)
				{
					float outline = 0.0;

					[unroll(16)]
					for (int i = 1; i <= _OutlineSize; i++)
					{
						float2 uvUp    = IN.texcoord + float2(0,  i * _MainTex_TexelSize.y);
						float2 uvDown  = IN.texcoord - float2(0,  i * _MainTex_TexelSize.y);
						float2 uvRight = IN.texcoord + float2(i * _MainTex_TexelSize.x, 0);
						float2 uvLeft  = IN.texcoord - float2(i * _MainTex_TexelSize.x, 0);

						float alphaUp    = IsOutOfRange(uvUp)    ? 0.0 : tex2D(_MainTex, uvUp).a;
						float alphaDown  = IsOutOfRange(uvDown)  ? 0.0 : tex2D(_MainTex, uvDown).a;
						float alphaRight = IsOutOfRange(uvRight) ? 0.0 : tex2D(_MainTex, uvRight).a;
						float alphaLeft  = IsOutOfRange(uvLeft)  ? 0.0 : tex2D(_MainTex, uvLeft).a;

						float minAlpha = min(min(alphaUp, alphaDown), min(alphaRight, alphaLeft));
						if (minAlpha == 0.0)
						{
							outline = 1.0;
						}
					}

					if (outline > 0.0) 
					{
						c = _OutlineColor; 
					}
				}

				c.rgb *= c.a;
				return c;
			}
		ENDCG
		}
	}
}
