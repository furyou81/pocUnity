using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace SA
{
	public static class SaveGame {
		static string path = "/Users/lfujimot/pocUnity/test.tt";
		// static string path = Application.persistentDataPath + "/buildings.building";
		public static void saveGame(Building[] buildings) {
			BinaryFormatter formatter = new BinaryFormatter();
			FileStream stream = new FileStream(path, FileMode.Create);
			Debug.Log("SIZE" + buildings.Length);
			BuildingData[] d = new BuildingData[buildings.Length];
			int i = 0;
			foreach (Building building in buildings) {
				BuildingData data = new BuildingData(building);
				d[i] = data;
				i++;
				//formatter.Serialize(stream, data);
			}
			formatter.Serialize(stream, d);
			stream.Close();
		}

		public static BuildingData[] loadGame() {
			if (File.Exists(path)) {
				BinaryFormatter formatter = new BinaryFormatter();
				FileStream stream = new FileStream(path, FileMode.Open);
				BuildingData[]  data = (BuildingData[])formatter.Deserialize(stream);
				Debug.Log(data.Length);
				//Building[] buildings = new Building[data.Length];
				// for (int i =0; i < data.Length; i++) {
				// 	Building b;
				// 	b.Type = (Building.BuildingType)data[i].type;
				// 	b.Position = new Vector3(data[i].position[0], data[i].position[1], data[i].position[2]);
				// 	buildings[0] = new Building();
				// }
				return data;
			} else {
				BuildingData[] buildings = new BuildingData[0];
				Debug.Log("NO DATA");
				return buildings;
			}
		}

	}
}

