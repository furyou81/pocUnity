using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
	public class Building : MonoBehaviour {

		public enum BuildingType { Home = 0, CityHall = 1, WallPole = 2, Wall = 3 }
		public BuildingType type;

		private Vector3 position;
		public Vector3 Position { get { return position; }
									set { position = value;}
								}

        private Vector3 localScale = new Vector3(1,1,1);
        public Vector3 LocalScale
        {
            get { return localScale; }
            set { localScale = value; }
        }

        private Quaternion rotation = Quaternion.identity;
        public Quaternion Rotation
        {
            get { return rotation; }
            set { rotation = value; }
        }


        int nbCol = 0;
        //private bool collision = false;
        public int NbCol
        {
            set { nbCol = value; }
        }

        public bool inCollision { get { return nbCol != 0; }}


		private void OnCollisionEnter(Collision col) {
			if (col.gameObject.tag != "terrain" && col.gameObject.name != "Collision" && col.gameObject.tag != "currentWall") {
                nbCol++;
            }
		}

		private void OnCollisionExit(Collision col) {
			if (col.gameObject.tag != "terrain" && col.gameObject.name != "Collision" && col.gameObject.tag != "currentWall") {
                nbCol--;
			}
		}

		private void OnTriggerEnter(Collider other) {
			//Debug.Log("TRIGGER ENTER");
		}

		private void OnMouseUp() {
			if (SelectBuilding.singleton.isRemoving) {
				Destroy(gameObject);
                GameStateManager.singleton.RemoveBuilding(this);
			}
		}

		

	}
}