// Anime Particle Shader V1.1
// By Zer0#9999
Shader "!Zer0/AnimeParticlesV1.1"
{
	Properties
	{
		[Header((Zer0s Anime Particle Shader V1))]
		[Header(__Layer 1__)]
		[Header(Texture 1)]
		_Texture1("Texture 1", 2D) = "white" {}
		[Toggle]_Texture1Color("Texture 1 Color", Float) = 1
		_Texture1Strength("Texture 1 Strength", Float) = 1
		_Texture1Rotation("Texture 1 Rotation", Range( -5 , 5)) = 0
		_Texture1X("Texture 1 X", Range( -10 , 10)) = 0
		_Texture1Y("Texture 1 Y", Range( -10 , 10)) = 0
		[Header(Distortion 1)]
		_Distortion1("Distortion 1", 2D) = "bump" {}
		_Distortion1Strength("Distortion 1 Strength", Float) = 0
		_Distortion1X("Distortion 1 X", Range( -10 , 10)) = 0
		_Distortion1Y("Distortion 1 Y", Range( -10 , 10)) = 0
		[Header(__Layer 2__)]
		[Header(Texture 2)]
		_Texture2("Texture 2", 2D) = "white" {}
		[Toggle]_Texture2Color("Texture 2 Color", Float) = 1
		_Texture2Strength("Texture 2 Strength", Float) = 0
		_Texture2Rotation("Texture 2 Rotation", Range( -5 , 5)) = 0
		_Texture2X("Texture 2 X", Range( -10 , 10)) = 0
		_Texture2Y("Texture 2 Y", Range( -10 , 10)) = 0
		[Header(Distortion 2)]
		_Distortion2("Distortion 2", 2D) = "bump" {}
		_Distortion2Strength("Distortion 2 Strength", Float) = 0
		_Distortion2X("Distortion 2 X", Range( -10 , 10)) = 0
		_Distortion2Y("Distortion 2 Y", Range( -10 , 10)) = 0
		[Header(__Layer 3__)]
		[Header(Texture 3)]
		_Texture3("Texture 3", 2D) = "white" {}
		[Toggle]_Texture3Color("Texture 3 Color", Float) = 1
		_Texture3Strength("Texture 3 Strength", Float) = 0
		_Texture3Rotation("Texture 3 Rotation", Range( -5 , 5)) = 0
		_Texture3X("Texture 3 X", Range( -10 , 10)) = 0
		_Texture3Y("Texture 3 Y", Range( -10 , 10)) = 0
		[Header(Distortion 3)]
		_Distortion3("Distortion 3", 2D) = "bump" {}
		_Distortion3Strength("Distortion 3 Strength", Float) = 0
		_Distortion3X("Distortion 3 X", Range( -10 , 10)) = 0
		_Distortion3Y("Distortion 3 Y", Range( -10 , 10)) = 0
		[Header(__Masking Layer__)]
		[Header(Mask Texture)]
		_Mask("Mask", 2D) = "white" {}
		_MaskRotation("Mask Rotation", Range( -5 , 5)) = 0
		_MaskX("Mask X", Range( -10 , 10)) = 0
		_MaskY("Mask Y", Range( -10 , 10)) = 0
		[Header(Mask Distortion)]
		_MaskDistortion("MaskDistortion", 2D) = "white" {}
		_MaskDistortionStrength("MaskDistortionStrength", Float) = 0
		_MaskDisortionY("Mask Disortion Y", Range( -10 , 10)) = 0
		_MaskDisortionX("Mask Disortion X", Range( -10 , 10)) = 0
		[Header(Blending Options)]
		_AlphaBlend("Alpha Blend", Float) = 3
		_Additive("Additive", Float) = 3
		_EmissionBloom("Emission (Bloom)", Float) = 1
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#include "UnityStandardUtils.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
			float4 vertexColor : COLOR;
		};

		uniform float _Texture1Color;
		uniform sampler2D _Texture1;
		uniform float _Texture1X;
		uniform float _Texture1Y;
		uniform float4 _Texture1_ST;
		uniform float _Texture1Rotation;
		uniform sampler2D _Distortion1;
		uniform float _Distortion1X;
		uniform float _Distortion1Y;
		uniform float4 _Distortion1_ST;
		uniform float _Distortion1Strength;
		uniform float _Texture1Strength;
		uniform float _Texture2Color;
		uniform sampler2D _Texture2;
		uniform sampler2D _Distortion2;
		uniform float _Distortion2X;
		uniform float _Distortion2Y;
		uniform float4 _Distortion2_ST;
		uniform float _Distortion2Strength;
		uniform float _Texture2X;
		uniform float _Texture2Y;
		uniform float4 _Texture2_ST;
		uniform float _Texture2Rotation;
		uniform float _Texture2Strength;
		uniform float _Texture3Color;
		uniform sampler2D _Texture3;
		uniform sampler2D _Distortion3;
		uniform float _Distortion3X;
		uniform float _Distortion3Y;
		uniform float _Distortion3Strength;
		uniform float _Texture3X;
		uniform float _Texture3Y;
		uniform float4 _Texture3_ST;
		uniform float _Texture3Rotation;
		uniform float _Texture3Strength;
		uniform float _Additive;
		uniform sampler2D _Mask;
		uniform float _MaskX;
		uniform float _MaskY;
		uniform float4 _Mask_ST;
		uniform float _MaskRotation;
		uniform sampler2D _MaskDistortion;
		uniform float _MaskDisortionX;
		uniform float _MaskDisortionY;
		uniform float4 _MaskDistortion_ST;
		uniform float _MaskDistortionStrength;
		uniform float _EmissionBloom;
		uniform float _AlphaBlend;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float mulTime189 = _Time.y * 0.1;
			float4 appendResult188 = (float4(_Texture1X , _Texture1Y , 0.0 , 0.0));
			float2 uv_Texture1 = i.uv_texcoord * _Texture1_ST.xy + _Texture1_ST.zw;
			float mulTime271 = _Time.y * _Texture1Rotation;
			float cos275 = cos( mulTime271 );
			float sin275 = sin( mulTime271 );
			float2 rotator275 = mul( uv_Texture1 - float2( 0.5,0.5 ) , float2x2( cos275 , -sin275 , sin275 , cos275 )) + float2( 0.5,0.5 );
			float2 panner193 = ( mulTime189 * appendResult188.xy + rotator275);
			float mulTime67 = _Time.y * 0.1;
			float4 appendResult69 = (float4(_Distortion1X , _Distortion1Y , 0.0 , 0.0));
			float2 uv_Distortion1 = i.uv_texcoord * _Distortion1_ST.xy + _Distortion1_ST.zw;
			float2 panner70 = ( mulTime67 * appendResult69.xy + uv_Distortion1);
			float3 tex2DNode61 = UnpackScaleNormal( tex2D( _Distortion1, panner70 ), 0.98 );
			float4 appendResult78 = (float4(( tex2DNode61.r * _Distortion1Strength ) , ( tex2DNode61.g * _Distortion1Strength ) , 0.0 , 0.0));
			float4 temp_output_390_0 = ( tex2D( _Texture1, ( float4( panner193, 0.0 , 0.0 ) + appendResult78 ).xy ) * i.vertexColor * _Texture1Strength );
			float grayscale398 = Luminance(temp_output_390_0.rgb);
			float4 temp_cast_5 = (grayscale398).xxxx;
			float mulTime184 = _Time.y * 0.1;
			float4 appendResult183 = (float4(_Distortion2X , _Distortion2Y , 0.0 , 0.0));
			float2 uv_Distortion2 = i.uv_texcoord * _Distortion2_ST.xy + _Distortion2_ST.zw;
			float2 panner186 = ( mulTime184 * appendResult183.xy + uv_Distortion2);
			float3 tex2DNode174 = UnpackScaleNormal( tex2D( _Distortion2, panner186 ), 0.98 );
			float4 appendResult177 = (float4(( tex2DNode174.r * _Distortion2Strength ) , ( tex2DNode174.g * _Distortion2Strength ) , 0.0 , 0.0));
			float mulTime197 = _Time.y * 0.1;
			float4 appendResult198 = (float4(_Texture2X , _Texture2Y , 0.0 , 0.0));
			float2 uv_Texture2 = i.uv_texcoord * _Texture2_ST.xy + _Texture2_ST.zw;
			float mulTime267 = _Time.y * _Texture2Rotation;
			float cos269 = cos( mulTime267 );
			float sin269 = sin( mulTime267 );
			float2 rotator269 = mul( uv_Texture2 - float2( 0.5,0.5 ) , float2x2( cos269 , -sin269 , sin269 , cos269 )) + float2( 0.5,0.5 );
			float2 panner200 = ( mulTime197 * appendResult198.xy + rotator269);
			float4 temp_output_406_0 = ( tex2D( _Texture2, ( appendResult177 + float4( panner200, 0.0 , 0.0 ) ).xy ) * i.vertexColor * _Texture2Strength );
			float grayscale407 = Luminance(temp_output_406_0.rgb);
			float4 temp_cast_13 = (grayscale407).xxxx;
			float mulTime242 = _Time.y * 0.1;
			float4 appendResult243 = (float4(_Distortion3X , _Distortion3Y , 0.0 , 0.0));
			float2 panner244 = ( mulTime242 * appendResult243.xy + uv_Distortion2);
			float3 tex2DNode248 = UnpackScaleNormal( tex2D( _Distortion3, panner244 ), 0.98 );
			float4 appendResult256 = (float4(( tex2DNode248.r * _Distortion3Strength ) , ( tex2DNode248.g * _Distortion3Strength ) , 0.0 , 0.0));
			float mulTime251 = _Time.y * 0.1;
			float4 appendResult252 = (float4(_Texture3X , _Texture3Y , 0.0 , 0.0));
			float2 uv_Texture3 = i.uv_texcoord * _Texture3_ST.xy + _Texture3_ST.zw;
			float mulTime265 = _Time.y * _Texture3Rotation;
			float cos260 = cos( mulTime265 );
			float sin260 = sin( mulTime265 );
			float2 rotator260 = mul( uv_Texture3 - float2( 0.5,0.5 ) , float2x2( cos260 , -sin260 , sin260 , cos260 )) + float2( 0.5,0.5 );
			float2 panner257 = ( mulTime251 * appendResult252.xy + rotator260);
			float4 temp_output_411_0 = ( tex2D( _Texture3, ( appendResult256 + float4( panner257, 0.0 , 0.0 ) ).xy ) * i.vertexColor * _Texture3Strength );
			float grayscale412 = Luminance(temp_output_411_0.rgb);
			float4 temp_cast_21 = (grayscale412).xxxx;
			float4 temp_output_396_0 = ( lerp(temp_cast_5,temp_output_390_0,_Texture1Color) + lerp(temp_cast_13,temp_output_406_0,_Texture2Color) + lerp(temp_cast_21,temp_output_411_0,_Texture3Color) );
			float mulTime226 = _Time.y * 0.1;
			float4 appendResult225 = (float4(_MaskX , _MaskY , 0.0 , 0.0));
			float2 uv_Mask = i.uv_texcoord * _Mask_ST.xy + _Mask_ST.zw;
			float mulTime277 = _Time.y * _MaskRotation;
			float cos280 = cos( mulTime277 );
			float sin280 = sin( mulTime277 );
			float2 rotator280 = mul( uv_Mask - float2( 0.5,0.5 ) , float2x2( cos280 , -sin280 , sin280 , cos280 )) + float2( 0.5,0.5 );
			float2 panner228 = ( mulTime226 * appendResult225.xy + rotator280);
			float mulTime212 = _Time.y * 0.1;
			float4 appendResult211 = (float4(_MaskDisortionX , _MaskDisortionY , 0.0 , 0.0));
			float2 uv_MaskDistortion = i.uv_texcoord * _MaskDistortion_ST.xy + _MaskDistortion_ST.zw;
			float2 panner214 = ( mulTime212 * appendResult211.xy + uv_MaskDistortion);
			float3 tex2DNode215 = UnpackScaleNormal( tex2D( _MaskDistortion, panner214 ), 0.98 );
			float4 appendResult219 = (float4(( tex2DNode215.r * _MaskDistortionStrength ) , ( tex2DNode215.g * _MaskDistortionStrength ) , 0.0 , 0.0));
			float4 tex2DNode208 = tex2D( _Mask, ( float4( panner228, 0.0 , 0.0 ) + appendResult219 ).xy );
			o.Emission = ( saturate( ( temp_output_396_0 * _Additive * tex2DNode208 * i.vertexColor.a ) ) * _EmissionBloom ).rgb;
			float4 temp_cast_29 = (grayscale398).xxxx;
			float4 temp_cast_32 = (grayscale407).xxxx;
			float4 temp_cast_35 = (grayscale412).xxxx;
			float grayscale419 = dot(temp_output_396_0.rgb, float3(0.299,0.587,0.114));
			o.Alpha = saturate( ( i.vertexColor.a * _AlphaBlend * grayscale419 * tex2DNode208 ) ).r;
		}

		ENDCG
	}
}