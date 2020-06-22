#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0
#define PS_SHADERMODEL ps_4_0
#endif

Texture2D SpriteTexture;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

#ifdef XBOX
	#define FXAA_360 1
#else
	#define FXAA_PC 1
#endif
#define FXAA_HLSL_3 1
#define FXAA_GREEN_AS_LUMA 1

#include "Fxaa3_11.fxh"

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float2 TexCoord : TEXCOORD0;
};

float2 InverseViewportSize;
float4 ConsoleSharpness;
float4 ConsoleOpt1;
float4 ConsoleOpt2;
float SubPixelAliasingRemoval;
float EdgeThreshold;
float EdgeThresholdMin;
float ConsoleEdgeSharpness;

float ConsoleEdgeThreshold;
float ConsoleEdgeThresholdMin;

// Must keep this as constant register instead of an immediate
float4 Console360ConstDir = float4(1.0, -1.0, 0.25, -0.25);

float4 PixelShaderFunction_FXAA(in float2 texCoords : TEXCOORD0) : COLOR0
{
	float4 value = FxaaPixelShader(
		texCoords,
		0,	// Not used in PC or Xbox 360
		SpriteTextureSampler,
		SpriteTextureSampler,	// *** TODO: For Xbox, can I use additional sampler with exponent bias of -1
		SpriteTextureSampler,	// *** TODO: For Xbox, can I use additional sampler with exponent bias of -2
		InverseViewportSize,	// FXAA Quality only
		ConsoleSharpness,		// Console only
		ConsoleOpt1,
		ConsoleOpt2,
		SubPixelAliasingRemoval,	// FXAA Quality only
		EdgeThreshold,// FXAA Quality only
		EdgeThresholdMin,
		ConsoleEdgeSharpness,
		ConsoleEdgeThreshold,	// TODO
		ConsoleEdgeThresholdMin, // TODO
		Console360ConstDir
		);

    return value;
}

float4 PixelShaderFunction_Standard(in float2 texCoords : TEXCOORD0) : COLOR0
{
	return tex2D(SpriteTextureSampler, texCoords);
}

technique Standard
{
    pass Pass1
    {
        PixelShader = compile PS_SHADERMODEL PixelShaderFunction_Standard();
    }
}

technique FXAA
{
    pass Pass1
    {
        PixelShader = compile PS_SHADERMODEL PixelShaderFunction_FXAA();
    }
}
