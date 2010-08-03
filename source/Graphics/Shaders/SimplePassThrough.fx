Texture2D tex2D;
SamplerState linearSampler
{
    Filter = MIN_MAG_MIP_LINEAR;
    AddressU = Wrap;
    AddressV = Wrap;
};

struct VS_IN
{
	float4 pos : POSITION;
	float4 col : COLOR;
	float2 Tex : TEXCOORD;
};

struct PS_IN
{
	float4 pos : SV_POSITION;
	float4 col : COLOR;
	float2 Tex : TEXCOORD;
};

PS_IN VS( VS_IN input )
{
	PS_IN output = (PS_IN)0;
	
	output.pos = input.pos;
	output.col = input.col;
	output.Tex = input.Tex;
	
	return output;
}

float4 PS( PS_IN input ) : SV_Target
{
	return input.col;
}

float4 textured( PS_IN input ) : SV_Target
{
	return tex2D.Sample( linearSampler, input.Tex );
}

float4 noTexture( PS_IN input ) : SV_Target
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