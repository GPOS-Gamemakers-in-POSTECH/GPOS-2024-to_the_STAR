using UnityEngine.InputSystem;

namespace Game.Input
{
    public class GameInputSystem : SingletonMonoBehaviour<GameInputSystem>
    {
        private InputActions _inputActions; 
        
        public InputActions.PlayerActions PlayerActions { get; private set; }
        public InputActions.UIActions UIActions { get; private set; }
        
        
        protected override void OnAwake()
        {
            _inputActions = new InputActions();

            PlayerActions = _inputActions.Player;
            UIActions = _inputActions.UI;
            
            _inputActions.Enable();
        }
    }
}