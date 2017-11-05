using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PanelHightAdjust : MonoBehaviour {
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		float Ysize = GameManager._MainCanvas.GetComponent<RectTransform> ().position.y; //gets Vertical size of an canvas
		float YsizeBefore = 0;
		if (Ysize != YsizeBefore) { //Checks if Canvas size has been changed
			YsizeBefore = Ysize;
			RectTransform[] AllPanelTransforms = GameManager._MainCanvas.transform.GetComponentsInChildren<RectTransform> ();
			for (int i = 0; i < AllPanelTransforms.Length; i++) {
				if (AllPanelTransforms [i].gameObject.layer == 8) {
					AllPanelTransforms [i].sizeDelta = new Vector2 (AllPanelTransforms [i].sizeDelta.x, Ysize * 2);
				}
			}
		}

	}
}
