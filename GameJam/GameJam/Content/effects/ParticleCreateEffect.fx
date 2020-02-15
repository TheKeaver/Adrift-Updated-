#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0
#define PS_SHADERMODEL ps_4_0
#endif

struct VertexShaderInput
{
	uint ID : TEXCOORD0;
	uint VertexID : TEXCOORD1;
	float2 ParticlePosition : TEXCOORD2;
};
struct VertexShaderOutput
{
	float4 Position : SV_POSITION0;
	uint ID : TEXCOORD0;
	float2 ParticlePosition : TEXCOORD1;
};

int Size;

VertexShaderOutput MainVS(in VertexShaderInput input) {
	VertexShaderOutput output = (VertexShaderOutput)0;

	int lookupX = input.ID % Size;
	int lookupY = (input.ID - lookupX) / Size;

	const int halfSize = Size / 2;

	float2 pos;
    /*switch (input.VertexID) {
    case 0:
        pos = float2(1.0f / (-halfSize + lookupX), 1.0f / (halfSize - lookupY));
		pos = float2(-1, 1);
        break;
    case 1:
		pos = float2(1.0f / (-halfSize + lookupX + 1), 1.0f / (halfSize - (lookupY + 1)));
		pos = float2(-1 + 1.5f/1024, 1 - 1.5f/1024);
		break;
    }*/
	float idUnit = 1.0f / (1024 / 2.0f);
	const float oth = 2;
	float2 topLeft = float2(-1 - idUnit * oth / 2, 1 + idUnit * oth / 2);
	switch (input.VertexID) {
	case 0:
		//pos = float2(1, 1);
		pos = topLeft + float2(idUnit, 0) * oth;
		break;
	case 1:
		//pos = float2(1, -1);
		pos = topLeft + float2(idUnit, -idUnit) * oth;
		break;
	case 2:
		//pos = float2(-1, -1);
		pos = topLeft + float2(0, -idUnit) * oth;
		break;
	case 3:
		//pos = float2(-1, 1);
		pos = topLeft + float2(0, 0) * oth;
		break;
	}
    output.Position = float4(pos, 0, 1);

	output.ID = input.ID;
	output.ParticlePosition = input.ParticlePosition;
	return output;
}

float4 MainPS(in VertexShaderOutput input) : COLOR
{
	return float4(0.5f, 0.5f, 0.1f, input.ID); // (x, y, size, n/a)
}

technique SpriteDrawing
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL MainVS();
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};