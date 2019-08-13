using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class MouseOperations : MonoBehaviour
    {
        public Transform indicator;

        public CameraManager cameraManager;

        private void Start()
        {
           // cameraManager = Camera.main.transform.GetComponentInParent<CameraManager>();
        }

        void Update()
        {
            DetectNode();
            HandleFLoors();
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
                    indicator.position = n.worldPosition;
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
    }
}
