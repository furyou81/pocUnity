using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SA
{
    public class Node
    {
        public Vector3Int _gridPosition;
        public Vector3 _worldPosition;

        public Node(Vector3Int gp, Vector3 wp)
        {
            _gridPosition = gp;
            _worldPosition = wp;
        }

        public Vector3Int gridPosition
        {
            get
            {
                return _gridPosition;
            }
        }

        public Vector3 worldPosition
        {
            get
            {
                return _worldPosition;
            }
        }
    }
}

