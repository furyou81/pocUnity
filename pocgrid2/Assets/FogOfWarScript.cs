using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfWarScript : MonoBehaviour {

	public GameObject fogOfWarPlane;
	public Transform[] players;
	public LayerMask fogLayer;
	public float radius = 5f;
	
	private float radiusSqr { get { return radius * radius; }}

	private Mesh mesh;
	private Vector3[] vertices;
	private Color[] colors;

	// Use this for initialization
	void Start () {
		initialize();
	}
	
	// Update is called once per frame
	void Update () {
		reHide();
		foreach (Transform player in players)
		{
			Ray r = new Ray(transform.position, player.position - transform.position);
		RaycastHit hit;
		if (Physics.Raycast(r, out hit, 1000, fogLayer, QueryTriggerInteraction.Collide)) {
			for (int i = 0; i < vertices.Length; i++) {
				Vector3 v = fogOfWarPlane.transform.TransformPoint(vertices[i]);
				float dist = Vector3.SqrMagnitude(v - hit.point);
				if (dist < radiusSqr) {
					float alpha = Mathf.Min(colors[i].a, dist / radiusSqr);
					colors[i].a = alpha;
				}
			}
			
		}
		}
		updateColor();
	}

	void initialize() {
		mesh = fogOfWarPlane.GetComponent<MeshFilter>().mesh;
		vertices = mesh.vertices;
		colors = new Color[vertices.Length];
		for (int i = 0; i < colors.Length; i++) {
			colors[i] = Color.black;
		}
		updateColor();
	}

	void reHide() {
		for (int i = 0; i < colors.Length; i++) {
			Debug.Log("EE" + colors[i].a);
			if (colors[i].a != 1) {
				colors[i].a = 0.4f;
			}
		}
		updateColor();
	}

	void updateColor() {
		mesh.colors = colors;
	}
}
