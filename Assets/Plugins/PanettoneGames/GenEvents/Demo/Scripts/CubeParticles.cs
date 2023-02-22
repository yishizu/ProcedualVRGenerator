using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PanettoneGames.GenericEvents;

public class CubeParticles : MonoBehaviour, IGameEventListener<GameObject>
{
    [SerializeField] GameObjectEvent gameObjectEvent;
    [SerializeField] ParticleSystem particleFX;

    private void OnEnable() => gameObjectEvent.RegisterListener(this);
    private void OnDisable() => gameObjectEvent.UnregisterListener(this);

    public void OnEventRaised(GameObject item)
    {
        particleFX.startColor =  item.GetComponent<ColorChanger>().Color;
        
        if(!particleFX.isPlaying)
        particleFX.Play();
    }
}
