Texture2D tex2D;

cbuffer perFrame
{
	float4x4 viewProj;
};

SamplerState linearSampler
{
    Filter = ANISOTROPIC;
    AddressU = Clamp;
    AddressV = Clamp;
};



struct VS_IN
{
	float4 pos : POSITION;
	float2 tex : TEXCOORD;
	row_major float4x4 model : model;
};


struct PS_IN
{
	float4 pos : SV_POSITION;
	float2 tex : TEXCOORD;
};

PS_IN VS(VS_IN input)
{
	PS_IN output = (PS_IN)0;
	
	/*output.pos = input.pos;
	output.pos.xy = (input.pos.xy  * bounds + posi) * 2.0f - 1.0f;
	output.pos.y *= -1.0f;*/
	
	float4x4 mv = mul(input.model, viewProj);
	output.pos = mul(mul(input.pos, input.model), viewProj);
	output.tex = input.tex;
	
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