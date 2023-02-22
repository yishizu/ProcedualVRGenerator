using PanettoneGames.GenericEvents;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class SFXManager : MonoBehaviour,
IGameEventListener<GameObject>
{
    [SerializeField] GameObjectEvent gameObjectEvent;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    private void OnEnable() => gameObjectEvent.RegisterListener(this);
    private void OnDisable() => gameObjectEvent.UnregisterListener(this);

    public void OnEventRaised(GameObject item)
    {
        var obj = item.GetComponent<HitPublisher>();
        audioSource.PlayOneShot(obj.SFX);
    }
}
