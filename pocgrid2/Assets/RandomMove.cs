using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMove : MonoBehaviour {

	public float speed = 2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float x = Random.Range(0.0f, 10.0f);
		float z = Random.Range(0.0f, 10.0f);
	//	Debug.Log("EEEE" + x);
		x *= speed * Time.deltaTime;
		z *= speed * Time.deltaTime;
		transform.Translate(x, 0, z);
	}
}
