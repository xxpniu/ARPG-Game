using UnityEngine;
using System.Collections;
using GameLogic.Game.Elements;
using System.Collections.Generic;
using GameLogic;

[
	BoneName("Top","__Top"),
	BoneName("Bottom","__Bottom"),
	BoneName("Body","__Body"),
	//BoneName("HandLeft","bn_handleft"),
	//BoneName("HandRight","bn_handright")
]
public class UCharacterView : UElementView,IBattleCharacter {

	// Use this for initialization
	void Start () {
	
	}

	private string SpeedStr ="Speed";
	private Animator CharacterAnimator;
	// Update is called once per frame
	void Update ()
	{
		lookQuaternion = Quaternion.Lerp (lookQuaternion, targetLookQuaternion, Time.deltaTime * this.damping);
		Character.transform.localRotation = lookQuaternion;
		CharacterAnimator.SetFloat (SpeedStr, Agent.velocity.magnitude);
		if (bcharacter != null)
			hp = bcharacter.HP;
		if (Agent.velocity.magnitude > 0) {
		
			targetLookQuaternion = Quaternion.LookRotation (Agent.velocity, Vector3.up);
		}
	}

	void Awake()
	{
		Agent=this.gameObject.AddComponent<NavMeshAgent> ();
		Agent.updateRotation = false;
		Agent.updatePosition = true;

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
			
			var trans= new GTransform (this.Character.transform);
			return trans;
		}
	}
		

	public void SetPosition (EngineCore.GVector3 pos)
	{
		this.transform.position = new Vector3 (pos.x, pos.y, pos.z);
	}
		

	public void PlayMotion (string motion)
	{
		if (IsDead)
			return;
		var an =CharacterAnimator;
		an.SetTrigger (motion);
	}




	public void MoveTo (EngineCore.GVector3 position)
	{
		this.Agent.Resume ();
		this.Agent.SetDestination (GTransform.ToVector3 (position));
		//Agent.Stop ();
	}

	public void StopMove()
	{
		Agent.velocity = Vector3.zero;
		Agent.ResetPath();
		Agent.Stop ();
	}

	public int hp;

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

		CharacterAnimator= Character. GetComponent<Animator> ();
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

	public void Death ()
	{

		PlayMotion ("Die");
		IsDead = true;
	}

	private bool IsDead = false;

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

	public override void JoinState (EngineCore.Simulater.GObject el)
	{
		base.JoinState (el);
		bcharacter = el as BattleCharacter;
	}

	private BattleCharacter bcharacter;

	public BattleCharacter GetBattleCharacter(){
		return bcharacter;
	}
}
