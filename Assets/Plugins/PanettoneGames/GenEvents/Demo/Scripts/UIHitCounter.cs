using UnityEngine;
using PanettoneGames.GenericEvents;
using TMPro;

public class UIHitCounter : MonoBehaviour,
IGameEventListener<GameObject>
{
    [SerializeField] GameObjectEvent gameObjectEvent;
    private TextMeshProUGUI scoreText;
    private void Awake() => scoreText = GetComponent<TextMeshProUGUI>();
    private void OnEnable() => gameObjectEvent.RegisterListener(this);
    private void OnDisable() => gameObjectEvent.UnregisterListener(this);

    public void OnEventRaised(GameObject item)
    {
        var x = item.GetComponent<HitPublisher>().HitCounter;
        //Debug.Log($"{x} was recived by {gameObject.name}");
        scoreText.text = ($"Hit {x} times");
    }

}

