using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class SelectBuilding : MonoBehaviour
    {
        public GameObject homePrefab;
        public GameObject cityHallPrefab;
        public GameObject wallPrefab;
        public GameObject polePrefab;

        public static SelectBuilding singleton;

        private GameObject selectedBuilding;
        private Building.BuildingType buildingType;
        private bool buildingIsSelected = false;
        private bool wallIsSelected = false;
        private bool removing = false;
        

        public GameObject getSelectedBuilding { get { return selectedBuilding; } }
        public bool isBuildingSelected { get { return buildingIsSelected; } }
        public bool isWallSelected { get { return wallIsSelected; } }
        public bool isRemoving { get { return removing; } }


        void Awake()
        {
            singleton = this;
        }


        public void SelectHome()
        {
            selectedBuilding = homePrefab;
            buildingType = Building.BuildingType.Home;
            buildingIsSelected = true;
            removing = false;
            wallIsSelected = false;
        }

        public void SelectCityHall()
        {
            selectedBuilding = cityHallPrefab;
            buildingType = Building.BuildingType.CityHall;
            buildingIsSelected = true;
            removing = false;
            wallIsSelected = false;
        }

        public void SelectWall()
        {
            wallIsSelected = true;
            buildingIsSelected = false;
        }

        public void StopBuilding()
        {
            buildingIsSelected = false;
            removing = false;
        }

        public void RemoveBuilding()
        {
            removing = true;
            buildingIsSelected = false;
        }
    }

}