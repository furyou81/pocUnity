using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace SA
{
    public class PlaceWall : PlaceScript
    {
        public static PlaceWall singleton;

        public float maxWallSize = 2.0f;
        public Camera cam;

        private Building poleBuildingComponent;
        private Building wallBuildingComponent;
        private Color poleShaderColor;
        private Color poleSpecularColor;
        private Color wallShaderColor;
        private Color wallSpecularColor;
        private bool wrong = true;
        float currentTime = 0F;


        bool placing;
        
        
        private GameObject pole;
        private GameObject endingPole;
        private GameObject wall;
        List<Building> walls = new List<Building>();

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
                    if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                    {
                        SetStart(hit.point);
                    }
                    else if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
                    {
                        SetEnd(hit.point);
                    }
                    else
                    {
                        if (placing)
                        {
                            Adjust(hit.point);
                        }
                    }
                }
                else if (Input.GetMouseButtonUp(0) && !EventSystem.current.IsPointerOverGameObject())
                {
                    //Destroy(building);
                    placing = false;
                    Destroy(wall);
                    Destroy(endingPole);

                    Reset();
                    Debug.Log("OOOO");

                }
            }
        }

        void GenerateNewPoleAndWall(Vector3 polePosition)
        {
            pole = (GameObject)Instantiate(SelectBuilding.singleton.polePrefab, polePosition, Quaternion.identity);
            pole.tag = "currentWall";
            wall = (GameObject)Instantiate(SelectBuilding.singleton.wallPrefab, pole.transform.position, Quaternion.identity);
            wall.tag = "currentWall";

            poleBuildingComponent = pole.GetComponent<Building>();
            wallBuildingComponent = wall.GetComponent<Building>();
        }

        override
        protected void SetStart(Vector3 point)
        {
            placing = true;

            GenerateNewPoleAndWall(point);

            wallShaderColor = wall.GetComponent<Renderer>().material.GetColor("_Color");
            poleShaderColor = pole.GetComponent<Renderer>().material.GetColor("_Color");
        }


        void Placed(GameObject prefab, Building buildingComponent, Color shaderColor, Color specularColor)
        {
            Renderer rend = prefab.GetComponent<Renderer>();

            rend.material.shader = Shader.Find("_Color");
            rend.material.SetColor("_Color", shaderColor);
            rend.material.shader = Shader.Find("Specular");
            rend.material.SetColor("_SpecColor", specularColor);
            // to be able to get Raycast for Mouse Events
            prefab.layer = 0;
            //b.Position = hitPoint;
            //b.Type = buildingType;
            //GridManager.singleton.addBuilding(b);
            prefab.tag = "placed";
            //walls.Add(prefab.GetComponent<Building>());
            buildingComponent.Position = prefab.transform.position;
            buildingComponent.LocalScale = prefab.transform.localScale;
            buildingComponent.Rotation = prefab.transform.rotation;
            GameStateManager.singleton.AddBuilding(buildingComponent);
        }

        override
        protected void SetEnd(Vector3 point)
        {
            placing = false;
            AdjustEndingPole(point);
            if (poleBuildingComponent.inCollision || wallBuildingComponent.inCollision)
            {
                Destroy(wall);
                Destroy(endingPole);
            }
            else
            {
                Placed(wall, wallBuildingComponent, wallShaderColor, wallSpecularColor);
                Placed(endingPole, poleBuildingComponent, poleShaderColor, poleSpecularColor);
                wall.tag = "placed";
                endingPole.tag = "placed";
                
            }
            Debug.Log("EEEEEE");
            GameStateManager.singleton.AddWall(walls);
            Reset();
        }

        private void Reset()
        {
            pole = null;
            wall = null;
            endingPole = null;
            walls.Clear();
        }

        override
        protected void Adjust(Vector3 hit)
        {
            AdjustEndingPole(hit);
            AdjustWall();
        }

        void AdjustWall()
        {
            pole.transform.LookAt(endingPole.transform.position);

            float distance = Vector3.Distance(pole.transform.position, endingPole.transform.position);
            if (distance >= maxWallSize)
            {
                distance = maxWallSize;
            }

            wall.transform.position = pole.transform.position + distance / 2 * pole.transform.forward;
            wall.transform.rotation = pole.transform.rotation;
            wall.transform.localScale = new Vector3(wall.transform.localScale.x, wall.transform.localScale.y, 0.9f * distance);

            if (!wrong && (poleBuildingComponent.inCollision || wallBuildingComponent.inCollision))
            {
                WrongPlacement(endingPole);
                WrongPlacement(wall);
                wrong = true;
                currentTime = 0F;
            }
            else if (wrong && !poleBuildingComponent.inCollision && !wallBuildingComponent.inCollision)
            {
                GoodPlacement(endingPole);
                GoodPlacement(wall);
                wrong = false;
                currentTime = 0F;
            }
            currentTime += Time.deltaTime;
            if (currentTime > 0.2F)
            {

                if (distance >= maxWallSize && !poleBuildingComponent.inCollision && !wallBuildingComponent.inCollision)
                {
                    Placed(pole, poleBuildingComponent, poleShaderColor, poleSpecularColor);
                    Placed(wall, wallBuildingComponent, wallShaderColor, wallSpecularColor);
                    wall.GetComponent<Renderer>().material.mainTextureScale = new Vector2(distance, wall.transform.localScale.y);

                    GenerateNewPoleAndWall(pole.transform.position + distance * pole.transform.forward);
                    Destroy(endingPole);
                    endingPole = null;
                    wallBuildingComponent = wall.GetComponent<Building>();

                    currentTime = 0F;
                    wrong = true;
                }
            }
        }

        void AdjustEndingPole(Vector3 point)
        {
            if (endingPole != null)
            {
                endingPole.transform.position = point;
            }
            else
            {
                endingPole = (GameObject)Instantiate(SelectBuilding.singleton.polePrefab, point, Quaternion.identity);
                endingPole.tag = "currentWall";
                poleBuildingComponent = endingPole.GetComponent<Building>();
            }
        }
       
    }

}