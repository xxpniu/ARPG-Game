using UnityEngine;
using System.Collections;

public class MoveDown : MonoBehaviour {

	public float Delay = 2f;
	public float speed = 1;
	public float duration = 2;

	// Use this for initialization
	IEnumerator Start () 
	{
		yield return new WaitForSeconds (Delay);
		var time = Time.time;
		while (Time.time < duration + time) {
			this.transform.position += (Vector3.down * speed * Time.deltaTime);
			yield return null;
		}
		GameObject.Destroy (this);
	}

	public static void BeginMove(GameObject obj,float speed, float delay =1, float duration =2)
	{
		var move = obj.AddComponent<MoveDown> ();
		move.Delay = delay;
		move.speed = speed;
		move.duration = duration;
	}
}
