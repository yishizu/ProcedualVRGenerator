using PanettoneGames.GenericEvents;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class HitPublisher : MonoBehaviour
{
    [SerializeField] GameObjectEvent gameEvent;
    [SerializeField] AudioClip sfx;

    private int hitCounter;

    public AudioClip SFX => sfx;
    public int HitCounter => hitCounter;

    private void Awake() => GetComponent<BoxCollider>().isTrigger = true;
    private void OnTriggerExit(Collider other)
    {
        hitCounter++;
        gameEvent.Raise(this.gameObject);
    }
}
