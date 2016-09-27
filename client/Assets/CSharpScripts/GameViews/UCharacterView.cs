using UnityEngine;
using System.Collections;
using GameLogic.Game.Elements;
using System.Collections.Generic;
using GameLogic;
using EngineCore;
using Quaternion = UnityEngine.Quaternion;
using Proto;
using Vector3 = UnityEngine.Vector3;

[
	BoneName("Top","__Top"),
	BoneName("Bottom","__Bottom"),
	BoneName("Body","__Body"),
	//BoneName("HandLeft","bn_handleft"),
	//BoneName("HandRight","bn_handright")
]
public class UCharacterView : UElementView,IBattleCharacter {

    public class HpChangeTip
    {
        public int id = -1;
        public float hideTime;
        public int hp;
        public Vector3 pos;
    }

    private List<HpChangeTip> _tips = new List<HpChangeTip>();

	// Use this for initialization
	void Start () {
	
	}

    public long UserID = -1;

    public void LookAtTarget(IBattleCharacter target)
    {
        this.LookAt(target.Transform);
    }

    public void ProtertyChange(Proto.HeroPropertyType type, int finalValue)
    {
       
    }

    //public Dictionary<Proto.HeroPropertyType,int> Properties = new Dictionary<HeroPropertyType, int>();

	private string SpeedStr ="Speed";
	private Animator CharacterAnimator;
	private bool IsStop = true;
    private string TopBone ="Top";

    public int hpBar = -1;
    private float showHpBarTime =0;
    private int max;
    private int cur;
   
	// Update is called once per frame
	void Update ()
	{
        if (_tips.Count > 0)
        {
            _tips.RemoveAll(t=>t.hideTime <Time.time);
            foreach (var i in _tips)
            {
                i.id =  UUITipDrawer.Singleton.DrawHPNumber(i.id,
                    i.hp, 
                    UUIManager.Singleton.OffsetInUI(i.pos));
            }
        }

        if (showHpBarTime > Time.time)
        {
           
            hpBar = UUITipDrawer.Singleton.DrawUUITipHpBar(hpBar, 
                    cur, max,
                    UUIManager.Singleton.OffsetInUI(GetBoneByName(TopBone).position)
                );

        }

		lookQuaternion = Quaternion.Lerp (lookQuaternion, targetLookQuaternion, Time.deltaTime * this.damping);
		Character.transform.localRotation = lookQuaternion;
        if (CharacterAnimator != null)
			CharacterAnimator.SetFloat (SpeedStr, Agent.velocity.magnitude);
		
		if (bcharacter != null)
			hp = bcharacter.HP;
		if (!Agent)
			return;
        
        if (lockRotationTime<Time.time&& !IsStop && Agent.velocity.magnitude > 0 ) {
		   
			targetLookQuaternion = Quaternion.LookRotation (Agent.velocity, Vector3.up);
		}
    }

