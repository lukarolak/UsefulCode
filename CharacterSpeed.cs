using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSpeed : MonoBehaviour {
	public string FloatVarName;
	void Start () 
	{

	}
	IEnumerator Wait()
	{
		Vector3 Start = gameObject.transform.position;
		yield return new WaitForSeconds(Time.deltaTime);
		Vector3 End = gameObject.transform.position;
		float Speed = Vector3.Distance (Start, End) / Time.deltaTime;
		gameObject.GetComponent<Animator> ().SetFloat(FloatVarName, Speed);

	}

	// Update is called once per frame
	void Update () {
		StartCoroutine(Wait());
	}
}
