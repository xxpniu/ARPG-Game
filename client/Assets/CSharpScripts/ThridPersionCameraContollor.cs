using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class ThridPersionCameraContollor : MonoBehaviour
{

	// Use this for initialization
	void Start () {
		//cCamera = GetComponent<Camera> ();
	}

	//private Camera cCamera;
	
	// Update is called once per frame
	void Update () 
	{
		if (lookAt != null) 
		{
			this.transform.position = lookAt.transform.position - (Quaternion.Euler(0,rotationY,0)* forward.normalized)  * Distance;
			this.transform.LookAt (lookAt);
		}
	}

	public Transform lookAt;

	public float Distance = 10f;

	public Vector3 forward = new Vector3(0,-0.2f,0.5f);

	public float rotationY = 0;
}
