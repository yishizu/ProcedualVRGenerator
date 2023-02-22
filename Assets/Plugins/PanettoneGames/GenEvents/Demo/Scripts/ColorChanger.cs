using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class ColorChanger : MonoBehaviour
{
    [SerializeField] Color[] colors;
    [SerializeField] [Range(0.1f, 2f)] float fadeSpeed = 0.5f;
    private Renderer rend;
    private int lastIndex;
    private Coroutine cr;
    private int currentindex;
    private Color color;

    public Color Color => color;

    private void Awake() => rend = GetComponent<Renderer>();
    private void OnTriggerEnter(Collider other)
    {
        while (currentindex == lastIndex)
            currentindex = UnityEngine.Random.Range(0, colors.Length - 1);

        color = colors[currentindex];
        rend.material.color = color;

        rend.material.EnableKeyword("_EMISSION");
        rend.material.SetColor("_EmissionColor", color);
        if (cr != null)
            StopCoroutine(cr);
        cr = StartCoroutine(FadeEmission(color));
        lastIndex = System.Array.IndexOf(colors, color);

    }

    IEnumerator FadeEmission(Color c)
    {

        float elapsedTime = 0;
        float intensity;

        while (elapsedTime < fadeSpeed)
        {
            intensity = Mathf.Lerp(2, 0, (elapsedTime / fadeSpeed));
            elapsedTime += Time.deltaTime;
            rend.material.SetColor("_EmissionColor", c * intensity);
            yield return null;
        }

        rend.material.SetColor("_EmissionColor", c * 0);

        yield return null;
    }
}
