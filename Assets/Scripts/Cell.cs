using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class Cell
{
    public GameObject cell;
    public Transform origin;
    public Connector[] connectors;
    public Connector selectedConnector;
    public int index;


    public Cell(GameObject _cell, Transform _origin, Connector[] _connectors, int _index)
    {
        cell = _cell;
        origin = _origin;
        connectors = _connectors;
        index = _index;
    }

    public void Reset()
    {
        selectedConnector = null;
    }

    public void AddBoxCollider()
    {
        
        GameObject g = new GameObject();
        g.transform.SetParent(cell.transform);
        g.name = "Collider";
        var box =g.AddComponent<BoxCollider>();
        box.isTrigger = true;

        Bounds bounds = cell.GetComponentInChildren<Renderer>().bounds;
        
        var allDescendants = cell.GetComponentsInChildren<Transform>();
        
        foreach (Transform desc in allDescendants)
        {
            Renderer childRenderer = desc.GetComponent<Renderer>();
            if (childRenderer != null)
            {
                bounds.Encapsulate(childRenderer.bounds);
            }
            
        }
        box.center = bounds.center - cell.transform.localPosition;
        box.size = bounds.size;
        
       
    }
    
}
