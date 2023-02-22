using PanettoneGames.GenericEvents;
using System.Collections;
using UnityEngine;

public class CamShake : MonoBehaviour,
IGameEventListener<GameObject>
{
    [SerializeField] [Range(0.05f, 1.5f)] float intensity = 0.4f;
    [SerializeField] GameObjectEvent gameObjectEvent;


    private void OnEnable() => gameObjectEvent.RegisterListener(this);
    private void OnDisable() => gameObjectEvent.UnregisterListener(this);


    public void OnEventRaised(GameObject item) => StartCoroutine(Shake(0.15f, intensity));

    IEnumerator Shake(float duration, float magnitude)
    {
        Vector3 originalPos = transform.localPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float x = Random.Range(-1f, 1f) * magnitude;
            float y = Random.Range(-1f, 1f) * magnitude;

            transform.localPosition = new Vector3(x, y, originalPos.z);

            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = originalPos;
    }
}
