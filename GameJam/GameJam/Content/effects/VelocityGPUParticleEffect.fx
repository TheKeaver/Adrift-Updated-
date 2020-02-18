#define VS_SHADERMODEL vs_4_0
#define PS_SHADERMODEL ps_4_0

/***
UNIFORMS
***/
texture PositionSizeRotation;
sampler2D PositionSizeRotationSampler = sampler_state
{
	Texture = <PositionSizeRotation>;
	MipFilter = NONE;
	MinFilter = NONE;
	MagFilter = NONE;
};

texture CreateMask;
sampler2D CreateMaskSampler = sampler_state
{
	Texture = <CreateMask>;
	MipFilter = NONE;
	MinFilter = NONE;
	MagFilter = NONE;
};

float ElapsedTime;
float Dt;
int Size;
matrix WorldViewProjection;

/***
Structs
***/
struct PassThroughVSInput
{
	float3 Position : SV_POSITION0;
	float2 TexCoord : TEXCOORD0;
};
struct PassThroughVSOutput
{
	float4 Position : SV_POSITION0;
    float2 TexCoord : TEXCOORD0;
};

struct DrawVSInput
{
	float ID : TEXCOORD0;
	int VertexID : TEXCOORD1;
};
struct DrawVSOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
};

/***
PASS THROUGH (create & update)
***/
PassThroughVSOutput PassThroughVS(in PassThroughVSInput input) {
	PassThroughVSOutput output = (PassThroughVSOutput)0;
	output.Position = float4(input.Position.xyz, 1);
	output.TexCoord = input.TexCoord;
	return output;
}

/***
CREATE
***/
float4 CreatePS(in PassThroughVSOutput input) : COLOR
{
	if (tex2D(CreateMaskSampler, input.TexCoord).a < 0.5) {
		discard;
	}
	float4 dat = tex2D(PositionSizeRotationSampler, input.TexCoord);
	return float4(dat.x, dat.y, dat.z, dat.w);
}

/***
UPDATE
***/
float4 UpdatePS(in PassThroughVSOutput input) : COLOR
{
	float4 positionSizeRotationValue = tex2D(PositionSizeRotationSampler, input.TexCoord);
	float2 position = positionSizeRotationValue.xy;
	float angleAroundOrigin = atan2(position.y, position.x);
	angleAroundOrigin = angleAroundOrigin + 0.005f;
	float mag = length(position);
	position = float2(mag * cos(angleAroundOrigin), mag * sin(angleAroundOrigin));
	float size = positionSizeRotationValue.z;
    return float4(position.x, position.y, 0.1, positionSizeRotationValue.w);
}

/***
DRAW
***/
DrawVSOutput DrawVS(in DrawVSInput input) {
	DrawVSOutput output = (DrawVSOutput)0;

	int id = input.ID;

	int lookupX = id % Size;
	int lookupY = (id - lookupX) / Size;

	float4 positionSizeLifeRotationValue = tex2Dlod(PositionSizeRotationSampler, float4(lookupX / float(Size), lookupY / float(Size), 0, 0));
	float2 position = positionSizeLifeRotationValue.xy;
	float size = positionSizeLifeRotationValue.z;
	float other = positionSizeLifeRotationValue.w;

	output.Color = float4(abs(position.x), abs(position.y), abs(size) / 0.003f, 1);

	float2 localPos = float2(0, 0);
	switch (input.VertexID) {
	case 0:
		localPos = float2(1, 1) * size;
		break;
	case 1:
		localPos = float2(1, -1) * size;
		break;
	case 2:
		localPos = float2(-1, -1) * size;
		break;
	case 3:
		localPos = float2(-1, 1) * size;
		break;
	}

	output.Position = mul(float4(localPos + position, 0, 1), WorldViewProjection);

	return output;
}
float4 DrawPS(in DrawVSOutput input) : COLOR
{
	return input.Color;
}

technique Create
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL PassThroughVS();
		PixelShader = compile PS_SHADERMODEL CreatePS();
	}
};

technique Update
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL PassThroughVS();
		PixelShader = compile PS_SHADERMODEL UpdatePS();
	}
};

technique Draw {
	pass P0 {
		VertexShader = compile VS_SHADERMODEL DrawVS();
		PixelShader = compile PS_SHADERMODEL DrawPS();
	}
};