//http://takinginitiative.net/2011/01/12/directx10-tutorial-9-the-geometry-shader/

cbuffer PerFrameBuffer : register(b1)
{
	float4x4 w;
	float4x4 v;
	float4x4 p;
	float4 c;
};

struct GS_OUTPUT
{
	float4 pos : SV_POSITION;
};

struct VS_INPUT
{
	float3 pos : ANCHOR;
};

VS_INPUT VShader(VS_INPUT input)
{

	
	//float4x4 wvp = mul(w, mul(v, p));
	//return  mul(input.pos, wvp);
	return input;
	
}

float4 PShader(float4 position: SV_POSITION) : SV_Target
{
	return c;
}

[maxvertexcount(3)]
void Triangulat0r( point VS_INPUT input[1], inout TriangleStream<GS_OUTPUT> OutputStream )
{	
    GS_OUTPUT output;
	float3 _input;
	
	float4x4 wvp = mul(w, mul(v, p));
	
	_input = input[0].pos;

	output.pos = float4(_input.x + 0.1, _input.y - 0.1, _input.z, 1.0);
    //output.pos = mul(output.pos, wvp);
	OutputStream.Append( output );


	output.pos = float4(_input.x - 0.1, _input.y - 0.1, _input.z, 1.0);
	//output.pos = mul(output.pos, wvp);
	OutputStream.Append( output );

	
	output.pos = float4(_input.x, _input.y + 0.1, _input.z, 1.0);
	//output.pos = mul(output.pos, wvp);
	OutputStream.Append( output );
	
}