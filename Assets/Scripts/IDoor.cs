using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDoor
{
    IEnumerator  OpenDoor();
    IEnumerator  CloseDoor();
    void ToggleDoor();
}
