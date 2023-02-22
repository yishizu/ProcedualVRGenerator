using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractAutomatic : MonoBehaviour
{
    [SerializeField] private GameObject doorGameObject;

    private IDoor door;

    private void Awake()
    {
        door = doorGameObject.GetComponent<IDoor>();
    }

    private void OnTriggerEnter(Collider other)
    {

        Debug.Log("hit");
        if (other.GetComponent<CapsuleCollider>() != null)
        {
            door.OpenDoor();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        Debug.Log("hit");
        if (other.GetComponent<CapsuleCollider>() != null)
        {
            door.CloseDoor();
        }
    }
}
