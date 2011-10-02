Texture2D tex2D;

SamplerState linearSampler
{
    Filter = ANISOTROPIC;
    AddressU = Clamp;
    AddressV = Clamp;
};

int2 frameDimensions;

struct VS_IN
{
	float4 pos : POSITION;
	float2 tex : TEXCOORD;
	float4x4 modelView : MODELVIEW;
	int frame : FRAME;
};

struct PS_IN
{
	float4 pos : SV_POSITION;
	float4 col : COLOR;
	float2 tex : TEXCOORD;
};

PS_IN VS(VS_IN input)
{
	PS_IN output = (PS_IN)0;
	
	output.pos = input.pos;
	output.col = input.col;
	output.tex = input.tex;
	
	return output;
}

float4 PS(PS_IN input) : SV_Target
{
	return input.col;
}

float4 textured(PS_IN input) : SV_Target
{
	return tex2D.Sample(linearSampler, input.tex);
}

float4 noTexture(PS_IN input) : SV_Target
{
	return input.col;
}


technique10 Full
{
    pass P0
    {
        SetVertexShader( CompileShader( vs_4_0, VS() ) );
        SetGeometryShader( NULL );
        SetPixelShader( CompileShader( ps_4_0, textured() ) );
    }
}

technique10 TexturesDisabled
{
	pass P0
	{
		SetGeometryShader( 0 );
		SetVertexShader( CompileShader( vs_4_0, VS() ) );
		SetPixelShader( CompileShader( ps_4_0, PS() ) );
	}
}