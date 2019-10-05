using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SA
{
    public class MouseOperations : MonoBehaviour
    {
        public static MouseOperations singleton;
        public CameraManager cameraManager;

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

        private Building b;

        private void Awake() {
            singleton = this;
        }
        private void Start()
        {
           // cameraManager = Camera.main.transform.GetComponentInParent<CameraManager>();
        }

        void Update()
        {
            if (action) {
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
                Debug.Log(hit.collider.gameObject.tag);
                
                if (GridManager.singleton.hitTerrain(hit))
                {
                        Vector3 buildingPosition = snapToGrid(hit.point);

                    if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()){
                        building = (GameObject)Instantiate(selectedBuilding, buildingPosition, Quaternion.identity);
                        placing = true;
                        b = building.GetComponent<Building>();
                        Renderer rend = building.GetComponent<Renderer>();
                        shaderColor = rend.material.GetColor("_Color");
                        // ERROR HERE BELOW
                       // specularColor = rend.material.GetColor("Specular");
                    } else if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject()) {
                        if (!b.inCollision) {
                            placed(hit.point);
                        } else {
                            Destroy(building);
                        }
                        placing = false;
                    } else if (placing) {
                        building.transform.position = buildingPosition;
                        if (b.inCollision) {
                            wrongPlacement();
                        } else {
                            goodPlacement();
                        }
                    }
                    
                }
            }
        }

        void wrongPlacement() {
            Renderer rend = building.GetComponent<Renderer>();

            rend.material.shader = Shader.Find("_Color");
            rend.material.SetColor("_Color", Color.red);
            rend.material.shader = Shader.Find("Specular");
            rend.material.SetColor("_SpecColor", Color.red);
        }

        void goodPlacement() {
            Renderer rend = building.GetComponent<Renderer>();

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

        public void selectHome() {
            selectedBuilding = home;
            buildingType = Building.BuildingType.Home;
            action = true;
            removing = false;
        }

        public void selectCityHall() {
            selectedBuilding = cityHall;
            buildingType = Building.BuildingType.CityHall;
            action = true;
            removing = false;
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
