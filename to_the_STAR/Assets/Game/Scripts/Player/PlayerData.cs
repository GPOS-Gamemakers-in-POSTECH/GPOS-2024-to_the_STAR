using UnityEngine;

namespace Game.Player
{
    public enum PlayerRotateDirection
    {
        Up,
        Right,
        Down,
        Left
    }
    
    public class PlayerData : MonoBehaviour
    {
        public PlayerRotateDirection RotateDir
        {
            get
            {
                var rot = transform.localRotation.eulerAngles.z;

                return rot switch
                {
                    > 45 and <= 135 => PlayerRotateDirection.Right,
                    > 135 and <= 225 => PlayerRotateDirection.Up,
                    > 225 and <= 315 => PlayerRotateDirection.Left,
                    _ => PlayerRotateDirection.Down
                };
            }
        }
    }
}
