using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SA
{
    public class PlaceBuilding : PlaceScript
    {
        public static PlaceBuilding singleton;
        private GameObject building;
        private bool placing = false;
        private Building buildingComponent;
        private Color shaderColor;
        private Color specularColor;

        private void Awake()
        {
            singleton = this;
        }

        override
        public void Place()
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 1000))
            {
                if (HitTerrain(hit))
                {
                    Vector3 buildingPosition = SnapToGrid(hit.point);

                    if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                    {
                        SetStart(buildingPosition);
                    }
                    else if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
                    {
                        SetEnd(hit.point);
                    }
                    else if (placing)
                    {
                        Adjust(buildingPosition);
                    }

                }
                else if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
                {
                    Destroy(building);
                    placing = false;
                }
            }
        }

        override
        protected void SetStart(Vector3 buildingPosition)
        {
            building = (GameObject)Instantiate(SelectBuilding.singleton.getSelectedBuilding, buildingPosition, Quaternion.identity);
            placing = true;
            buildingComponent = building.GetComponent<Building>();
            Renderer rend = building.GetComponent<Renderer>();
            shaderColor = rend.material.GetColor("_Color");
            // ERROR HERE BELOW
            // specularColor = rend.material.GetColor("Specular");
        }

        override
        protected void SetEnd(Vector3 point)
        {
            if (!buildingComponent.inCollision)
            {
                Placed(point);
            }
            else
            {
                Destroy(building);
            }
            placing = false;
        }

        override
        protected void Adjust(Vector3 buildingPosition)
        {
            building.transform.position = buildingPosition;
            if (buildingComponent.inCollision)
            {
                WrongPlacement(building);
            }
            else
            {
                GoodPlacement(building);
            }
        }

        void Placed(Vector3 hitPoint)
        {
            Renderer rend = building.GetComponent<Renderer>();

            rend.material.shader = Shader.Find("_Color");
            rend.material.SetColor("_Color", shaderColor);
            rend.material.shader = Shader.Find("Specular");
            rend.material.SetColor("_SpecColor", specularColor);
            // to be able to get Raycast for Mouse Events
            building.layer = 0;
            buildingComponent.Position = hitPoint;
            buildingComponent.LocalScale = building.transform.localScale;
            buildingComponent.Rotation = building.transform.rotation;
            GameStateManager.singleton.AddBuilding(buildingComponent);
        }
    }

}