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
texture StaticInfo;
sampler2D StaticInfoSampler = sampler_state
{
	Texture = <StaticInfo>;
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

struct CreateVSInput {
	float ID : TEXCOORD0;
	int VertexID : TEXCOORD1;
	float2 Position : POSITION0;
	float2 Velocity : TEXCOORD2;
	float4 Color : COLOR0;
};
struct CreateVSOutput {
	float4 Position : SV_POSITION;
	float2 ParticlePosition : POSITION1;
	float2 Velocity : TEXCOORD0;
	float4 Color : COLOR0;
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
PASS THROUGH (update)
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
CreateVSOutput CreateVS(in CreateVSInput input) {
	CreateVSOutput output = (CreateVSOutput)0;

	int id = input.ID;

	int lookupX = id % Size;
	int lookupY = (id - lookupX) / Size;

	float2 pixelPos = float2(0, 0);

	float testSize = 2.0f / Size;

	float2 localPos = float2(0, 0);
	switch (input.VertexID) {
	case 0:
		localPos = float2(1.0f / Size, 1.0f / Size);
		break;
	case 1:
		localPos = float2(1.0f / Size, -1.0f / Size);
		break;
	case 2:
		localPos = float2(-1.0f / Size, -1.0f / Size);
		break;
	case 3:
		localPos = float2(-1.0f / Size, 1.0f / Size);
		break;
	}

	float halfSize = 1 / Size / 2;
	float2 pixelPosition = float2(lookupX / halfSize - 1, lookupY / halfSize - 1);
	//output.Position = float4(pixelPosition + localPos * size - float2(1 / size / 2, 1 / size / 2), 0, 1); OLD
	//output.Position = float4(pixelPosition + localPos * halfSize * 10, 0, 1);
	output.Position = float4(float2(testSize * lookupX + testSize / 2 - 1, 1 - testSize * lookupY - testSize / 2) + localPos, 0, 1);

	// Pass through
	output.ParticlePosition = input.Position;
	output.Velocity = input.Velocity;
	output.Color = input.Color;

	return output;
}
float4 CreatePS(in CreateVSOutput input) : COLOR
{
	return float4(input.ParticlePosition.x, input.ParticlePosition.y, input.Velocity.x, input.Velocity.y);
}
float4 CreateStaticPS(in CreateVSOutput input) : COLOR
{
	return float4(input.Color.rgb, 1);
}

/***
UPDATE
***/
float4 UpdatePS(in PassThroughVSOutput input) : COLOR
{
	float4 positionVelocityValue = tex2D(PositionVelocitySampler, input.TexCoord);
	float2 position = positionVelocityValue.xy;
	float2 velocity = positionVelocityValue.zw;

	position = position + velocity * Dt;
	velocity = velocity * pow(VelocityDecayMultiplier, Dt * 144); // Decay multiplier was determined at 144Hz, this fixes the issue of explosion size being different on different refresh rates

    return float4(position.x, position.y, velocity.x, velocity.y);
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
	float2 position = positionVelocityValue.xy;
	float2 velocity = positionVelocityValue.zw;

	float4 staticInfoValue = tex2Dlod(StaticInfoSampler, float4(lookupX / float(Size), lookupY / float(Size), 0, 0));
	float3 color = staticInfoValue.rgb;

	float Speed = length(velocity);

	float Alpha = min(1, Speed);
	Alpha = Alpha * Alpha;
	output.Color = float4(color, Alpha);

	float Stretch = Speed * 0.003;

	float2 localPos = float2(0, 0);
	switch (input.VertexID) {
	case 0:
		localPos = float2(ScaleX / 2 * Stretch, ScaleY / 2);
		break;
	case 1:
		localPos = float2(ScaleX / 2 * Stretch, -ScaleY / 2);
		break;
	case 2:
		localPos = float2(-ScaleX / 2 * Stretch, -ScaleY / 2);
		break;
	case 3:
		localPos = float2(-ScaleX / 2 * Stretch, ScaleY / 2);
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
		VertexShader = compile VS_SHADERMODEL CreateVS();
		PixelShader = compile PS_SHADERMODEL CreatePS();
	}
};

technique CreateStatic
{
	pass P0
	{
		VertexShader = compile VS_SHADERMODEL CreateVS();
		PixelShader = compile PS_SHADERMODEL CreateStaticPS();
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