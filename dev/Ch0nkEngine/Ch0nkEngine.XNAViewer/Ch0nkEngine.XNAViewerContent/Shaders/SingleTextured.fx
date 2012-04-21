float4x4 World;
float4x4 View;
float4x4 Projection;
float4x4 ViewProjectionMatrix;

float3 DiffuseLightDirection = float3(1, 1, -1);
float4 DiffuseColor = float4(1, 1, 1, 1);
float DiffuseIntensity = 0.3;


// TODO: add effect parameters here.

struct VertexShaderInput
{
    float4 Position : POSITION0;
	float2 TextureCoordinate : TEXCOORD0;
	float4 Normal : NORMAL0;

    // TODO: add input channels such as texture
    // coordinates and vertex colors here.
};

struct VertexShaderOutput
{
    float4 Position : POSITION0;
	float2 TextureCoordinate : TEXCOORD0;
	float4 Color : COLOR0;

    // TODO: add vertex shader outputs such as colors and texture
    // coordinates here. These values will automatically be interpolated
    // over the triangle, and provided as input to your pixel shader.
};


texture Texture0;
sampler2D Texture0Sampler = sampler_state {
    Texture = (Texture0);
	

    MagFilter = Anisotropic;
    MinFilter = Anisotropic;
    AddressU = Wrap;
    AddressV = Wrap;
	
};


//===============================================================================
// Normal
//===============================================================================

VertexShaderOutput VertexShaderFunction(VertexShaderInput input)
{
    VertexShaderOutput output;

    /*float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
    output.Position = mul(viewPosition, Projection);*/
	output.Position = mul(input.Position, ViewProjectionMatrix);

    // TODO: add your vertex shader code here.
	output.TextureCoordinate = input.TextureCoordinate;
	
	float lightIntensity = dot(input.Normal, -DiffuseLightDirection);
    output.Color = saturate(DiffuseColor * lightIntensity * DiffuseIntensity);

	return output;
}

float4 PixelShaderFunction(VertexShaderOutput input) : COLOR0
{
    // TODO: add your pixel shader code here.
    //return tex2D(Texture0Sampler, input.TextureCoordinate);
	return saturate(input.Color + tex2D(Texture0Sampler, input.TextureCoordinate)*0.8);
	//return float4(0,1,1,1);
}

technique SingleTextured
{
    pass Pass1
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_2_0 VertexShaderFunction();
        PixelShader = compile ps_2_0 PixelShaderFunction();
    }
}

//===============================================================================
// Instancing
//===============================================================================

VertexShaderOutput InstancingVS(VertexShaderInput input, float4 instanceData : TEXCOORD1)
{
	VertexShaderOutput output;

	//gets the xyz position from the instanceData xyz coordinates
	//and gets the size of the block from the w coordinate
	float4 instancePosition = instanceData;
	float4 scaledPosition = input.Position * instanceData.w;
	instancePosition.w = scaledPosition.w = 1;

	float4 worldPosition = scaledPosition + instancePosition;
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);
	output.TextureCoordinate = input.TextureCoordinate * instanceData.w;
	
	float lightIntensity = dot(input.Normal, -DiffuseLightDirection);
    output.Color = saturate(DiffuseColor * lightIntensity*DiffuseIntensity);

	return output;
}
 
float4 InstancingPS(VertexShaderOutput input) : COLOR0
{
    return saturate(input.Color + tex2D(Texture0Sampler, input.TextureCoordinate));
}

technique SingleTexturedInstancing
{
	pass Pass0
	{
		VertexShader = compile vs_3_0 InstancingVS();
		PixelShader = compile ps_3_0 InstancingPS();
	}
}

//===============================================================================
// ShadowMap
//===============================================================================

struct VertexShaderShadowMapOutput
{
    float4 Position     : POSITION;
    float4 Position2D    : TEXCOORD0;
};

VertexShaderShadowMapOutput VertexShaderShadowMapFunction(VertexShaderInput input)
{
    VertexShaderShadowMapOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);

    output.Position = mul(viewPosition, Projection);
	output.Position2D = output.Position;

	return output;
}

float4 PixelShaderShadowMapFunction(VertexShaderShadowMapOutput input) : COLOR0
{
	//this is equivalent to the depth, with values between 0 and 1 (between near clip Plane and far clip Plane)
	//return input.Position2D.z/input.Position2D.w;
	float4 color = input.Position2D.z/input.Position2D.w;
	color.a = 1;
	return color;
}

