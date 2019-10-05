using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class GridManager : MonoBehaviour
    {

        List<Building> buildings = new List<Building>();

        public void addBuilding(Building b) {
            buildings.Add(b);
        }

        public void removeBuilding(Building b) {
            buildings.Remove(b);
        }

        public static GridManager singleton;

        public bool hitTerrain(RaycastHit hit)
        {
            //Debug.Log(hit.collider.tag + "TQA" + hit.collider.gameObject.tag);
            if (hit.collider.gameObject.tag == "terrain")
            {
               // Debug.Log("yes");
                return true;
            }
           // Debug.Log("no");
            return false;
        }

        private void Awake()
        {
            singleton = this;
            //nodePrefab = Resources.Load("NodePrefab") as GameObject;
           
        }


        public void saveGameState() {
			SaveGame.saveGame(buildings.ToArray());
		}

        public void restoreGameState() {

            BuildingData[] buildingsData = SaveGame.loadGame();
            buildings.Clear();
            foreach (BuildingData bd in buildingsData)
            {
                Vector3 pos = new Vector3(bd.position[0], bd.position[1], bd.position[2]);
                if ((Building.BuildingType)bd.type == Building.BuildingType.Home) {
                    GameObject go = (GameObject)Instantiate(MouseOperations.singleton.home, pos, Quaternion.identity);
                    go.layer = 0;
                    Building b = go.GetComponent<Building>();
                    b.Type = (Building.BuildingType)bd.type;
                    b.Position = pos;
                    buildings.Add(b);
                } else if ((Building.BuildingType)bd.type == Building.BuildingType.CityHall) {
                    GameObject go = (GameObject)Instantiate(MouseOperations.singleton.cityHall, pos, Quaternion.identity);
                    go.layer = 0;
                    Building b = go.GetComponent<Building>();
                    b.Type = (Building.BuildingType)bd.type;
                    b.Position = pos;
                    buildings.Add(b);
                } else {
                    Debug.Log("BUILDING TYPE NOT FOUND");
                }
            }

            // foreach (Building b in buildings) {
            //     if (b.Type == Building.BuildingType.Home) {
            //         Instantiate(MouseOperations.singleton.home, b.Position, Quaternion.identity);
            //     } else if (b.Type == Building.BuildingType.CityHall) {
            //         Instantiate(MouseOperations.singleton.cityHall, b.Position, Quaternion.identity);
            //     } else {
            //         Debug.Log("BUILDING TYPE NOT FOUND");
            //     }
                
            // }
        }
    }
}

