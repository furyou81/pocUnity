using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreateWall : MonoBehaviour
{

    bool wallSelected = false;

    bool creating;
    public float maxWallSize = 2.0f;
    public GameObject polePrefab;
    private GameObject startingWall;
    private GameObject endingWall;
    public GameObject wallPrefab;
    public Camera cam;

    GameObject wall;
    List<GameObject> walls = new List<GameObject>();
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (!EventSystem.current.IsPointerOverGameObject() && wallSelected)
        {
            getInput();
        }
    }

    void getInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            setStart();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            setEnd();
        }
        else
        {
            if (creating)
            {
                adjust();
            }
        }
    }

    void setStart()
    {
        creating = true;
        startingWall = (GameObject)Instantiate(polePrefab, getWorldPoint(), Quaternion.identity);
        wall = (GameObject)Instantiate(wallPrefab, startingWall.transform.position, Quaternion.identity);
        walls.Add(startingWall);
    }

    void setEnd()
    {
        creating = false;
        placeEndingWall();
    }

    void adjust()
    {
        placeEndingWall();
        adjustWall();
    }

    void adjustWall()
    {

        walls[walls.Count - 1].transform.LookAt(endingWall.transform.position);
        
        float distance = Vector3.Distance(startingWall.transform.position, endingWall.transform.position);
        if (distance >= maxWallSize)
        {
            distance = maxWallSize;
            wall.transform.position = startingWall.transform.position + distance / 2 * startingWall.transform.forward;
            wall.transform.rotation = startingWall.transform.rotation;
            wall.transform.localScale = new Vector3(wall.transform.localScale.x, wall.transform.localScale.y, distance);
            wall.GetComponent<Renderer>().material.mainTextureScale = new Vector2(distance, wall.transform.localScale.y);
            walls.Add(wall);
            startingWall = (GameObject)Instantiate(polePrefab, startingWall.transform.position + distance * startingWall.transform.forward, Quaternion.identity);
            walls.Add(startingWall);
            wall = (GameObject)Instantiate(wallPrefab, startingWall.transform.position, Quaternion.identity);
        }
    }

    void placeEndingWall()
    {
        if (endingWall != null)
        {
            endingWall.transform.position = getWorldPoint();
        }
        else
        {
            endingWall = (GameObject)Instantiate(polePrefab, getWorldPoint(), Quaternion.identity);
        }
    }

    Vector3 getWorldPoint()
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            return hit.point;
        }
        return Vector3.zero;
    }

    private void OnCollisionEnter(Collision other)
    {
        Debug.Log("COLLITION");
    }

    public void selectWall()
    {
        wallSelected = true;
    }

    public void unSelectWall()
    {
        wallSelected = false;
    }
}
