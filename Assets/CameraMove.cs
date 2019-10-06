using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour {

	public float panSpeed = 20f;
	
	void Update () {
		Vector3 cameraPosition = transform.position;

		if (Input.GetKey("w")) {
			cameraPosition.z += panSpeed * Time.deltaTime;
		}	
	}
}
