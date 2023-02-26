using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class PlanGenerator : MonoBehaviour
{
    [Header("Runtime")] 
    public List<Cell> cells = new List<Cell>();
    public List<Cell> tempCells = new List<Cell>();
    public List<Connector> connectedConnectors = new List<Connector>();
    public List<Connector> openConnectors = new List<Connector>();
    [Header("Prefabs")]
    public GameObject[] roomPrefabs;
    public GameObject[] startPrefabs;
    public GameObject[] doorPrefabs;
    public GameObject[] closedPrefabs;
    public KeyCode reloadKey = KeyCode.A;
    
    private Cell cellFrom, cellTo;
    
    private void Start()
    {
        GenerateSpace();
        FindConnectors();
        CreateClosed();
        CreateDoor();
    }

    private void CreateDoor()
    {
        foreach (var connector in connectedConnectors)
        {
            int index = Random.Range(0, doorPrefabs.Length);
            GameObject cellObj = createCellGameObject(doorPrefabs, name, index);
            cellObj.transform.position = connector.transform.position;
            cellObj.transform.rotation = connector.transform.rotation;
        }
    }

    private void CreateClosed()
    {
        foreach (var connector in openConnectors)
        {
            int index = Random.Range(0, closedPrefabs.Length);
            GameObject cellObj = createCellGameObject(closedPrefabs, name, index);
            cellObj.transform.position = connector.transform.position;
            cellObj.transform.rotation = connector.transform.rotation;
        }
        
    }
    private void FindConnectors()
    {
        foreach (var cell in cells)
        {
            var connectors = cell.connectors.Where(c => c.isConnected == false).ToList();
            openConnectors.AddRange(connectors);
        }

        for (int i = 0; i < cells.Count; i++)
        {
            if (i % 2 == 0)
            {
                var connected = cells[i].connectors.Where(c => c.isConnected == true).ToList();
                connectedConnectors.AddRange(connected);
            }
        }
    }

    private void GenerateSpace()
    {
        cellFrom = CreateCell(startPrefabs, "startCell");

        cells.Add(cellFrom);
        cellTo = CreateCell(roomPrefabs, "nextCell");
        MoveCell(cellFrom, cellTo);
        IsCollide();

        int count = 0;
        do
        {
            if (cellTo != null)
            {
                cellFrom = cellTo;
            }

            cellTo = CreateCell(roomPrefabs, "cell" + cells.Count);
            MoveCell(cellFrom, cellTo);

            if (IsCollide())
            {
                count++;
            }
        } while (cells.Count < 10 && count < 100);
    }

    private void Update()
    {
        if (Input.GetKeyDown(reloadKey))
        {
            SceneManager.LoadScene("Main");
        }
    }

   
    GameObject createCellGameObject(GameObject[] prefabs, string name, int index)
    {
        
        GameObject startCell = Instantiate(prefabs[index], Vector3.zero, Quaternion.identity ,transform);
        startCell.name =name;
        float yRot = Mathf.RoundToInt(Random.Range(0, 4)) * 90f;
        startCell.transform.Rotate(0,yRot,0);
        return startCell;
    }
    
    Cell CreateCell(GameObject[] prefabs, string name)
    {
        int index = Random.Range(0, prefabs.Length);
        GameObject cellObj = createCellGameObject(prefabs, name, index);
        Cell cell = new Cell(cellObj, cellObj.transform, cellObj.GetComponentsInChildren<Connector>(),index);
        tempCells.Add(cell);
        return cell;
    }

    void MoveCell(Cell startCell, Cell nextCell)
    {
        Transform fromConnector = GetRandomConnector(startCell);
        //startCell.selectedConnector.isConnected = true;
        if(fromConnector==null){return;}
        Transform toConnector = GetRandomConnector(nextCell);
        if(toConnector==null){return;}

        toConnector.SetParent(fromConnector);
        nextCell.cell.transform.SetParent(toConnector);
        toConnector.localPosition = Vector3.zero;
        toConnector.localRotation = Quaternion.identity;
        toConnector.Rotate(0,180f,0);
        nextCell.cell.transform.SetParent(transform);
        toConnector.SetParent(nextCell.cell.transform.Find("Connectors"));
    }

    

    Transform GetRandomConnector(Cell cell)
    {
        if (cell == null) return null;
        List<Connector> connectors = cell.connectors.Where(c => c.isConnected == false).ToList();
        if (connectors.Count > 0)
        {
            int index = Random.Range(0, connectors.Count);
            cell.selectedConnector = connectors[index];
            return connectors[index].transform;
        }
        return null;
    }

    bool IsCollide()
    {
        BoxCollider box = cellTo.cell.GetComponentInChildren<BoxCollider>();
        if (box == null)
        {
            box = cellTo.cell.GetComponentInChildren<MeshRenderer>().AddComponent<BoxCollider>();
            box.isTrigger = true;
        }

        Vector3 offset  = (cellTo.cell.transform.right * box.center.x) + (cellTo.cell.transform.up * box.center.y) + (cellTo.cell.transform.forward * box.center.z);
        Vector3 halfExtents = box.bounds.extents;
        List<Collider> hits = Physics
            .OverlapBox(cellTo.cell.transform.position + offset, halfExtents, Quaternion.identity, LayerMask.GetMask("Cell")).ToList();
        if (hits.Count > 0)
        {
            if (hits.Exists(x => x.transform.parent.transform != cellFrom.cell.transform && x.transform.parent.transform != cellTo.cell.transform))
            {
                DestroyImmediate(cellTo.cell);
                
                cellTo = null;
                return true;
            }
            else
            {
                cellFrom.selectedConnector.isConnected = true;
                cellTo.selectedConnector.isConnected = true;
                cells.Add(cellTo);
                return false;
            }
            
        }
        return false;
    }
    
}
