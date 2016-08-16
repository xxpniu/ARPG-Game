using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class ThridPersionCameraContollor :XSingleton<ThridPersionCameraContollor>
{
    void Awake()
    {
        ThridPersionCameraContollor._instance = this;
    }
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
            targetPos =lookAt.transform.position - (Quaternion.Euler(0,rotationY,0)* forward.normalized)  * Distance;

            this.transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 5);
            this.transform.LookAt(lookAt);
		}
	}

    private Vector3 targetPos;

	public Transform lookAt;

	public float Distance = 10f;

	public Vector3 forward = new Vector3(0,-0.2f,0.5f);

	public float rotationY = 0;
}
