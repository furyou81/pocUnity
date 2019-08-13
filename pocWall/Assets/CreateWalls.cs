using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWalls : MonoBehaviour {

bool creating;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		getInput();
	}

	void getInput() {
		if (Input.GetMouseButtonDown(0)){
			setStart();
		}
	}

	void setStart() {
		creating = true;
	}

	void setEnd() {
		creating = false;
	}
}
