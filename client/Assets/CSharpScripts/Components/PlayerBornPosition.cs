using UnityEngine;
using System.Collections;
using UGameTools;

public class PlayerBornPosition : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        GExtends.DrawSphere(this.transform.position, 3);
    }
}
