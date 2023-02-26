using System;
using System.Collections;
using System.Collections.Generic;
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
}
