using UnityEngine;
using System.Collections;

public class MainUpdater : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		appl = UAppliaction.Singleton;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	UAppliaction appl;

	void Awake()
	{
		DontDestroyOnLoad (this.gameObject);
	}

}
