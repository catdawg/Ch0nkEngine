cbuffer globals
{
	matrix finalMatrix;
}

struct VS_IN
{
	float3 pos : POSITION;
};

struct PS_IN
{
	float4 pos : SV_POSITION;
	float2 uv : TEXCOORD0;
};

struct GS_IN
{
	float3 pos : POSITION;
};

// Vertex Shader
GS_IN VS( VS_IN input )
{
	GS_IN output = (GS_IN)0;
	
	output.pos = input.pos;
	
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

[maxvertexcount(3)]
void GS( point GS_IN input[1], inout TriangleStream<PS_IN> OutputStream )
{	
    PS_IN output;
	float3 _input;
	
	
	_input = input[0].pos;

	output.pos = mul(float4(_input.x + 0.1, _input.y - 0.1, _input.z , 1), finalMatrix);
	output.uv = float2(1, 1);
	OutputStream.Append( output );
	
	output.pos = mul(float4(_input.x - 0.1, _input.y - 0.1, _input.z , 1), finalMatrix);
	output.uv = float2(0, 1);
	OutputStream.Append( output );
	
	output.pos = mul(float4(_input.x, _input.y + 0.1, _input.z , 1), finalMatrix);
	output.uv = float2(0.5, 0);
	OutputStream.Append( output );

	
	
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