using UnityEngine;
using System.Collections;
using UGameTools;

public class MonsterGroupPosition : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    [Header("Boss")]
    public bool CanBeBoss;

    [Header("Radius")]
    public float radius = 3;

    void OnDrawGizmos()
    {
        Color defaultColor = Gizmos.color;
        Gizmos.color = CanBeBoss?Color.red: Color.green;
        GExtends.DrawSphere(this.transform.position, radius);
        Gizmos.color = defaultColor;
    }

   
}
