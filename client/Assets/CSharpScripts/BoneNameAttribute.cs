using System;

[AttributeUsage( AttributeTargets.Class,AllowMultiple = true)]
public class BoneNameAttribute:Attribute
{
	public BoneNameAttribute (string name):this(name,name)
	{
		
	}

	public BoneNameAttribute (string name,string boneName):this(name,boneName,false)
	{

	}

	public BoneNameAttribute(string name,string boneName,bool temp)
	{
		this.BoneName = boneName;
		this.Name = name;
		Temp = temp;
	}

	public string BoneName{ set; get; }

	public string Name{ set; get;}

	public bool Temp{set;get;}
}


