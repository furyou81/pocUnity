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
        private bool action = false;
        private bool placing = false;
        private bool removing = false;
        public bool isRemoving { get { return removing; }}
        private Color shaderColor;
        private Color specularColor;

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

        void DetectNode()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 1000))
            {
                Node n = GridManager.singleton.GetNode(hit.point);
                if (n != null)
                {
                        Vector3 buildingPosition = n.worldPosition;
                            //buildingPosition.y = building.GetComponent<Renderer>().bounds.size.y / 2;
                        buildingPosition.y = 0;
                    if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()){
                        building = (GameObject)Instantiate(selectedBuilding, buildingPosition, Quaternion.identity);
                        placing = true;
                        Renderer rend = building.GetComponent<Renderer>();
                        shaderColor = rend.material.GetColor("_Color");
                        // ERROR HERE BELOW
                       // specularColor = rend.material.GetColor("Specular");
                    } else if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject()) {
                        //Instantiate(building, n.worldPosition, Quaternion.identity);
                        BuildingCollision bc = building.GetComponent<BuildingCollision>();
                        if (!bc.inCollision) {
                            placed();
                        } else {
                            Destroy(building);
                        }
                        placing = false;
                    } else if (placing) {
                        building.transform.position = buildingPosition;
                        BuildingCollision bc = building.GetComponent<BuildingCollision>();
                        if (bc.inCollision) {
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

        void placed() {
            Renderer rend = building.GetComponent<Renderer>();

            rend.material.shader = Shader.Find("_Color");
            rend.material.SetColor("_Color", shaderColor);
            rend.material.shader = Shader.Find("Specular");
            rend.material.SetColor("_SpecColor", specularColor);
            // to be able to get Raycast for Mouse Events
            building.layer = 0;
        }

        public void selectHome() {
            selectedBuilding = home;
            action = true;
            removing = false;
        }

        public void selectCityHall() {
            selectedBuilding = cityHall;
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
