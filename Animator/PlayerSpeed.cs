using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeed : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		
	}
	IEnumerator Wait()
	{
		Vector3 Start = gameObject.transform.position;
		yield return new WaitForSeconds(Time.deltaTime);
		Vector3 End = gameObject.transform.position;
		float Speed = Vector3.Distance (Start, End) / Time.deltaTime;
		gameObject.GetComponent<Animator> ().SetFloat("Speed", Speed);
			
	}

	// Update is called once per frame
	void Update () {
		StartCoroutine(Wait());
	}
}
