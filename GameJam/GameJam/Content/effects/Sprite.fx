#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;
matrix WorldViewProjection;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderInput
{
	float3 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};
struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

VertexShaderOutput MainVS(VertexShaderInput input) {
	VertexShaderOutput output = (VertexShaderOutput)0;

	output.Position = mul(float4(input.Position, 1), WorldViewProjection);
	output.Color = input.Color;
	output.TextureCoordinates = input.TextureCoordinates;

	return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	return tex2D(SpriteTextureSampler, input.TextureCoordinates) * input.Color;
}

technique SpriteDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};