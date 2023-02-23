using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class HingeDoor : MonoBehaviour,IDoor
{
    
    public bool isOpening = false;
    private IDoor door;

    private void Awake()
    {
        door = this.GetComponent<IDoor>();
    }

    public IEnumerator OpenDoor()
    {
        if (!isOpening)
        {
            transform.parent.DOLocalRotate(new Vector3(0,-90,0), 0.2f).OnStart(() => { isOpening = true; }).OnComplete(() =>
            {

            });
        }
        yield return new WaitForSeconds(.2f);
    }

    public IEnumerator CloseDoor()
    {
        yield return new WaitForSeconds(.2f);
        if (isOpening)
        {
            transform.parent.DOLocalRotate(new Vector3(0, 0, 0), 0.2f);
            isOpening = false;
        }
    }

    public void ToggleDoor()
    {
        isOpening = true;
    }
}
