#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0
#define PS_SHADERMODEL ps_4_0
#endif

texture PositionSizeLife;
sampler2D PositionSizeLifeSampler = sampler_state
{
	Texture = <PositionSizeLife>;
};

struct VertexShaderInput
{
	float3 Position : SV_POSITION0;
	float2 TexCoord : TEXCOORD0;
};
struct VertexShaderOutput
{
	float4 Position : SV_POSITION0;
    float2 TexCoord : TEXCOORD0;
};

float ElapsedTime;

VertexShaderOutput MainVS(in VertexShaderInput input) {
	VertexShaderOutput output = (VertexShaderOutput)0;
	output.Position = float4(input.Position.xyz, 1);
	output.TexCoord = input.TexCoord;
	return output;
}

float4 MainPS(in VertexShaderOutput input) : COLOR
{
	float4 positionSizeLifeValue = tex2D(PositionSizeLifeSampler, input.TexCoord);
	float2 position = positionSizeLifeValue.xy;
	float size = positionSizeLifeValue.z;
    return float4(position.x, position.y, /*abs(0.01f * cos(ElapsedTime))*/size, positionSizeLifeValue.w); // This is a test, (0, 0)@size of 100
}

technique SpriteDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};