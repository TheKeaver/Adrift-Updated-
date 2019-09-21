#if OPENGL
#define SV_POSITION POSITION
#define VS_SHADERMODEL vs_3_0
#define PS_SHADERMODEL ps_3_0
#else
#define VS_SHADERMODEL vs_4_0_level_9_1
#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

Texture2D SpriteTexture;

sampler2D SpriteTextureSampler = sampler_state
{
	Texture = <SpriteTexture>;
};

struct VertexShaderOutput
{
	float4 Position : SV_POSITION;
	float4 Color : COLOR0;
	float2 TextureCoordinates : TEXCOORD0;
};

float2 direction;
float resolution;
float radius;

float4 MainPS(VertexShaderOutput input) : COLOR
{
	float2 p = input.TextureCoordinates;

	float4 sum = float4(0, 0, 0, 0);

	float blur = radius / resolution;

	float hstep = direction.x;
	float vstep = direction.y;

	// Begin blurring (with predefined gaussian weights)
	// 11 kernel size
	// http://dev.theomader.com/gaussian-kernel-calculator/

	sum += tex2D(SpriteTextureSampler, float2(p.x - 5.0 * blur * hstep, p.y - 5.0 * blur * vstep)) * 0.000003;
	sum += tex2D(SpriteTextureSampler, float2(p.x - 4.0 * blur * hstep, p.y - 4.0 * blur * vstep)) * 0.000229;
	sum += tex2D(SpriteTextureSampler, float2(p.x - 3.0 * blur * hstep, p.y - 3.0 * blur * vstep)) * 0.005977;
	sum += tex2D(SpriteTextureSampler, float2(p.x - 2.0 * blur * hstep, p.y - 2.0 * blur * vstep)) * 0.060598;
	sum += tex2D(SpriteTextureSampler, float2(p.x - 1.0 * blur * hstep, p.y - 1.0 * blur * vstep)) * 0.24173;

	sum += tex2D(SpriteTextureSampler, p) * 0.382925;

	sum += tex2D(SpriteTextureSampler, float2(p.x + 1.0 * blur * hstep, p.y + 1.0 * blur * vstep)) * 0.24173;
	sum += tex2D(SpriteTextureSampler, float2(p.x + 2.0 * blur * hstep, p.y + 2.0 * blur * vstep)) * 0.060598;
	sum += tex2D(SpriteTextureSampler, float2(p.x + 3.0 * blur * hstep, p.y + 3.0 * blur * vstep)) * 0.005977;
	sum += tex2D(SpriteTextureSampler, float2(p.x + 4.0 * blur * hstep, p.y + 4.0 * blur * vstep)) * 0.000229;
	sum += tex2D(SpriteTextureSampler, float2(p.x + 5.0 * blur * hstep, p.y + 5.0 * blur * vstep)) * 0.000003;

	return input.Color * float4(sum.rgb, tex2D(SpriteTextureSampler, p).a);
}

technique SpriteDrawing
{
	pass P0
	{
		PixelShader = compile PS_SHADERMODEL MainPS();
	}
};