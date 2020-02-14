#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

matrix WorldViewProjection;

texture PositionSizeLife;
sampler2D PositionSizeLifeSampler = sampler_state
{
	Texture = <PositionSizeLife>;
};

struct VertexShaderInput
{
    uint ID : SAMPLE0;
    uint VertexID : SAMPLE1;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
    float4 Color : COLOR0;
};

int positionSizeLifeWidth;
int positionSizeLifeHeight;

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput)0;

    //output.Position = mul(float4(input.Position.x, input.Position.y, 0, 1), WorldViewProjection);
    //switch(VertexID) {

    //}

    float4 positionSizeLifeValue = tex2Dlod(PositionSizeLifeSampler, float4(0, 0, 0, 0));

    float2 localPos = float2(positionSizeLifeWidth * positionSizeLifeValue.x, positionSizeLifeHeight);
    switch(input.VertexID) {
    case 0:
        localPos = float2(0.5f, 0.5f);
        output.Color = float4(0, 0, 1, 1);
        break;
    case 1:
        localPos = float2(0.5f, -0.5f);
        output.Color = float4(0, 1, 1, 1);
        break;
    case 2:
        localPos = float2(-0.5f, -0.5f);
        output.Color = float4(1, 0, 0, 1);
        break;
    case 3:
        localPos = float2(-0.5f, 0.5f);
        output.Color = float4(0, 1, 0, 1);
        break;
    }

    output.Position = float4(localPos.x, localPos.y, 0, 1);

    return output;
}

float4 MainPS(VertexShaderOutput input) : COLOR
{
	//return float4(1, 1, 1, 1); // Solid white
    return input.Color;
}

technique ParticleDraw
{
	pass P0
	{
        VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};