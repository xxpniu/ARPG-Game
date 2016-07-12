using UnityEngine;
using System.Collections;

public class UGameScene : MonoBehaviour {

	// Use this for initialization
	void Start () {
		startPoint.gameObject.SetActive (false);
		enemyStartPoint.gameObject.SetActive (false);
		tower.gameObject.SetActive (false);
		towerEnemy.gameObject.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
		

	public Transform startPoint;

	public Transform enemyStartPoint;

	public Transform tower;
	public Transform towerEnemy;
}
