using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
	[System.Serializable]
	public class BuildingData {
		public int type;
		public float[] position;

		public BuildingData(Building building) {
			type = (int)building.Type;
			position = new float[3];
			position[0] = building.Position.x;
			position[1] = building.Position.y;
			position[2] = building.Position.z;
		}
	}
}