using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SA
{
    public class MouseOperations : MonoBehaviour
    {
        public CameraManager cameraManager;

        public GameObject home;
        public GameObject cityHall;

        private GameObject building;
        private bool action = false;
        private bool placing = false;
        private void Start()
        {
           // cameraManager = Camera.main.transform.GetComponentInParent<CameraManager>();
        }

        void Update()
        {
            if (action) {
                DetectNode();
                HandleFLoors();
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
                        building = (GameObject)Instantiate(building, buildingPosition, Quaternion.identity);
                        placing = true;
                    } else if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject()) {
                        //Instantiate(building, n.worldPosition, Quaternion.identity);
                        placing = false;
                    } else if (placing) {
                        Debug.Log("ELSE");
                        building.transform.position = buildingPosition;
                    }
                    
                }
            }
        }

        bool changedFloor;

        void HandleFLoors()
        {
            float mouseWheel = Input.GetAxis("Mouse ScrollWheel");
            if (!changedFloor)
            {
                if (mouseWheel != 0)
                {
                    bool increment = false;

             /*       if (mouseWheel < -0)
                    {

                    }*/

                    if (mouseWheel > 0)
                    {
                        increment = true;
                    }

                    float height = GridManager.singleton.ChangeFloor(increment);
                    Vector3 p = cameraManager.mTransform.position;
                    p.y = height;
                    cameraManager.mTransform.position = p;
                    changedFloor = true;
                }
            } else
            {
                if (mouseWheel == 0)
                {
                    changedFloor = false;
                }
            }
        }

        public void selectHome() {
            building = home;
            action = true;
        }

        public void selectCityHall() {
            building = cityHall;
            action = true;
        }

        public void stopBuilding() {
            action = false;
        }
    }
}
