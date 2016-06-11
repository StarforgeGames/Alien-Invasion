Texture2D tex2D;

SamplerState linearSampler
{
    Filter = MIN_MAG_MIP_POINT;
    AddressU = Clamp;
    AddressV = Clamp;
};

float2 frameDimensions;

struct VS_IN
{
	float4 pos : POSITION;
	float2 tex : TEXCOORD;
	float4x4 modelView : MODELVIEW;
	float frame : FRAME;
};


struct PS_IN
{
	float4 pos : SV_POSITION;
	float2 tex : TEXCOORD;
};

PS_IN VS(VS_IN input)
{
	PS_IN output = (PS_IN)0;

	output.pos = mul(input.modelView, input.pos);

	/*
		perform texture coordinate transformation here
	*/

	float2 framePosition;
	framePosition.x = (int)input.frame % (int)frameDimensions.x;
	framePosition.y = (int)input.frame / (int)frameDimensions.x;
	

	output.tex = (input.tex + framePosition) / frameDimensions;
	
	return output;
}

float4 PS(PS_IN input) : SV_Target
{
	return tex2D.Sample(linearSampler, input.tex);
}


technique10 Full
{
    pass P0
    {
        SetVertexShader( CompileShader( vs_4_0, VS() ) );
        SetGeometryShader( NULL );
        SetPixelShader( CompileShader( ps_4_0, PS() ) );
    }
}