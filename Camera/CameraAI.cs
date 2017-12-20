using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Script for camera movement and rotation towards a GameObject(usually player)
public class CameraAI : MonoBehaviour {
	public GameObject MoveTo;
	public GameObject RotateTo;
	public bool MoveTowards;
	public bool RotateTowards;
	public float MoveSpeed;
	public float RotationSpeed;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float Speed = Mathf.Sqrt (Vector3.Distance (gameObject.transform.position, MoveTo.transform.position))*MoveSpeed; //Moves the object towards MoveTo
		float RotSpeed = Mathf.Abs (gameObject.transform.rotation.eulerAngles.x - MoveTo.transform.rotation.eulerAngles.x)*RotationSpeed;
		if (MoveTowards) {
			gameObject.transform.position = Vector3.MoveTowards (gameObject.transform.position, MoveTo.transform.position, Speed);
		}
		if (RotateTowards) {
			gameObject.transform.rotation = Quaternion.LookRotation (Vector3.RotateTowards (gameObject.transform.position, -gameObject.transform.position + RotateTo.transform.position, RotSpeed, 0f));
		}
	}
}
