cbuffer globals
{
	matrix finalMatrix;
}

struct VS_IN
{
	float3 pos : POSITION;
	uint2 attr : ATTRIBUTES;
};

struct PS_IN
{
	float4 pos : SV_POSITION;
	float2 uv : TEXCOORD0;
};

struct GS_IN
{
	float3 pos : POSITION;
	uint2 attr : ATTRIBUTES;
};

// Vertex Shader
GS_IN VS( VS_IN input )
{
	GS_IN output;
	
	output.pos = input.pos;
	output.attr = input.attr;
	
	return output;
}

Texture2D yodaTexture;
SamplerState currentSampler
{
	Filter = MIN_MAG_MIP_LINEAR;
	AddressU = Wrap;
	AddressV = Wrap;
};

// Pixel Shader
float4 PS( PS_IN input ) : SV_Target
{
	return yodaTexture.Sample(currentSampler, input.uv);
}

void addFace(inout TriangleStream<PS_IN> OutputStream, float size, float3 left_bottom, float3 right_bottom, float3 right_top, float3 left_top)
{
    PS_IN output;
	output.pos = mul(float4(left_bottom, 1), finalMatrix);
	output.uv = float2(0, size);
	OutputStream.Append( output );

	output.pos = mul(float4(right_bottom, 1), finalMatrix);
	output.uv = float2(size, size);
	OutputStream.Append( output );
	
	output.pos = mul(float4(right_top, 1), finalMatrix);
	output.uv = float2(size, 0);
	OutputStream.Append( output );

	OutputStream.RestartStrip();

	
	output.pos = mul(float4(left_bottom, 1), finalMatrix);
	output.uv = float2(0, size);
	OutputStream.Append( output );

	output.pos = mul(float4(right_top, 1), finalMatrix);
	output.uv = float2(size, 0);
	OutputStream.Append( output );
	
	output.pos = mul(float4(left_top, 1), finalMatrix);
	output.uv = float2(0, 0);
	OutputStream.Append( output );

	OutputStream.RestartStrip();
}

[maxvertexcount(36)]
void GS( point GS_IN input[1], inout TriangleStream<PS_IN> OutputStream )
{	
	float3 _input;
	
	
	_input = input[0].pos;

	float size = input[0].attr.y;

	addFace(OutputStream, size, float3(_input.x, _input.y, _input.z),
					      float3(_input.x + size, _input.y, _input.z),
					      float3(_input.x + size, _input.y + size, _input.z),
					      float3(_input.x, _input.y + size, _input.z));
						  
	addFace(OutputStream, size, float3(_input.x, _input.y, _input.z + size),
					      float3(_input.x, _input.y + size, _input.z + size),
					      float3(_input.x + size, _input.y + size, _input.z + size),
					      float3(_input.x + size, _input.y, _input.z + size));
						  
	addFace(OutputStream, size, float3(_input.x + size, _input.y, _input.z),
					      float3(_input.x + size, _input.y, _input.z + size),
					      float3(_input.x + size, _input.y + size, _input.z + size),
					      float3(_input.x + size, _input.y + size, _input.z));

						  
	addFace(OutputStream, size, float3(_input.x, _input.y, _input.z),
					      float3(_input.x, _input.y + size, _input.z),
					      float3(_input.x, _input.y + size, _input.z + size),
					      float3(_input.x, _input.y, _input.z + size));

	addFace(OutputStream, size, float3(_input.x, _input.y + size, _input.z),
					      float3(_input.x + size, _input.y + size, _input.z),
					      float3(_input.x + size, _input.y + size, _input.z + size),
					      float3(_input.x, _input.y + size, _input.z + size));

	addFace(OutputStream, size, float3(_input.x, _input.y, _input.z),
					      float3(_input.x, _input.y, _input.z + size),
					      float3(_input.x + size, _input.y, _input.z + size),
					      float3(_input.x + size, _input.y, _input.z));
						  
}

// Technique
technique10 Render
{
	pass P0
	{
		SetGeometryShader( CompileShader( gs_4_0, GS() ) );
		SetVertexShader( CompileShader( vs_4_0, VS() ) );
		SetPixelShader( CompileShader( ps_4_0, PS() ) );
	}
}