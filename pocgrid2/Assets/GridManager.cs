using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class GridManager : MonoBehaviour
    {
        public Vector3Int gridSize = new Vector3Int(30, 10, 30);
        public float scaleXZ = 1;
        public float scaleY = 2;
        public Node[,,] grid;

        GameObject nodePrefab;

        List<GameObject> floorParents = new List<GameObject>();

        public static GridManager singleton;

        private void Awake()
        {
            singleton = this;
            nodePrefab = Resources.Load("NodePrefab") as GameObject;
            createGrid();
            changeFloorActual(gridSize.y / 2);
          //  CreateCollision();
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

        int floorIndex;

        public float getFloorHeight()
        {
            return floorIndex * scaleY;
        }

        public float ChangeFloor(bool increment)
        {
            if (increment)
            {
                floorIndex++;
            } else
            {
                floorIndex--;
            }
            if (floorIndex < 0)
            {
                floorIndex = 0;
            }
            if (floorIndex > floorParents.Count - 1)
            {
                floorIndex = floorParents.Count - 1;
            }

            return changeFloorActual(floorIndex);
        }

        float changeFloorActual(int y)
        {
            for (int i = 0; i < floorParents.Count; i++)
            {
                floorParents[i].gameObject.SetActive(false);

                if (i <= y)
                {
                    floorParents[i].gameObject.SetActive(true);
                }
            }

            return getFloorHeight();
        }

       /* void CreateCollision()
        {
            GameObject go = new GameObject();
            BoxCollider box = go.AddComponent<BoxCollider>();
            Vector3 size = new Vector3((gridSize.z / 2) * scaleXZ, 0.01f, (gridSize.z / 2) * scaleXZ);
            box.size = size;
            size.y = 0;
            go.transform.position = size;
        }*/
    }
}

