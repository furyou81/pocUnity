using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace SA
{
    public class CameraManager : MonoBehaviour
    {

        public Transform mTransform;

        void Start()
        {
            mTransform = this.transform;
            GridManager gm = GridManager.singleton;

            Node centerNode = gm.GetNode(gm.gridSize.x / 2, gm.gridSize.y / 2, gm.gridSize.z / 2);
            transform.position = centerNode.worldPosition;
        }

        
        void Update()
        {

        }
    }
}

