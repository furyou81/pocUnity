using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
	public class Building : MonoBehaviour {

		public enum BuildingType { Home = 0, CityHall = 1, WallPole = 2, Wall = 3 }
		public BuildingType type;
		public BuildingType Type { get { return type; }
									set { type = value;}
								}
		private Vector3 position;
		public Vector3 Position { get { return position; }
									set { position = value;}
								}
        int nbCol = 0;
		private bool collision = false;
		
		public bool inCollision { get { return nbCol > 0; }}

		// Use this for initialization
		void Start () {
			
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		private void OnCollisionEnter(Collision col) {
			if (col.gameObject.name != "Floor" && col.gameObject.name != "Collision" && col.gameObject.tag != "currentWall") {
			//	Debug.Log("COLLISION" + col.gameObject.name + " " + col.gameObject.tag);
				collision = true;
                nbCol++;
			}
		}

		private void OnCollisionExit(Collision col) {
			if (col.gameObject.name != "Floor" && col.gameObject.name != "Collision" && col.gameObject.tag != "currentWall") {

             //   Debug.Log("COLLISION EXIT" + col.gameObject.name);
				collision = false;
                nbCol--;
			}
		}

		private void OnTriggerEnter(Collider other) {
			Debug.Log("TRIGGER");
		}

		private void OnMouseOver()
		{
			//Debug.Log("MOUSE OVER");
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
				GridManager.singleton.removeBuilding(this);
			}
		}

		

	}
}