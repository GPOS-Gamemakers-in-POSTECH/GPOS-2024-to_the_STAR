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
        float Hp = 100.0f;
        const float maxHp = 100.0f;

        public void playerDamage(float damage)
        {
            Hp -= damage;
        }
        public float playerHp()
        {
            return Hp;
        }

        public float hammerCharge()
        {
            return 0;
        }

        public float hammerCooldown()
        {
            return 0;
        }

        public float flamethrowerCooldown()
        {
            return 0;
        }
        public float flamethrowerFever()
        {
            return 0;
        }
        public int weaponSelection()
        {
            return 0;
        }
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
