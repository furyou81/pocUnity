using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class GridManager : MonoBehaviour
    {
        public Vector2Int gridSizeXZ = new Vector2Int(30, 30);

        public Vector3Int gridSize { get { return new Vector3Int(gridSizeXZ.x, 1, gridSizeXZ.y); }}
        public float scaleXZ = 1;
        public float scaleY = 2;
        public Node[,,] grid;

        GameObject nodePrefab;

        List<GameObject> floorParents = new List<GameObject>();

        List<Building> buildings = new List<Building>();

        public void addBuilding(Building b) {
            buildings.Add(b);
        }

        public void removeBuilding(Building b) {
            buildings.Remove(b);
        }

        public static GridManager singleton;

        private void Awake()
        {
            singleton = this;
            nodePrefab = Resources.Load("NodePrefab") as GameObject;
            createGrid();
        }

        void createGrid()
        {
            grid = new Node[gridSize.x, gridSize.y, gridSize.z];

            for (int y = 0; y < gridSize.y; y++)
            {
                GameObject floorParent = new GameObject(y.ToString());
                floorParents.Add(floorParent);

                for (int x = 0; x < gridSize.x; x++)
                {

                    for (int z = 0; z < gridSize.z; z++)
                    {
                        Vector3Int gp = Vector3Int.zero;
                        gp.x = x;
                        gp.y = y;
                        gp.z = z;

                        Vector3 wp = Vector3.zero;
                        wp.x = x * scaleXZ;
                        wp.y = y * scaleY;
                        wp.z = z * scaleXZ;

                        Node n = new Node(gp, wp);

                        grid[x, y, z] = n;

                        GameObject go = Instantiate(nodePrefab);
                        go.transform.position = n.worldPosition;
                        go.transform.parent = floorParent.transform;
                    }
                }
            }
        }

        public Node GetNode(Vector3 p)
        {
            int x = Mathf.FloorToInt(p.x / scaleXZ);
            int y = Mathf.FloorToInt(p.y / scaleY);
            int z = Mathf.FloorToInt(p.z / scaleXZ);

            return GetNode(x, y, z);
        }

        public Node GetNode(Vector3Int p)
        {
            return GetNode(p.x, p.y, p.z);
        }

        public Node GetNode(int x, int y, int z)
        {
            if (x < 0 || x > gridSize.x - 1 || y < 0 || y > gridSize.y - 1 || z < 0 || z > gridSize.z - 1)
            {
                return null;
            }

            return grid[x, y, z];
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

