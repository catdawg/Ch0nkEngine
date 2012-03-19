//http://takinginitiative.net/2011/01/12/directx10-tutorial-9-the-geometry-shader/
float4x4 World;
float4x4 View;
float4x4 Projection;


struct PS_INPUT
{
	float4 pos : SV_POSITION;
};

float4 VShader(float4 position : POSITION) : SV_POSITION
{
    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    float4 position2 = mul(viewPosition, Projection);

	return position2;
}

float4 PShader(float4 position: SV_POSITION) : SV_Target
{
	return float4(1.0f, 1.0f, 0.0f, 1.0f);
}





[maxvertexcount(6)]
void Triangulat0r( point float4 input [1] :POSITION, inout TriangleStream<PS_INPUT> OutputStream )
{	
    PS_INPUT output;
	float4 _input = input[0];

	

	
	
    output.pos = _input;
	output.pos.x += 0.5;
	output.pos.y -= 0.5;
	OutputStream.Append( output );


    output.pos = _input;
	output.pos.x -= 0.5;
	output.pos.y -= 0.5;

	OutputStream.Append( output );

	
    output.pos = _input;
	output.pos.x += 0.5;
	output.pos.y += 0.5;
	OutputStream.Append( output );


	
    output.pos = _input;
	output.pos.x += 0.5;
	output.pos.y += 0.5;
	OutputStream.Append( output );


    output.pos = _input;
	output.pos.x -= 0.5;
	output.pos.y += 0.5;

	OutputStream.Append( output );

	

    output.pos = _input;
	output.pos.x -= 0.5;
	output.pos.y -= 0.5;

	OutputStream.Append( output );

	
	
    OutputStream.RestartStrip();
}