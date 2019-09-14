using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class FogOfWarScript : MonoBehaviour {

	public GameObject fogOfWarPlane;
	public Transform[] players;
	public LayerMask fogLayer;
	public float radius = 5f;
	
	private float radiusSqr { get { return radius * radius; }}

	private Mesh mesh;
	private Vector3[] vertices;
	private Color[] colors;

	float elapsedTime = 0f;
	void Start () {
		initialize();
	}
	
	void Update () {
        elapsedTime += Time.deltaTime;
        if (elapsedTime >= 2f) {
            elapsedTime = elapsedTime % 2f;
            Thread thread = new Thread(reHide);
            thread.Start();
        }
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
