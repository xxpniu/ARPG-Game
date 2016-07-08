using UnityEngine;
using System.Collections;

public class MainUpdater : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		UAppliaction.Singleton.ChangeGate (null);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//UAppliaction appl;

	void Awake()
	{
		DontDestroyOnLoad (this.gameObject);
	}

}
