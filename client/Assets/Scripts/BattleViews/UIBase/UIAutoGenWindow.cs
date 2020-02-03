using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


/// <summary>
/// 自动生成UI
/// 自动生成UI是为了将UI逻辑中对资源描述的部分分离，当资源发生变化的时候不用重新写逻辑代码
/// 
/// @author:xxp
/// </summary>
public abstract class UUIAutoGenWindow : UUIWindow
{
	protected override void OnCreate()
	{
		InitTemplate();
		InitModel();
	}

	protected virtual void InitTemplate()
	{ }

	protected virtual void InitModel()
	{ }
}

