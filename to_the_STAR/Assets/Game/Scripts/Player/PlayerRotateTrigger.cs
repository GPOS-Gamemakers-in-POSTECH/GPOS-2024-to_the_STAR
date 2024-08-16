using UnityEngine;

namespace Game.Player
{
    public class PlayerRotateTrigger : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.gameObject.tag == "TurningPoint")
            {
                int turningType = other.gameObject.GetComponent<TurningPointSet>().getType();
                PlayerRotateDirection floor = GetComponent<PlayerData>().RotateDir;
                Debug.Log(turningType);
                if(floor == PlayerRotateDirection.Up)
                {
                    switch (turningType)
                    {
                        case 3: break;
                        case 4: break;
                        case 5: break;
                        case 6: break;
                    }
                }
                if(floor == PlayerRotateDirection.Right)
                {
                    switch (turningType)
                    {
                        case 1: break;
                        case 3: break;
                        case 6: break;
                        case 8: break;
                    }
                }
                if(floor == PlayerRotateDirection.Down)
                {
                    switch (turningType)
                    {
                        case 1: break;
                        case 2: break;
                        case 7: break;
                        case 8: break;
                    }
                }
                if(floor == PlayerRotateDirection.Left) 
                {
                    switch (turningType)
                    {
                        case 2: break;
                        case 4: break;
                        case 5: break;
                        case 7: break;
                    }
                }
            }
        }
    }
}
