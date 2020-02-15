#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0
#define PS_SHADERMODEL ps_4_0
#endif

matrix WorldViewProjection;

Texture2D PositionSizeLife;
sampler2D PositionSizeLifeSampler = sampler_state
{
	Texture = <PositionSizeLife>;
};

struct VertexShaderInput
{
    uint ID : TEXCOORD0;
    uint VertexID : TEXCOORD1;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
    float4 Color : COLOR0;
};

int Size;

VertexShaderOutput MainVS(in VertexShaderInput input)
{
    VertexShaderOutput output = (VertexShaderOutput)0;

    int lookupX = input.ID % Size;
    int lookupY = (input.ID - lookupX) / Size;

    float4 positionSizeLifeValue = tex2Dlod(PositionSizeLifeSampler, float4((lookupX) / float(Size), lookupY / float(Size), 0, 0));
    float2 position = positionSizeLifeValue.xy;
    float size = positionSizeLifeValue.z;
    float other = positionSizeLifeValue.w;

    float2 localPos = float2(0, 0);
    switch(input.VertexID) {
    case 0:
        localPos = float2(1, 1) * size;
        output.Color = float4(other, 0, 1, 1);
        break;
    case 1:
        localPos = float2(1, -1) * size;
        output.Color = float4(other, 0, 1, 1);
        break;
    case 2:
        localPos = float2(-1, -1) * size;
        output.Color = float4(other, 0, 1, 1);
        break;
    case 3:
        localPos = float2(-1, 1) * size;
        output.Color = float4(other, 0, 1, 1);
        break;
    }

    output.Position = float4(localPos + position / (input.ID + 1), 0, 1);

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