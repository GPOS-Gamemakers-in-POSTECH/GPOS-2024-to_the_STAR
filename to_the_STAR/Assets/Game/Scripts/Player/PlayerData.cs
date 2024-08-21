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
            GetComponent<PlayerMovementController>().damaged();
            Hp -= damage;
        }
        public float playerHp()
        {
            return Hp/maxHp;
        }

        public float hammerCharge()
        {
            return GetComponent<Weapon_Hammer>().getHammerCharge();
        }

        public float hammerCooldown()
        {
            return GetComponent<Weapon_Hammer>().getHammerCooldown();
        }

        public float flamethrowerCooldown()
        {
            return GetComponent<Weapon_Flamethrower>().getFlameCooldown();
        }
        public float flamethrowerFever()
        {
            return GetComponent<Weapon_Flamethrower>().getFlameFever();
        }
        public int weaponSelection()
        {
            if (GetComponent<Weapon_Hammer>().isEnabledHammer()) return 0;
            if (GetComponent<Weapon_Flamethrower>().isEnabledFlame()) return 1;
            return -1;
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
