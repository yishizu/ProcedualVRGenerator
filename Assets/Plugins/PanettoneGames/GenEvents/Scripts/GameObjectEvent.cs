using UnityEngine;
namespace PanettoneGames.GenericEvents
{
    //GameObject
    [CreateAssetMenu(fileName = "New GameObject Event", menuName = "Game Events/Game Object Event", order = 51)]
    public class GameObjectEvent : BaseGameEvent<GameObject> { }
}