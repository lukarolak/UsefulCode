using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateTowardsMouse : MonoBehaviour {
	public float sensitivity = 0.0001f;
	void Update ()
	{
		Transform c = gameObject.transform;
		c.Rotate(0, Input.GetAxis("Mouse X")* 3f, 0);
		c.Rotate(-Input.GetAxis("Mouse Y")* 3f, 0, 0);
		c.Rotate(0, 0, -Input.GetAxis("QandE")*200 * Time.deltaTime);
		if (Input.GetKey(KeyCode.Space)){
			if(Cursor.lockState == CursorLockMode.Locked)
				Cursor.lockState = CursorLockMode.None;
			else
				Cursor.lockState = CursorLockMode.Locked;
		}
	}
}
