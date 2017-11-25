using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//USING NAV MESH!!

public class CharacterController : MonoBehaviour {
	public bool UseNavMesh; //Script uses NavMeshAgent instead clasical transform.position method
	private NavMeshAgent Agent;
	public float NonNavMeshSpeed;
	public float NonNavMeshRotationSpeed;
	// Use this for initialization
	void Start () {
			Agent = gameObject.GetComponent<NavMeshAgent> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (UseNavMesh) {
			Agent.SetDestination (new Vector3 (gameObject.transform.position.x + Input.GetAxis ("Horizontal"), gameObject.transform.position.y, gameObject.transform.position.z + Input.GetAxis ("Vertical")));
		} else {
			gameObject.transform.Translate (0f, gameObject.transform.position.y, Input.GetAxis ("Vertical") * NonNavMeshSpeed, Space.Self);
			gameObject.transform.rotation = Quaternion.Euler (new Vector3 (0f,gameObject.transform.rotation.eulerAngles.y + Input.GetAxis ("Horizontal")*NonNavMeshRotationSpeed, 0f));
		}
	}
}
