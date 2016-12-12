using UnityEngine;
using System.Collections;
using GameLogic.Game.Elements;
using System.Collections.Generic;
using GameLogic;
using EngineCore;
using Quaternion = UnityEngine.Quaternion;
using Proto;
using Vector3 = UnityEngine.Vector3;
using UMath;
using UGameTools;
using EngineCore.Simulater;

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
		Agent=this.gameObject.AddComponent<UnityEngine.AI.NavMeshAgent> ();
        trans = new UTransform();
		Agent.updateRotation = false;
		Agent.updatePosition = true;
        Agent.acceleration = 20;
        Agent.radius = 0.1f;
        Agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance;

	}

	private UnityEngine.AI.NavMeshAgent Agent;
    private UTransform trans;
    public string lastMotion =string.Empty;
    private float last = 0;
	private Dictionary<string ,Transform > bones = new Dictionary<string, Transform>();
    private Vector3? targetPos;

    public int hp;
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

    private BattleCharacter bcharacter;

    /// <summary>
    /// Not All have
    /// </summary>
    /// <returns>The battle character.</returns>
    public BattleCharacter GetBattleCharacter(){
        return bcharacter;
    }


    private float lockRotationTime = -1f;
    void LookAt(UTransform target)
    {
        if (target == null)
            return;
        var v = target.position.ToUVer3();
        var look = v - this.transform.position;
        if (look.magnitude <= 0.01f)
            return;
        look.y = 0;
        lockRotationTime = Time.time + 0.3f;
        var qu = Quaternion.LookRotation (look, Vector3.up);
        lookQuaternion = targetLookQuaternion = qu;

    }

    private void stopMove()
    {
        IsStop = true;
        if (!Agent ||!Agent.enabled)
            return;
        Agent.velocity = Vector3.zero;
        Agent.ResetPath();
        Agent.Stop ();
        targetPos = null;
    }



    public Dictionary<int, HeroMagicData> MagicCds = new Dictionary<int, HeroMagicData>();

    public float GetCdTime(int magicKey)
    {
        HeroMagicData cd;
        if (MagicCds.TryGetValue(magicKey, out cd))
            return cd.CDTime;
        return 0;
    }

    public override void OnAttachElement(GObject el)
    {
        base.OnAttachElement(el);
        bcharacter = el as BattleCharacter;
    }

    #region impl
    void IBattleCharacter.SetForward (UVector3 forward)
	{
        this.transform.forward = forward.ToUVer3();
	}

    UTransform IBattleCharacter.Transform {
		get 
		{
            trans.localPosition = transform.localPosition.ToGVer3();
            trans.localRotation = transform.localRotation.ToGQu();
            trans.localScale = transform.localScale.ToGVer3();
            return trans;
		}
	}

    void IBattleCharacter.SetPosition (UVector3 pos)
    {
        this.transform.localPosition = pos.ToUVer3();
    }

    void IBattleCharacter.LookAtTarget(IBattleCharacter target)
    {
        this.LookAt(target.Transform);
    }

    void IBattleCharacter.ProtertyChange(Proto.HeroPropertyType type, int finalValue)
    {

    }

    void IBattleCharacter.SetAlpha(float alpha)
    {
       //do nothing
    }

    void IBattleCharacter.PlayMotion (string motion)
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
		

    void IBattleCharacter.MoveTo (UVector3 position)
    {
        if (!Agent || !Agent.enabled)
            return;
        IsStop = false;
        this.Agent.Resume();
        var pos = position.ToUVer3();
        UnityEngine.AI.NavMeshHit hit;
        if (UnityEngine.AI.NavMesh.SamplePosition(pos, out  hit, 10000, this.Agent.areaMask))
        {
            targetPos = hit.position;
        }
        else
        {
            return;
        }

        if (Vector3.Distance(targetPos.Value, this.transform.position) < 0.2f)
        {
            stopMove();
            return;
        }
        this.Agent.SetDestination(targetPos.Value);
    }

    bool IBattleCharacter.IsMoving
    {
        get
        {
            return targetPos.HasValue && Vector3.Distance(targetPos.Value, this.transform.position) > 0.2f;
        }
    }
        
    void IBattleCharacter.StopMove()
    {
        stopMove();
	}

    void IBattleCharacter.Death ()
	{
        var view = this as IBattleCharacter;
		view.PlayMotion ("Die");
		view.StopMove ();
        showHpBarTime = -1;
		if(Agent)
		 Agent.enabled = false;
		IsDead = true;
		MoveDown.BeginMove (this.Character, 1, 1, 5);
	}


    void IBattleCharacter.SetSpeed(float speed)
    {
        this.Agent.speed = speed;
    }

    void IBattleCharacter.SetPriorityMove (float priorityMove)
    {
        Agent.avoidancePriority = (int)priorityMove;
    }

    void IBattleCharacter.SetScale(float scale)
    {
        this.gameObject.transform.localScale = Vector3.one * scale;
    }


    void IBattleCharacter.ShowHPChange(int hp,int cur,int max)
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

    void IBattleCharacter.ShowMPChange(int mp, int cur, int maxMP)
    {
        //throw new System.NotImplementedException();
    }

    void IBattleCharacter.AttachMaigc(int magicID, float cdCompletedTime)
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
    #endregion

   
}
