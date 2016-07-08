#pragma strict

var buttonInterface : ButtonInterface;

var textureColor : Texture[] ;

function Start ()
{
}

function Update ()
{
	switch(buttonInterface.colIndex)
	{
		case("Color01"):
		GetComponent.<Renderer>().material.mainTexture = textureColor[0];
		break;
		
		case("Color02"):
		GetComponent.<Renderer>().material.mainTexture = textureColor[1];
		break;
		
		case("Color03"):
		GetComponent.<Renderer>().material.mainTexture = textureColor[2];
		break;
		
		case("Color04"):
		GetComponent.<Renderer>().material.mainTexture = textureColor[3];
		break;
		
		default:
		GetComponent.<Renderer>().material.mainTexture = textureColor[0];
		break;
	}
}