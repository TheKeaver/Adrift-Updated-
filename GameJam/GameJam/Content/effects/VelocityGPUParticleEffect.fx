#define VS_SHADERMODEL vs_4_0
#define PS_SHADERMODEL ps_4_0

/***
UNIFORMS
***/
texture PositionVelocity;
sampler2D PositionVelocitySampler = sampler_state
{
	Texture = <PositionVelocity>;
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

float ScaleX;
float ScaleY;

float VelocityDecayMultiplier;

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
	float4 dat = tex2D(PositionVelocitySampler, input.TexCoord);
	return tex2D(PositionVelocitySampler, input.TexCoord);
}

/***
UPDATE
***/
float4 UpdatePS(in PassThroughVSOutput input) : COLOR
{
	//float4 positionVelocityValue = tex2D(PositionVelocitySampler, input.TexCoord);
	//float2 position = float2(positionVelocityValue.w, positionVelocityValue.z);
	//float2 velocity = positionVelocityValue.xy;
	/*float angleAroundOrigin = atan2(position.y, position.x);
	angleAroundOrigin = angleAroundOrigin + 0.005f;
	float mag = length(position);
	position = float2(mag * cos(angleAroundOrigin), mag * sin(angleAroundOrigin));*/
	//position = position + velocity * Dt;

    //return float4(position.x, velocity.y, position.y, position.x);
	return tex2D(PositionVelocitySampler, input.TexCoord);
}

/***
DRAW
***/
DrawVSOutput DrawVS(in DrawVSInput input) {
	DrawVSOutput output = (DrawVSOutput)0;

	int id = input.ID;

	int lookupX = id % Size;
	int lookupY = (id - lookupX) / Size;

	float4 positionVelocityValue = tex2Dlod(PositionVelocitySampler, float4(lookupX / float(Size), lookupY / float(Size), 0, 0));
	//float2 position = float2(positionVelocityValue.w, positionVelocityValue.z);
	//float2 velocity = positionVelocityValue.xy;
	float2 position = positionVelocityValue.rg;
	float2 velocity = float2(positionVelocityValue.b, positionVelocityValue.a);
	//float2 velocity = positionVelocityValue.ba;

	output.Color = float4(1, 1, 1, 1);

	float2 localPos = float2(0, 0);
	switch (input.VertexID) {
	case 0:
		localPos = float2(ScaleX / 2, ScaleY / 2);
		break;
	case 1:
		localPos = float2(ScaleX / 2, -ScaleY / 2);
		break;
	case 2:
		localPos = float2(-ScaleX / 2, -ScaleY / 2);
		break;
	case 3:
		localPos = float2(-ScaleX / 2, ScaleY / 2);
		break;
	}

	float rotation = atan2(velocity.y, velocity.x);
	float rotCos = cos(rotation);
	float rotSin = sin(rotation);
	float2 rotatedPos = float2(localPos.x * rotCos - localPos.y * rotSin,
		localPos.x * rotSin + localPos.y * rotCos);

	output.Position = mul(float4(rotatedPos + position, 0, 1), WorldViewProjection);

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