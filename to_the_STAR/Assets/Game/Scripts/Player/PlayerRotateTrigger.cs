using UnityEngine;

namespace Game.Player
{
    public class PlayerRotateTrigger : MonoBehaviour
    {
        Quaternion Up = Quaternion.Euler(new Vector3(0, 0, 180));
        Quaternion Down = Quaternion.Euler(new Vector3(0, 0, 0));
        Quaternion Right = Quaternion.Euler(new Vector3(0, 0, 90));
        Quaternion Left = Quaternion.Euler(new Vector3(0, 0, 270));

        Vector3 Up_V = new Vector3(0, 1.05f, 0);
        Vector3 Down_V = new Vector3(0, -1.05f, 0);
        Vector3 Right_V = new Vector3(1.05f, 0, 0);
        Vector3 Left_V = new Vector3(-1.05f, 0, 0);

        private void Rotate(Quaternion to)
        {
            transform.rotation = to;
        }
        private void Move(Vector3 go)
        {
            transform.position = transform.position + go;
        }
        private void OnTriggerStay2D(Collider2D other)
        {
            if(other.gameObject.tag == "TurningPoint")
            {
                int turningType = other.gameObject.GetComponent<TurningPointSet>().getType();
                PlayerRotateDirection floor = GetComponent<PlayerData>().RotateDir;
                GetComponent<PlayerMovementController>().turn();
                if (turningType != 0) transform.position = other.gameObject.transform.position;
                if(floor == PlayerRotateDirection.Up)
                {
                    switch (turningType)
                    {
                        case 3: Rotate(Right); Move(Up_V); break;
                        case 4: Rotate(Left); Move(Up_V); break;
                        case 5: Rotate(Left); Move(Down_V); break;
                        case 6: Rotate(Right); Move(Down_V); break;
                    }
                }
                else if(floor == PlayerRotateDirection.Right)
                {
                    switch (turningType)
                    {
                        case 1: Rotate(Down); Move(Right_V); break;
                        case 3: Rotate(Up); Move(Right_V); break;
                        case 6: Rotate(Up); Move(Left_V); break;
                        case 8: Rotate(Down); Move(Left_V); break;
                    }
                }
                else if(floor == PlayerRotateDirection.Down)
                {
                    switch (turningType)
                    {
                        case 1: Rotate(Right); Move(Down_V); break;
                        case 2: Rotate(Left); Move(Down_V); break;
                        case 7: Rotate(Left); Move(Up_V); break;
                        case 8: Rotate(Right); Move(Up_V); break;
                    }
                }
                else if(floor == PlayerRotateDirection.Left) 
                {
                    switch (turningType)
                    {
                        case 2: Rotate(Down); Move(Left_V); break;
                        case 4: Rotate(Up); Move(Left_V); break;
                        case 5: Rotate(Up); Move(Right_V); break;
                        case 7: Rotate(Down); Move(Right_V); break;
                    }
                }
            }
        }
    }
}
