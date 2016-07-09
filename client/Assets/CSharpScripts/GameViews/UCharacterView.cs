using UnityEngine;
using System.Collections;
using GameLogic.Game.Elements;
using System.Collections.Generic;
using GameLogic;

[
	BoneName("Top","__Top"),
	BoneName("Bottom","__Bottom"),
	BoneName("Body","__Body"),
	BoneName("HandLeft","bn_handleft"),
	BoneName("HandRight","bn_handright")
]
public class UCharacterView : UElementView,IBattleCharacter {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		lookQuaternion = Quaternion.Lerp (lookQuaternion, targetLookQuaternion, Time.deltaTime * this.damping);
		Character.transform.localRotation = lookQuaternion;
	}

	void Awake()
	{
		Agent=this.gameObject.AddComponent<NavMeshAgent> ();
		Agent.updateRotation = false;

	}

	private NavMeshAgent Agent;

	private Dictionary<string ,Transform > bones = new Dictionary<string, Transform>();

	public void SetForward (EngineCore.GVector3 eulerAngles)
	{
		this.transform.localRotation = Quaternion.Euler (eulerAngles.x, eulerAngles.y, eulerAngles.z);
	}
	public ITransform Transform {
		get 
		{
			return new GTransform (transform);
		}
	}
		

	public void SetPosition (EngineCore.GVector3 pos)
	{
		this.transform.position = new Vector3 (pos.x, pos.y, pos.z);
	}
		

	public void PlayMotion (string motion)
	{
		var an =Character. GetComponent<Animator> ();
		an.SetTrigger (motion);
	}

	public GameObject Character{ private set; get; }

	public void SetCharacter(GameObject character)
	{
		this.Character = character;

		var collider = this.Character.GetComponent<CapsuleCollider> ();
		var gameTop = new GameObject ("__Top");
		gameTop.transform.parent = this.transform;
		gameTop.transform.localPosition =  new Vector3(0,collider.height,0);
		gameTop.transform.localRotation = Quaternion.identity;
		bones.Add ("Top", gameTop.transform);

		var bottom = new GameObject ("__Bottom");
		bottom.transform.parent = this.transform;
		bottom.transform.localPosition =  new Vector3(0,0,0);
		bottom.transform.localRotation = Quaternion.identity; 
		bones.Add ("Bottom", bottom.transform);

		var body = new GameObject ("__Body");
		body.transform.parent = this.transform;
		body.transform.localPosition =  new Vector3(0,collider.height/2,0);
		body.transform.localRotation = Quaternion.identity; 
		bones.Add ("Body", body.transform);


	}
		
	private List<string> GetBoneInfo(string name,bool haveTemp)
	{
		var att = typeof(UCharacterView).GetCustomAttributes(typeof(BoneNameAttribute),false) as BoneNameAttribute[];
		List<string> tnames = new List<string> ();
		List<string> tbones = new List<string> ();
		foreach (var i in att) 
		{
			if (!haveTemp && i.Temp) {
				continue;
			}
			tnames.Add (i.Name);
			tbones.Add (i.BoneName);
		}
		return tbones;
	}
		
	public void LookAt(ITransform target)
	{
		var v = GTransform.ToVector3 (target.Position);
		var look = v - this.transform.position;
		var qu = Quaternion.LookRotation (look, Vector3.up);
		lookQuaternion = targetLookQuaternion = qu;
	}

	public float damping  = 5;

	public Quaternion targetLookQuaternion;

	public Quaternion lookQuaternion = Quaternion.identity;

	public Transform GetBoneByName(string name)
	{
		
		Transform bone;
		if (bones.TryGetValue (name, out bone)) {
			return bone;
		}
		return transform;
	}
}
