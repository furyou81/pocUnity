using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlaceScript : MonoBehaviour
{
    public abstract void Place();
    protected abstract void SetStart(Vector3 point);
    protected abstract void Adjust(Vector3 hit);
    protected abstract void SetEnd(Vector3 point);
    protected void WrongPlacement(GameObject build)
    {
        Renderer rend = build.GetComponent<Renderer>();

        rend.material.shader = Shader.Find("_Color");
        rend.material.SetColor("_Color", Color.red);
        rend.material.shader = Shader.Find("Specular");
        rend.material.SetColor("_SpecColor", Color.red);
    }

    protected void GoodPlacement(GameObject build)
    {
        Renderer rend = build.GetComponent<Renderer>();

        rend.material.shader = Shader.Find("_Color");
        rend.material.SetColor("_Color", Color.green);
        rend.material.shader = Shader.Find("Specular");
        rend.material.SetColor("_SpecColor", Color.green);
    }

    protected Vector3 SnapToGrid(Vector3 point)
    {
        Vector3 snapPoint = new Vector3();

        snapPoint.x = Mathf.FloorToInt(point.x / 1);
        snapPoint.y = 0.5f;
        snapPoint.z = Mathf.FloorToInt(point.z / 1);

        return snapPoint;
    }

    protected bool HitTerrain(RaycastHit hit)
    {
        if (hit.collider.gameObject.tag == "terrain")
        {
            return true;
        }
        return false;
    }
}
