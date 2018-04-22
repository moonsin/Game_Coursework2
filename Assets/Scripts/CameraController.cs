using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

	private float moveSpeed = 0.4f;
	private Vector3 toPos;
	private bool MoveCamera = false;

	public void goUpStairs(){
		
		toPos = new Vector3 (Camera.main.transform.position.x, this.transform.position.y + 20f);
		MoveCamera = true;

	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (MoveCamera) {
			if (Camera.main.transform.position.y < toPos.y) {
				Camera.main.transform.position = new Vector3 (Camera.main.transform.position.x, Camera.main.transform.position.y + moveSpeed, Camera.main.transform.position.z);
			}
		}
	}
}
