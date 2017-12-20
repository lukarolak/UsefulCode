using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpeed : MonoBehaviour {
//Script that returns GameObject speed in it's local space
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
