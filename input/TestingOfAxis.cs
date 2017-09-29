using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingOfAxis : MonoBehaviour {
	public string Axis;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		print (Input.GetAxis (Axis));
	}
}
