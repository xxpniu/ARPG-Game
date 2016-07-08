var buttonInterface : ButtonInterface;

var center : Vector3;
var radius : float = 3.0;

private var angle : float = 80;
private var angleSpeed : float;

var maxHeight : float = 3.0;
private var crvHeight : AnimationCurve;
var minHeight: float = 0.5;
private var crvLower : AnimationCurve;
private var heightRate : float;

private var posMove : Vector3;
private var disableOtherClicks : boolean=false;

function Start ()
{
	heightRate = (maxHeight + minHeight) * .5;
	crvHeight = new AnimationCurve(Keyframe(minHeight,1), Keyframe(maxHeight - heightRate, 1), Keyframe(maxHeight, 0));
	crvLower = new AnimationCurve(Keyframe(minHeight,0), Keyframe(minHeight + heightRate, 1), Keyframe(maxHeight, 1));
}

function Update ()
{
	var evaHeight : float = crvHeight.Evaluate(transform.position.y);
	var evaLower : float = crvLower.Evaluate(transform.position.y);

	if(Input.GetMouseButton(0) && buttonInterface.CanIClick())
	{
		disableOtherClicks = true;

		if(Input.GetAxis("Mouse Y") < 0)
		{
			posMove.y -= Input.GetAxis("Mouse Y") * evaHeight;	
		}
		else
		{
			posMove.y -= Input.GetAxis("Mouse Y") * evaLower;
		}
		
		angleSpeed -= Mathf.Abs(Mathf.Pow(Input.GetAxis("Mouse X"),1.0)) * Mathf.Sign(Input.GetAxis("Mouse X")) * 50.0;
	}
	
	if(Input.GetMouseButtonUp(0))
	{
		disableOtherClicks = false;
	}

	angleSpeed = Mathf.Lerp(angleSpeed, 0, Time.deltaTime * 5.0);
	angle += angleSpeed * Time.deltaTime;
	
	var calRadius : float = Mathf.Lerp(radius*.5, radius, evaHeight);
		
	var desiredHorizontalPosition : Vector2;
	desiredHorizontalPosition.x = Mathf.Cos(angle * Mathf.Deg2Rad) * calRadius;
	desiredHorizontalPosition.y = Mathf.Sin(angle * Mathf.Deg2Rad) * calRadius;
	
	transform.position.x = desiredHorizontalPosition.x;
	transform.position.z = desiredHorizontalPosition.y;
					
	posMove = Vector3.Lerp(posMove, Vector3.zero, Time.deltaTime * 5.0);
	
	if(posMove.y > 0 && transform.position.y > maxHeight - heightRate)
	{
		posMove.y = Mathf.Lerp(posMove.y, 0, 1 - evaHeight);
	}
	
	transform.Translate(posMove * Time.deltaTime, Space.World);
	transform.position.y = Mathf.Clamp(transform.position.y, minHeight, maxHeight);
	
	transform.LookAt(center);
}

function CanIClick() : boolean
{
	return !disableOtherClicks;
}