	void Awake()
	{
		Agent=this.gameObject.AddComponent<NavMeshAgent> ();

		Agent.updateRotation = false;
		Agent.updatePosition = true;
        Agent.acceleration = 20;
        Agent.radius = 0.1f;
        Agent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;

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
			if (this.Character) {
				var trans = new GTransform (this.Character.transform);
				return trans;
			}
			return null;
		}
	}
		

	public void SetPosition (EngineCore.GVector3 pos)
    {
        if (this.transform)
            this.transform.position = new Vector3(pos.x, pos.y, pos.z);
    }
		
	public string lastMotion =string.Empty;
	private float last = 0;

    public void SetAlpha(float alpha)
    {
       //do nothing
    }

	public void PlayMotion (string motion)
	{
		
		var an = CharacterAnimator;
		if (an == null)
			return;
        
		if (motion == "Hit") {
			if (last + 0.3f > Time.time)
				return;
		}
		if (IsDead)
			return;
        
        if (!string.IsNullOrEmpty(lastMotion)&& lastMotion != motion) {
			an.ResetTrigger (lastMotion);
		}
		lastMotion = motion;
		last = Time.time;
		an.SetTrigger (motion);

	}
		

    public void MoveTo (EngineCore.GVector3 position)
    {
        if (!Agent || !Agent.enabled)
            return;
        IsStop = false;
        this.Agent.Resume();
        var pos = GTransform.ToVector3(position);
        NavMeshHit hit;
        if (NavMesh.SamplePosition(pos, out  hit, 10000, this.Agent.areaMask))
        {
            targetPos = hit.position;
        }
        else
        {
            return;
        }

        if (Vector3.Distance(targetPos.Value, this.transform.position) < 0.2f)
        {
            StopMove();
            return;
        }
        this.Agent.SetDestination(targetPos.Value);
    }

    private Vector3? targetPos;

    public bool IsMoving
    {
        get
        {
            return targetPos.HasValue && Vector3.Distance(targetPos.Value, this.transform.position) > 0.2f;
        }
    }



    public void MoveToImmediate(EngineCore.GVector3 position)
    {
        
    }

	public void StopMove()
	{
		IsStop = true;
        if (!Agent ||!Agent.enabled)
			return;
		Agent.velocity = Vector3.zero;
		Agent.ResetPath();
		Agent.Stop ();
        targetPos = null;
	}

	public int hp;

	public GameObject Character{ private set; get; }

	public void SetCharacter(GameObject character)
	{
		this.Character = character;

		var collider = this.Character.GetComponent<CapsuleCollider> ();
		var gameTop = new GameObject ("__Top");
		gameTop.transform.SetParent(this.transform);
		gameTop.transform.localPosition =  new Vector3(0,collider.height,0);
		bones.Add ("Top", gameTop.transform);

		var bottom = new GameObject ("__Bottom");
		bottom.transform.SetParent( this.transform,false);
		bottom.transform.localPosition =  new Vector3(0,0,0);
		bones.Add ("Bottom", bottom.transform);

		var body = new GameObject ("__Body");
		body.transform.SetParent( this.transform,false);
		body.transform.localPosition =  new Vector3(0,collider.height/2,0);
		bones.Add ("Body", body.transform);

		CharacterAnimator= Character. GetComponent<Animator> ();
		Agent.radius = collider.radius;
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
	
    private float lockRotationTime = -1f;
	public void LookAt(ITransform target)
	{
		if (target == null)
			return;
		var v = GTransform.ToVector3 (target.Position);
		var look = v - this.transform.position;
        if (look.magnitude <= 0.01f)
            return;
		look.y = 0;
        lockRotationTime = Time.time + 0.3f;
		var qu = Quaternion.LookRotation (look, Vector3.up);
		lookQuaternion = targetLookQuaternion = qu;

	}



	public void Death ()
	{
		PlayMotion ("Die");
		StopMove ();
        showHpBarTime = -1;
		if(Agent)
		 Agent.enabled = false;
		IsDead = true;
		MoveDown.BeginMove (this.Character, 1, 1, 5);
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

    /// <summary>
    /// Not All have
    /// </summary>
    /// <returns>The battle character.</returns>
	public BattleCharacter GetBattleCharacter(){
		return bcharacter;
	}

	public void SetSpeed(float speed)
	{
		this.Agent.speed = speed;
	}

	public void SetPriorityMove (float priorityMove)
	{
		Agent.avoidancePriority = (int)priorityMove;
	}

	public void SetScale(float scale)
	{
		this.gameObject.transform.localScale = Vector3.one * scale;
	}
		

    public void ShowHPChange(int hp,int cur,int max)
    {
        if (IsDead)
            return;
       
        this.cur = cur;
        this.max = max;
        if (hp < 0)
        {
            _tips.Add(new HpChangeTip
                { 
                    id = -1, hp = hp, hideTime = Time.time + 3, pos = GetBoneByName(TopBone).position
                });
        }
        showHpBarTime = Time.time + 3;
    }

    public void ShowMPChange(int mp, int cur, int maxMP)
    {
        //throw new System.NotImplementedException();
    }

    public void AttachMaigc(int magicID, float cdCompletedTime)
    {
        if (MagicCds.ContainsKey(magicID))
        {
            MagicCds[magicID].CDTime = cdCompletedTime;
        }
        else
        {
            MagicCds.Add(magicID, new HeroMagicData{ MagicID = magicID, CDTime = cdCompletedTime});
        }
    }

    public Dictionary<int, HeroMagicData> MagicCds = new Dictionary<int, HeroMagicData>();

    public float GetCdTime(int magicKey)
    {
        HeroMagicData cd;
        if (MagicCds.TryGetValue(magicKey, out cd))
            return cd.CDTime;
        return 0;
    }

   
}
