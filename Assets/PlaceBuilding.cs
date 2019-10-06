using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class PlaceBuilding : MonoBehaviour
    {
        public GameObject home;
        public GameObject cityHall;
        public static PlaceBuilding singleton;
        private void Awake()
        {
            singleton = this;
        }
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }


    }

}