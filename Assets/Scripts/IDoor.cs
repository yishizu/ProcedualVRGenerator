using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDoor
{
    IEnumerable  OpenDoor();
    IEnumerable  CloseDoor();
    void ToggleDoor();
}
