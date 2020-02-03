using UnityEngine;
using System.Collections;

public class MissileFollowPath : MonoBehaviour {

	public enum MissileMoveState
	{
		NoStart,
		Actived,
		Moveing,
		Death
	}

	public MissileMoveState state = MissileMoveState.NoStart;

	public float Speed;
	public Transform Target;


	public Transform Actived;
	public Transform Moveing;
	public Transform Death;

	// Use this for initialization
	void Start () 
	{
		if (Actived)
			Actived.gameObject.SetActive (false);
		if (Moveing)
			Moveing.gameObject.SetActive (false);
		if (Death)
			Death.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		switch (state) {
		case MissileMoveState.NoStart:
			break;
		case MissileMoveState.Actived:
			if (Actived)
				Actived.gameObject.SetActive (true);
			state = MissileMoveState.Moveing;
			if (Moveing) {
				Moveing.gameObject.SetActive (true);
				Moveing.LookAt (Target);
			}
			break;
		case MissileMoveState.Moveing:
			if (Moveing) {
				Moveing.LookAt (Target);
				Moveing.localPosition+= (Moveing.forward * Speed * Time.deltaTime);

				if (Vector3.Distance (Target.position, this.Moveing.position) <= Speed/20) {
					state = MissileMoveState.Death;
				}
			}
			break;
		case MissileMoveState.Death:
			if (Moveing) {
				Moveing.gameObject.SetActive (false);
			}
			if (Death) {
				
				Death.gameObject.SetActive (true);
				Death.localPosition = Moveing.localPosition;
			}
			state = MissileMoveState.NoStart;
			break;
		}
	}

	

	public void SetTarget(Transform target,float speed)
	{
		Target = target;
		this.Speed = speed;
		state = MissileMoveState.Actived;
	}


}
