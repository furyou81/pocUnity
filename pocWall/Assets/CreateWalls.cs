using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWalls : MonoBehaviour {

bool creating;
public GameObject start;
public GameObject end;
private GameObject startingWall;
private GameObject endingWall;
public GameObject wallPrefab;
GameObject wall;
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
		startingWall = (GameObject)Instantiate(start, getWorldPoint(), Quaternion.identity);
		wall = (GameObject)Instantiate(wallPrefab, startingWall.transform.position, Quaternion.identity);
	}

	void setEnd() {
		creating = false;
		placeEndingWall();
	}

	void adjust() {
		placeEndingWall();
		adjustWall();
	}

	void adjustWall() {
		startingWall.transform.LookAt(endingWall.transform.position);
		endingWall.transform.LookAt(startingWall.transform.position);
		float distance = Vector3.Distance(startingWall.transform.position, endingWall.transform.position);
		wall.transform.position = startingWall.transform.position + distance / 2 * startingWall.transform.forward;
		wall.transform.rotation = startingWall.transform.rotation;
		wall.transform.localScale = new Vector3(wall.transform.localScale.x, wall.transform.localScale.y, distance);
		//wall.GetComponent<Renderer>().material.mainTexture.wrapMode = TextureWrapMode.Repeat;
		wall.GetComponent<Renderer>().material.mainTextureScale = new Vector2(distance, wall.transform.localScale.y);
		//wall.GetComponent<Renderer>().material.SetTextureScale("wallMat", new Vector3(wall.transform.localScale.x, wall.transform.localScale.y, distance));
	}

	void placeEndingWall() {
		if (endingWall != null) {
			endingWall.transform.position = getWorldPoint();
		} else {
			endingWall = (GameObject)Instantiate(end, getWorldPoint(), Quaternion.identity);
		}
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
}
