using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
	public class BuildingCollision : MonoBehaviour {

		private bool collision = false;
		
		public bool inCollision { get { return collision; }}

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		private void OnCollisionEnter(Collision col) {
			if (col.gameObject.name != "Floor" && col.gameObject.name != "Collision") {
				Debug.Log("COLLISION" + col.gameObject.name);
				collision = true;
			}
		}

		private void OnCollisionExit(Collision col) {
			if (col.gameObject.name != "Floor" && col.gameObject.name != "Collision") {
				Debug.Log("COLLISION EXIT" + col.gameObject.name);
				collision = false;
			}
		}

		private void OnTriggerEnter(Collider other) {
			Debug.Log("TRIGGER");
		}

		private void OnMouseOver()
		{
			Debug.Log("MOUSE OVER");
		}

		private void OnMouseEnter() {
			Debug.Log("MOUSE ENTER");
		}

		private void OnMouseDown() {
			Debug.Log("MOUSE DOWN");
		}

		private void OnMouseUp() {
			Debug.Log("MOUSE OVER");
			if (MouseOperations.singleton.isRemoving) {
				Destroy(gameObject);
			}
		}


	}
}