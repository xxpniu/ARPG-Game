using System;
using GameLogic;
using UnityEngine;
using EngineCore;

public struct GTransform:ITransform
{
	public GTransform (Transform trans)
	{
		this.trans = trans;
	}


	private Transform trans;

	public static GVector3 Convent(Vector3 v)
	{
		return new GVector3 (v.x, v.y, v.z);
	}

	public static Vector3 ToVector3(GVector3 v)
	{
		return new Vector3 (v.x, v.y, v.z);
	}

	#region ITransform implementation

	public void LookAt (ITransform trans)
	{
		var t = (GTransform)trans;
		var forward = t.trans.position- this.trans.position;
		forward.y = 0;
		this.trans.rotation = Quaternion.LookRotation (forward);
	}

	public EngineCore.GVector3 Position {
		get {

			return Convent (trans.position);
		}
	}

	public EngineCore.GVector3 ForwardEulerAngles {
		get {
			return Convent (trans.rotation.eulerAngles);
		}
	}

	public EngineCore.GVector3 Forward {
		get {
			return Convent (trans.forward);
		}
	}

	#endregion

	public Quaternion Rotation {get{ return trans.rotation;}}
}
