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
    [Header("Prefabs")]
    public GameObject[] roomPrefabs;
    public GameObject[] startPrefabs;
    public KeyCode reloadKey = KeyCode.A;
    private Transform cellFrom, cellTo;

    [SerializeField] private GameObject player;
    [SerializeField] private GameObject camera;
    

    private void Start()
    {
        
        
        cellFrom = CreateInitialCell();
        cellTo = CreateCell(roomPrefabs, "nextCell").transform;
        MoveCell(cellFrom, cellTo);
        CollisionCheck();
        for (int i = 0; i < 10; i++)
        {
            if (cellTo != null)
            {
                cellFrom = cellTo;
            }
            
            
            cellTo = CreateCell(roomPrefabs, "cell" + i).transform;
            MoveCell(cellFrom, cellTo);
            Debug.Log(i);
            CollisionCheck();
        }
        
    }
    private void Update()
    {
        if (Input.GetKeyDown(reloadKey))
        {
            SceneManager.LoadScene("Main");
        }
    }

    GameObject CreateCell(GameObject[] prefabs, string name)
    {
        int index = Random.Range(0, prefabs.Length);
        GameObject startCell = Instantiate(prefabs[index], Vector3.zero, Quaternion.identity ,transform);
        startCell.name =name;
        float yRot = Mathf.RoundToInt(Random.Range(0, 4)) * 90f;
        startCell.transform.Rotate(0,yRot,0);
        return startCell;
    }
    Transform CreateInitialCell()
    {
        GameObject startCell = CreateCell(startPrefabs, "startCell");
        Cell cell = new Cell(startCell.transform, null);
        tempCells.Add(cell);
        cells.Add(cell);
        return startCell.transform;
    }

    Transform CreateNextCell()
    {
        
        var cell =CreateCell(roomPrefabs, "nextCell").transform;
        return cell.transform;
    }

    void MoveCell(Transform startCell, Transform nextCell)
    {
        Transform fromConnector = GetRandomConnector(startCell);
        if(fromConnector==null){return;}
        Transform toConnector = GetRandomConnector(nextCell);
        if(toConnector==null){return;}

        toConnector.SetParent(fromConnector);
        nextCell.SetParent(toConnector);
        toConnector.localPosition = Vector3.zero;
        toConnector.localRotation = Quaternion.identity;
        toConnector.Rotate(0,180f,0);
        nextCell.SetParent(transform);
        toConnector.SetParent(nextCell.Find("Connectors"));
    }

    

    Transform GetRandomConnector(Transform cell)
    {
        if (cell == null) return null;
        List<Connector> connectors = cell.GetComponentsInChildren<Connector>().Where(c => c.isConnected == false).ToList();
        if (connectors.Count > 0)
        {
            int index = Random.Range(0, connectors.Count);
            connectors[index].isConnected = true;
            return connectors[index].transform;
        }
        return null;
    }

    void CollisionCheck()
    {
        BoxCollider box = cellTo.GetComponentInChildren<BoxCollider>();
        if (box == null)
        {
            box = cellTo.GetComponentInChildren<MeshRenderer>().AddComponent<BoxCollider>();
            box.isTrigger = true;
        }

        Vector3 offset  = (cellTo.right * box.center.x) + (cellTo.up * box.center.y) + (cellTo.forward * box.center.z);
        Vector3 halfExtents = box.bounds.extents;
        List<Collider> hits = Physics
            .OverlapBox(cellTo.position + offset, halfExtents, Quaternion.identity, LayerMask.GetMask("Cell")).ToList();
        if (hits.Count > 0)
        {
            if (hits.Exists(x => x.transform.parent.transform != cellFrom && x.transform.parent.transform != cellTo))
            {
                Debug.Log("HIT:" + hits.Count);
                Debug.Log(hits[0].transform.parent.name);
                DestroyImmediate(cellTo.gameObject);
                cellTo = null;
            }
            
        }
        else
        {
            
        }
    }
}
