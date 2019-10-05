using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SA
{
    public class MouseOperations : MonoBehaviour
    {
        public static MouseOperations singleton;

        public GameObject home;
        public GameObject cityHall;

        private GameObject selectedBuilding;
        private GameObject building;
        private Building.BuildingType buildingType;
        private bool action = false;
        private bool placing = false;
        private bool removing = false;
        public bool isRemoving { get { return removing; }}
        private Color shaderColor;
        private Color specularColor;

        private Building pb;
        private Building wb;

        bool stopWall = false;

        private Building b;

        bool wallSelected = false;

        bool creating;
        public float maxWallSize = 2.0f;
        public GameObject polePrefab;
        private GameObject startingWall;
        private GameObject endingWall;
        public GameObject wallPrefab;
        public Camera cam;

        GameObject wall;
        List<GameObject> walls = new List<GameObject>();





        private void Awake() {
            singleton = this;
        }
        private void Start()
        {
        }

        void Update()
        {
            if (action || wallSelected) {
                DetectNode();
            }
        }

        Vector3 snapToGrid(Vector3 point)
        {
            Vector3 snapPoint = new Vector3();

            snapPoint.x = Mathf.FloorToInt(point.x / 1);
            snapPoint.y = 0.5f;
            snapPoint.z = Mathf.FloorToInt(point.z / 1);

            return snapPoint;
        }

        void DetectNode()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000))
            {
                //Debug.Log(hit.collider.gameObject.tag);
                
                if (GridManager.singleton.hitTerrain(hit))
                {
                    if (action)
                    {
                        Vector3 buildingPosition = snapToGrid(hit.point);

                        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                        {
                            building = (GameObject)Instantiate(selectedBuilding, buildingPosition, Quaternion.identity);
                            placing = true;
                            b = building.GetComponent<Building>();
                            Renderer rend = building.GetComponent<Renderer>();
                            shaderColor = rend.material.GetColor("_Color");
                            // ERROR HERE BELOW
                            // specularColor = rend.material.GetColor("Specular");
                        }
                        else if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
                        {
                            Debug.Log("PPPPPPP");
                            if (!b.inCollision)
                            {
                                Debug.Log("PLQCEEEED");
                                placed(hit.point);
                            }
                            else
                            {
                                Debug.Log("DDDDDDESTROY");
                                Destroy(building);
                            }
                            placing = false;
                        }
                        else if (placing)
                        {
                            building.transform.position = buildingPosition;
                            if (b.inCollision)
                            {
                                wrongPlacement(building);
                            }
                            else
                            {
                                goodPlacement(building);
                            }
                        }
                    } else if (wallSelected && !EventSystem.current.IsPointerOverGameObject())
                    {
                        getInput();
                    }            
                } else if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
                {
                    Destroy(building);
                    placing = false;
                }
            }
        }

        void wrongPlacement(GameObject build) {
            Renderer rend = build.GetComponent<Renderer>();

            rend.material.shader = Shader.Find("_Color");
            rend.material.SetColor("_Color", Color.red);
            rend.material.shader = Shader.Find("Specular");
            rend.material.SetColor("_SpecColor", Color.red);
        }

        void goodPlacement(GameObject build) {
            Renderer rend = build.GetComponent<Renderer>();

            rend.material.shader = Shader.Find("_Color");
            rend.material.SetColor("_Color", Color.green);
            rend.material.shader = Shader.Find("Specular");
            rend.material.SetColor("_SpecColor", Color.green);
        }

        void placed(Vector3 hitPoint) {
            Renderer rend = building.GetComponent<Renderer>();

            rend.material.shader = Shader.Find("_Color");
            rend.material.SetColor("_Color", shaderColor);
            rend.material.shader = Shader.Find("Specular");
            rend.material.SetColor("_SpecColor", specularColor);
            // to be able to get Raycast for Mouse Events
            building.layer = 0;
            b.Position = hitPoint;
            b.Type = buildingType;
            GridManager.singleton.addBuilding(b);
        }

        void placedWall(GameObject w)
        {
            Renderer rend = w.GetComponent<Renderer>();

            rend.material.shader = Shader.Find("_Color");
            rend.material.SetColor("_Color", shaderColor);
            rend.material.shader = Shader.Find("Specular");
            rend.material.SetColor("_SpecColor", specularColor);
            // to be able to get Raycast for Mouse Events
            w.layer = 0;
            //b.Position = hitPoint;
            //b.Type = buildingType;
            //GridManager.singleton.addBuilding(b);
        }

        void getInput()
        {
            if (Input.GetMouseButtonDown(0))
            {
                setStart();
            }
            else if (Input.GetMouseButtonUp(0))
            {
                setEnd();
            }
            else
            {
                if (creating)
                {
                    adjust();
                }
            }
        }

        void setStart()
        {
            creating = true;

            startingWall = (GameObject)Instantiate(polePrefab, getWorldPoint(), Quaternion.identity);
            startingWall.tag = "currentWall";
            wall = (GameObject)Instantiate(wallPrefab, startingWall.transform.position, Quaternion.identity);
            wall.tag = "currentWall";
            walls.Add(startingWall);
            Renderer rend = wall.GetComponent<Renderer>();
            shaderColor = rend.material.GetColor("_Color");
            pb = startingWall.GetComponent<Building>();
            wb = wall.GetComponent<Building>();

        }

        void setEnd()
        {
            creating = false;
            placeEndingWall();
            if (pb.inCollision || wb.inCollision)
            {
                Destroy(wall);
                Destroy(endingWall);
            } else
            {
                //placedWall(startingWall);
                placedWall(wall);
                placedWall(endingWall);
                wall.tag = "placed";
                endingWall.tag = "placed";
            }
        }

        void adjust()
        {
            placeEndingWall();
            adjustWall();
        }

        void adjustWall()
        {

            walls[walls.Count - 1].transform.LookAt(endingWall.transform.position);

            

            float distance = Vector3.Distance(startingWall.transform.position, endingWall.transform.position);
            if (distance >= maxWallSize)
            {
                distance = maxWallSize;
            }
            wall.transform.position = startingWall.transform.position + distance / 2 * startingWall.transform.forward;
            wall.transform.rotation = startingWall.transform.rotation;
            wall.transform.localScale = new Vector3(wall.transform.localScale.x, wall.transform.localScale.y, 0.9f * distance);
            if (distance >= maxWallSize)
            {
                if (!pb.inCollision && !wb.inCollision)
                {
                    placedWall(startingWall);
                    placedWall(wall);
                    stopWall = false;
                } else
                {
                    stopWall = true;
                }
               // distance = maxWallSize;
                
                if (!stopWall)
                {
                    
                    wall.GetComponent<Renderer>().material.mainTextureScale = new Vector2(distance, wall.transform.localScale.y);
                    //wall.tag = "placed";
                    if (walls.Count > 2)
                    {
                        walls[walls.Count - 2].tag = "placed";
                        walls[walls.Count - 1].tag = "placed";
                    }
                    wall.tag = "placed";
                    walls.Add(wall);
                    startingWall.tag = "placed";
                    startingWall = (GameObject)Instantiate(polePrefab, startingWall.transform.position + distance * startingWall.transform.forward, Quaternion.identity);
                    startingWall.tag = "currentWall";
                    walls.Add(startingWall);
                    wall = (GameObject)Instantiate(wallPrefab, startingWall.transform.position, Quaternion.identity);
                    wall.tag = "currentWall";
                    pb = endingWall.GetComponent<Building>();
                    wb = wall.GetComponent<Building>();
                }
            }

            


            if (pb.inCollision || wb.inCollision)
            {
                wrongPlacement(endingWall);
                wrongPlacement(wall);
            }
            else
            {
                goodPlacement(endingWall);
                goodPlacement(wall);
            }
        }

        void placeEndingWall()
        {
            if (endingWall != null)
            {
                endingWall.transform.position = getWorldPoint();
            }
            else
            {
                endingWall = (GameObject)Instantiate(polePrefab, getWorldPoint(), Quaternion.identity);
                endingWall.tag = "currentWall";
            }
           pb = endingWall.GetComponent<Building>();
            
        }

        Vector3 getWorldPoint()
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                return hit.point;
            }
            return Vector3.zero;
        }

        public void selectHome() {
            selectedBuilding = home;
            buildingType = Building.BuildingType.Home;
            action = true;
            removing = false;
            wallSelected = false;
        }

        public void selectCityHall() {
            selectedBuilding = cityHall;
            buildingType = Building.BuildingType.CityHall;
            action = true;
            removing = false;
            wallSelected = false;
        }

        public void selectWall()
        {
            wallSelected = true;
            action = false;
        }

        public void unSelectWall()
        {
            wallSelected = false;
        }

        public void stopBuilding() {
            action = false;
            removing = false;
        }

        public void removeBuilding() {
            removing = true;
            action = false;
            placing = false;
        }
    }
}
