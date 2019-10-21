using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
	[System.Serializable]
	public class BuildingData {
		public int type;
		public float[] position;
        public float[] localScale;
        public float[] rotation;

        public BuildingData(Building building) {
			type = (int)building.type;
			position = new float[3];
            localScale = new float[3];
            rotation = new float[4];
            position[0] = building.Position.x;
			position[1] = building.Position.y;
			position[2] = building.Position.z;
            localScale[0] = building.LocalScale.x;
            localScale[1] = building.LocalScale.y;
            localScale[2] = building.LocalScale.z;
            rotation[0] = building.Rotation.x;
            rotation[1] = building.Rotation.y;
            rotation[2] = building.Rotation.z;
            rotation[3] = building.Rotation.w;
        }
	}
}