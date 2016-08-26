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

    private Vector3 current;
    private Vector3 targetForward;

	void Update ()
    {
        if (lookAt != null)
        {
            targetForward = lookAt.forward;
            targetForward.y = forward.y;
            //forward = Vector3.Lerp(forward, targetForward, Time.deltaTime * 5);
            current = Vector3.Lerp(current, lookAt.position, Time.deltaTime * 10);
            targetPos = current - (Quaternion.Euler(0, rotationY, 0) * forward.normalized) * Distance;
            this.transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 5);
            this.transform.LookAt(current);
        }
    }

    private Vector3 targetPos;

	public Transform lookAt;

	public float Distance = 10f;

	public Vector3 forward = new Vector3(0,-0.2f,0.5f);

	public float rotationY = 0;
}
