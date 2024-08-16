using UnityEngine;

namespace Game.Player
{
    public class PlayerRotateTrigger : MonoBehaviour
    {
        Quaternion Up = Quaternion.Euler(new Vector3(0, 0, 180));
        Quaternion Down = Quaternion.Euler(new Vector3(0, 0, 0));
        Quaternion Right = Quaternion.Euler(new Vector3(0, 0, 90));
        Quaternion Left = Quaternion.Euler(new Vector3(0, 0, 270));

        private void Rotate(Quaternion to)
        {
            transform.rotation = to;
        }
        private void OnTriggerEnter2D(Collider2D other)
        {
            if(other.gameObject.tag == "TurningPoint")
            {
                int turningType = other.gameObject.GetComponent<TurningPointSet>().getType();
                PlayerRotateDirection floor = GetComponent<PlayerData>().RotateDir;
                if(turningType != 0 ) transform.position = other.gameObject.transform.position;
                if(floor == PlayerRotateDirection.Up)
                {
                    switch (turningType)
                    {
                        case 3: Rotate(Right); break;
                        case 4: Rotate(Left); break;
                        case 5: Rotate(Left); break;
                        case 6: Rotate(Right); break;
                    }
                }
                else if(floor == PlayerRotateDirection.Right)
                {
                    switch (turningType)
                    {
                        case 1: Rotate(Down); break;
                        case 3: Rotate(Up); break;
                        case 6: Rotate(Up); break;
                        case 8: Rotate(Down); break;
                    }
                }
                else if(floor == PlayerRotateDirection.Down)
                {
                    switch (turningType)
                    {
                        case 1: Rotate(Right); break;
                        case 2: Rotate(Left); break;
                        case 7: Rotate(Left); break;
                        case 8: Rotate(Right); break;
                    }
                }
                else if(floor == PlayerRotateDirection.Left) 
                {
                    switch (turningType)
                    {
                        case 2: Rotate(Down); break;
                        case 4: Rotate(Up); break;
                        case 5: Rotate(Up); break;
                        case 7: Rotate(Down); break;
                    }
                }
            }
        }
    }
}
