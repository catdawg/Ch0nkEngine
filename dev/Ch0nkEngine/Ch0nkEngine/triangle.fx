//http://takinginitiative.net/2011/01/12/directx10-tutorial-9-the-geometry-shader/

cbuffer PerFrameBuffer : register(b0)
{
	float4x4 wvp;
	float4 c;
};

struct GS_OUTPUT
{
	float4 pos : SV_POSITION;
};

struct VS_INPUT
{
	float4 pos : POSITION;
};

GS_OUTPUT VShader(VS_INPUT input)
{

	
    GS_OUTPUT output;
	//input.pos.w = 1.0f;
	output.pos = mul(input.pos, wvp);
	//output.pos = input.pos;
	return  output;
	//return input;
	
}

float4 PShader(float4 position: SV_POSITION) : SV_Target
{
	return c;
}

[maxvertexcount(3)]
void Triangulat0r( point VS_INPUT input[1], inout TriangleStream<GS_OUTPUT> OutputStream )
{	
    GS_OUTPUT output;
	float4 _input;
	
	
	_input = input[0].pos;

	output.pos = float4(_input.x + 0.1, _input.y - 0.1, _input.z, 1);
    output.pos = mul(output.pos, wvp);
	OutputStream.Append( output );

	output.pos = float4(_input.x - 0.1, _input.y - 0.1, _input.z, 1);
    output.pos = mul(output.pos, wvp);
	OutputStream.Append( output );
	
	output.pos = float4(_input.x, _input.y + 0.1, _input.z, 1);
    output.pos = mul(output.pos, wvp);
	OutputStream.Append( output );

	
	
}

technique10 Render
{
	pass P0
	{
		SetGeometryShader( 0 );
		SetVertexShader( CompileShader( vs_4_0, VShader() ) );
		SetPixelShader( CompileShader( ps_4_0, PShader() ) );
	}
}