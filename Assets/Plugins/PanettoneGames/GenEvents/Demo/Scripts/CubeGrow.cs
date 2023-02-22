using PanettoneGames.GenericEvents;
using System.Collections;
using UnityEngine;

public class CubeGrow : MonoBehaviour,
IGameEventListener<GameObject>
{
    [SerializeField] GameObjectEvent gameObjectEvent;
    private float speed = 0.2f;
    private Coroutine growRoutine;
    private Vector3 initialScale;

    private void Awake() => initialScale = transform.localScale;
    private void OnEnable() => gameObjectEvent.RegisterListener(this);
    private void OnDisable() => gameObjectEvent.UnregisterListener(this);

    public void OnEventRaised(GameObject item)
    {
        if (growRoutine != null)
            StopCoroutine(growRoutine);
        growRoutine = StartCoroutine(Grow());
    }

    IEnumerator Grow()
    {

        float elapsedTime = 0;

        while (elapsedTime < speed)
        {
            transform.localScale = Vector3.Lerp(Vector3.zero, initialScale, (elapsedTime / speed));
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localScale = initialScale;
        yield return null;
    }
}
