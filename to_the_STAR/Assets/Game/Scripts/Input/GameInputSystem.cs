using UnityEngine.InputSystem;

namespace Game.Input
{
    public class GameInputSystem : SingletonMonoBehaviour<GameInputSystem>
    {
        public InputActions InputActions { get; private set; }
        
        protected override void OnAwake()
        {
            InputActions = new InputActions();
            
            InputActions.Enable();
        }
    }
}