technique ShadowMap
{
    pass Pass0
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_2_0 VertexShaderShadowMapFunction();
        PixelShader = compile ps_2_0 PixelShaderShadowMapFunction();
    }
}

//===============================================================================
// Shadowed
//===============================================================================

Texture ShadowMapTexture;

sampler ShadowMapSampler = sampler_state { texture = <ShadowMapTexture> ;magfilter = Anisotropic; minfilter = Anisotropic; mipfilter=LINEAR; AddressU = clamp; AddressV = clamp;};


float4x4 LightViewProjection;

/*
For edge detection

Texture DepthTexture;

sampler DepthTextureSampler = sampler_state { texture = <DepthTexture> ; magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = clamp; AddressV = clamp;};

float NormalThreshold = 0.5;
float DepthThreshold = 0.1;
float NormalSensitivity = 1;
float DepthSensitivity = 10;
float EdgeIntensity = 1;*/

struct VertexShaderShadowOutput
{
    float4 Position     : POSITION;
    float4 Pos2DAsSeenByLight    : TEXCOORD0;
	float2 TextureCoordinate : TEXCOORD1; // this is new
	//float4 Position3D : TEXCOORD2; // this is new
	float4 Color : COLOR0; // this is new
};

VertexShaderShadowOutput VertexShaderShadowFunction(VertexShaderInput input)
{
    VertexShaderShadowOutput output;

	//The matrices created using the directx functions have the bottom row for the translation vector rather than the right hand column. 
	//This means that when you want to make transformations that are relative to the previous transform frame you place the new transform matrix on the left of the previous instead of the right. 
    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);

    output.Position = mul(viewPosition, Projection);
	output.Pos2DAsSeenByLight = mul(input.Position, LightViewProjection);

	output.TextureCoordinate = input.TextureCoordinate;
	//output.Position3D = output.Position;
	
	float lightIntensity = dot(input.Normal, -DiffuseLightDirection);
    output.Color = saturate(DiffuseColor * lightIntensity*DiffuseIntensity);

	return output;
}

float4 PixelShaderShadowFunction(VertexShaderShadowOutput input) : COLOR0
{
	float2 ProjectedTexCoords;
	ProjectedTexCoords[0] = input.Pos2DAsSeenByLight.x/input.Pos2DAsSeenByLight.w/2.0f +0.5f;
    ProjectedTexCoords[1] = -input.Pos2DAsSeenByLight.y/input.Pos2DAsSeenByLight.w/2.0f +0.5f;
	
	float lightIntensity = 0;

	if ((saturate(ProjectedTexCoords).x == ProjectedTexCoords.x) && (saturate(ProjectedTexCoords).y == ProjectedTexCoords.y))
	{
		float depthStoredInShadowMap = tex2D(ShadowMapSampler, ProjectedTexCoords).r;
        float realDistance = input.Pos2DAsSeenByLight.z/input.Pos2DAsSeenByLight.w;
        if ((realDistance - 1.0f/100.0f) <= depthStoredInShadowMap)
        {
			lightIntensity = 1;
		}
	}
	else
	{
		lightIntensity = 1;
	}

	//return tex2D(ShadowMapSampler, ProjectedTexCoords).r;

	/*float2 coordXY  = input.Position3D.xy  / input.Position3D.w;
	coordXY[0] = input.Position3D.x/input.Position3D.w/2.0f +0.5f;
    coordXY[1] = -input.Position3D.y/input.Position3D.w/2.0f +0.5f;

	float edgeOffset = 0.001;

	float4 n0 = tex2D(DepthTextureSampler,coordXY);
	float4 n1 = tex2D(DepthTextureSampler, coordXY + float2(-1, -1) * edgeOffset);
    float4 n2 = tex2D(DepthTextureSampler, coordXY + float2( 1,  1) * edgeOffset);
    float4 n3 = tex2D(DepthTextureSampler, coordXY + float2(-1,  1) * edgeOffset);
    float4 n4 = tex2D(DepthTextureSampler, coordXY + float2( 1, -1) * edgeOffset);

	
	// Work out how much the normal and depth values are changing.
    float4 diagonalDelta = abs(n1 - n2) + abs(n3 - n4);

    float normalDelta = dot(diagonalDelta.xyz, 1);
    float depthDelta = diagonalDelta.w;
        
    // Filter out very small changes, in order to produce nice clean results.
    normalDelta = saturate((normalDelta - NormalThreshold) * NormalSensitivity);
    depthDelta = saturate((depthDelta - DepthThreshold) * DepthSensitivity);

    // Does this pixel lie on an edge?
    float edgeAmount = saturate(normalDelta + depthDelta) * EdgeIntensity;
	
	return saturate(input.Color*lightIntensity + tex2D(Texture0Sampler, input.TextureCoordinate)*0.8)*(1-edgeAmount);
	*/
	return lightIntensity;
	//return saturate(input.Color*lightIntensity + tex2D(Texture0Sampler, input.TextureCoordinate)*0.8);
}

