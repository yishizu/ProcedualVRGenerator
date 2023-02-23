using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SlideDoor : MonoBehaviour,IDoor
{
    private bool isOpening = false;

    public IEnumerator  OpenDoor()
    {
        if (!isOpening)
        {
            transform.DOLocalMoveX(1f, 0.2f).OnStart(() => { isOpening = true; }).OnComplete(() =>
            {
               
                
            });
        }
        yield return new WaitForSeconds(.2f);
    }

    public IEnumerator  CloseDoor()
    {
        yield return new WaitForSeconds(.2f);
        if (isOpening)
        {
            transform.DOLocalMoveX(0f, 0.2f);
            isOpening = false;
        }


    }

    public void ToggleDoor()
    {
        isOpening = true;
    }
}
