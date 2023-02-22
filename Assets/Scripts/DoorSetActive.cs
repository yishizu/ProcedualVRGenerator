using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DoorSetActive : MonoBehaviour, IDoor
{


    private bool isOpen = false;
    public IEnumerable OpenDoor()
    {
       gameObject.SetActive(false);
       yield return null;
    }

    public IEnumerable CloseDoor()
    {
        gameObject.SetActive(true);
        yield return null;
    }

    public void ToggleDoor()
    {
        isOpen = !isOpen;
        if (isOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }
}
