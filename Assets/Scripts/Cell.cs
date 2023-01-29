using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Cell
{
    public Transform cell;
    public Transform origin;
    public Connector connector;

    public Cell(Transform _cell, Transform _origin)
    {
        cell = _cell;
        origin = _origin;
    }
}
