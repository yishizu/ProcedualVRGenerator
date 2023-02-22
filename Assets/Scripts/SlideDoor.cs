using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class SlideDoor : MonoBehaviour,IDoor
{
    private bool isOpening = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public IEnumerable OpenDoor()
    {
        if (!isOpening)
        {
            transform.DOLocalMoveX(4f, 1f).OnStart(() => { isOpening = true; }).OnComplete(() =>
            {
                isOpening = false;
                
               
            });
            yield return CloseDoor();
        }
    }

    public IEnumerable CloseDoor()
    {
        yield return new WaitForSeconds(2f);
        transform.DOLocalMoveX(0f, 1f);
        
    }

    public void ToggleDoor()
    {
        
    }
}
