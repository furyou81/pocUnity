using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlaceBuilding : MonoBehaviour {

	enum Building {BlackSmith, Home}


	bool placing = false;
	private Building building;
	private GameObject toPlace;
	public GameObject blackSmith;
	public GameObject home;
	bool creating;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!EventSystem.current.IsPointerOverGameObject() && placing) {
			getInput();	
		}
		
	}

	void getInput() {
		if (Input.GetMouseButtonDown(0)){
			setStart();
		} else if (Input.GetMouseButtonUp(0)) {
			setEnd();
		} else {
			if (creating) {
				adjust();
			}
		}
	}

	void setStart() {
		creating = true;
		GameObject b;
		switch(building) {
			case Building.BlackSmith:
			b = blackSmith;
			break;
			case Building.Home:
			b = home;
			break;
			default:
			b = home;
			break;
		}
		toPlace = (GameObject)Instantiate(b, getWorldPoint(), Quaternion.identity);
	}

	void setEnd() {
		creating = false;
	}

	void adjust() {
		toPlace.transform.position = getWorldPoint();
	}

	Vector3 getWorldPoint() {
		Camera camera = GetComponent<Camera>();
		Ray ray = camera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if (Physics.Raycast(ray, out hit)) {
			return hit.point;
		}
		return Vector3.zero;
	}

	public void selectBlackSmith() {
		building = Building.BlackSmith;
	}

	public void selectHome() {
		building = Building.Home;
	}

	public void setPlacing() {
		placing = true;
	}

	public void unsetPlacing() {
		placing = false;
	}
}
