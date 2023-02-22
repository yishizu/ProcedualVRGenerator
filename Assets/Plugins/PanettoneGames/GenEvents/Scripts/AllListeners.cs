namespace PanettoneGames.GenericEvents
{
    public class GameObjectEventListener : BaseGameEventListener<UnityEngine.GameObject, GameObjectEvent> { }
    public class IntEventListener : BaseGameEventListener<int, IntEvent> { }
    public class StringEventListener : BaseGameEventListener<string, StringEvent> { }
    public class VoidEventListener : BaseGameEventListener<Void, VoidEvent> { }
}