technique Shadow
{
    pass Pass0
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_2_0 VertexShaderShadowFunction();
        PixelShader = compile ps_2_0 PixelShaderShadowFunction();
    }
}



//===============================================================================
// Shadowed
//===============================================================================

Texture ShadowTexture;
sampler ShadowSampler = sampler_state { texture = <ShadowTexture> ;magfilter = LINEAR; minfilter = LINEAR; mipfilter=LINEAR; AddressU = clamp; AddressV = clamp;};


struct VertexShaderShadowedOutput
{
    float4 Position     : POSITION;
	float2 TextureCoordinate : TEXCOORD1; // this is new
	float4 Position3D : TEXCOORD2; // this is new
	float4 Color : COLOR0; // this is new
};

VertexShaderShadowedOutput VertexShaderShadowedFunction(VertexShaderInput input)
{
    VertexShaderShadowedOutput output;

	//The matrices created using the directx functions have the bottom row for the translation vector rather than the right hand column. 
	//This means that when you want to make transformations that are relative to the previous transform frame you place the new transform matrix on the left of the previous instead of the right. 
    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);

    output.TextureCoordinate = input.TextureCoordinate;
	output.Position3D = output.Position;
	
	float lightIntensity = dot(input.Normal, -DiffuseLightDirection);
    output.Color = saturate(DiffuseColor * lightIntensity*DiffuseIntensity);

	return output;
}

float4 PixelShaderShadowedFunction(VertexShaderShadowedOutput input) : COLOR0
{
	float2 ProjectedTexCoords;
	ProjectedTexCoords[0] = input.Position3D.x/input.Position3D.w/2.0f +0.5f;
    ProjectedTexCoords[1] = -input.Position3D.y/input.Position3D.w/2.0f +0.5f;
	
	float lightIntensity = 1;

	//lightIntensity = tex2D(ShadowSampler, ProjectedTexCoords).r;

	if (tex2D(ShadowSampler, ProjectedTexCoords).r < 0.5)
	{
		lightIntensity = tex2D(ShadowSampler, ProjectedTexCoords).r;	
	}
	
	//return tex2D(ShadowMapSampler, ProjectedTexCoords).r;

	//return lightIntensity;
	float4 color = saturate(input.Color*lightIntensity + tex2D(Texture0Sampler, input.TextureCoordinate)*0.8);
	color.a = 1;
	return color;
}

technique Shadowed
{
    pass Pass0
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_2_0 VertexShaderShadowedFunction();
        PixelShader = compile ps_2_0 PixelShaderShadowedFunction();
    }
}

//===============================================================================
// NormalMap
//===============================================================================

struct VertexShaderNormalMapOutput
{
    float4 Position     : POSITION;
    float4 Normal    : TEXCOORD0;
};

VertexShaderNormalMapOutput VertexShaderNormalMapFunction(VertexShaderInput input)
{
    VertexShaderNormalMapOutput output;

    float4 worldPosition = mul(input.Position, World);
    float4 viewPosition = mul(worldPosition, View);

    output.Position = mul(viewPosition, Projection);
	output.Normal = input.Normal;
	output.Normal.a = output.Position.z / output.Position.w;

	return output;
}

float4 PixelShaderNormalMapFunction(VertexShaderNormalMapOutput input) : COLOR0
{
	//this is equivalent to the depth, with values between 0 and 1 (between near clip Plane and far clip Plane)
	//return input.Position2D.z/input.Position2D.w;
	return input.Normal;
}

technique NormalMap
{
    pass Pass0
    {
        // TODO: set renderstates here.

        VertexShader = compile vs_2_0 VertexShaderNormalMapFunction();
        PixelShader = compile ps_2_0 PixelShaderNormalMapFunction();
    }
}